﻿using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Data.Entities;
using HamroFashion.Api.V1.Exceptions;
using HamroFashion.Api.V1.Extensions;
using HamroFashion.Api.V1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

namespace HamroFashion.Api.V1.Services
{
    /// <summary>
    /// Our User persistence service. Use this to read/write Users to the database
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// The HamroFashionContext we persist to
        /// </summary>
        public readonly HamroFashionContext db;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="db">The HamroFashionContext to use</param>
        public UserService(HamroFashionContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Creates a new UserIdentity
        /// </summary>
        /// <param name="command">The createUser command to execute</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>UserCreated or throws exception</returns>
        public async Task<UserModel> CreateAsync(CreateUser command, CancellationToken cancellationToken, bool skipEmail = false)
        {
            var user = new UserEntity
            {
                UserName = command.UserName,
                EmailAddress = command.EmailAddress,
                PasswordHash = command.Password!.ToPasswordHash(),
            };

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Name == "User", cancellationToken);
            var userRole = new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            db.UserRoles.Add(userRole);

            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);

            if (!skipEmail)
            {
                var template = System.IO.File.ReadAllText("V1/Templates/NewEmail.htm")
                    .Replace("{{USERNAME}}", user.UserName);

                MailerBackgroundService.QueueMessage(user.EmailAddress, "Welcome to HamroFashion, please complete your registration", template);

            }
          

            return user.ToModel();
        }

        /// <summary>
        /// Get a user by unique id
        /// </summary>
        /// <param name="id">The unique id of the User to get</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>UserModel</returns>
        /// <exception cref="EntityNotFoundException">Thrown when no user found for provided id</exception>
        public async Task<UserModel> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var query = db.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .AsQueryable();

            var user = await query.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(nameof(UserEntity), userId);

            return user.ToModel();
        }

        public async Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(nameof(UserEntity), userId);
            db.Users.Remove(user);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangePasswordAsync(ChangePassword command, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            if (command.UserId != CurrentUser.GetUserId())
                throw new ApiException("Failed to update account, You are not authorized to edit this account", new Dictionary<string, string[]>
                {
                    { "id", ["target user's id doesn't match with current user"] }
                });

            var user = await db.Users
                .FirstOrDefaultAsync(x => x.Id == command.UserId);

            user.PasswordHash = command.Password!.ToPasswordHash();

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddWishListProduct(Guid productId, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();

            var user = await db.Users
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
                throw new EntityNotFoundException(nameof(UserEntity), userId);
            var product = await db.Products
                .FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);
            if (product == null)
                throw new EntityNotFoundException(nameof(ProductEntity), productId);

            var wishlistitem = new WishListItem
            {
                UserId = userId,
                ProductId = productId,
                User = user,
                Product = product
            };

            db.WishListItems.Add(wishlistitem);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveWishListProduct(Guid productId, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var wishlistitem = await db.WishListItems
                .FirstOrDefaultAsync(x => x.UserId == CurrentUser.GetUserId() && x.ProductId == productId, cancellationToken);

            if (wishlistitem == null)
            {
                throw new EntityNotFoundException(nameof(WishListItem), productId);
            }

            db.WishListItems.Remove(wishlistitem);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<CartModel> GetCart(Guid userId, CancellationToken cancellationToken)
        {
            var cart = await db.CartEntities
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart == null)
                throw new EntityNotFoundException(nameof(CartEntity), userId);

            return cart.ToModel();
        }

        public async Task AddToCart(Guid productId, Guid userId, CancellationToken cancellationToken)
        {
            var cart = await db.CartEntities
                .Include(x => x.CartItems)
                    .ThenInclude(ci => ci.Product) // Eagerly load Product
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart == null)
                throw new EntityNotFoundException(nameof(CartEntity), userId);

            var product = await db.Products
                .FirstOrDefaultAsync(x => x.Id ==  productId, cancellationToken);

            if (cart.CartItems != null && cart.CartItems.Any(x => x.ProductId == productId))
            {
                var item = cart.CartItems.First(x => x.ProductId == productId);
                item.Quantity += 1;
                
            }
            else
            {
                var cartItem = new CartItem { CartId = cart.Id, ProductId = productId, Quantity = 1, Price = product.Price };
                db.CartItems.Add(cartItem);
            }
       
            await db.SaveChangesAsync(cancellationToken);
            // Reload the cart or update the total cost directly
            cart = await db.CartEntities
                .Include(x => x.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            cart = await UpdateTotalCost(cart, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }


        public async Task RemoveFromCart(Guid productId, Guid userId, CancellationToken cancellationToken)
        {
            var cart = await db.CartEntities
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart == null)
                throw new EntityNotFoundException(nameof(CartEntity), userId);

            var item = await db.CartItems
                .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
            
            db.CartItems.Remove(item);
            await db.SaveChangesAsync(cancellationToken);
            // Reload the cart or update the total cost directly
            cart = await db.CartEntities
                .Include(x => x.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            cart = await UpdateTotalCost(cart, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task DecreaseCartQuantity(Guid productId, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();
            var cart = await db.CartEntities
                .Include(x => x.CartItems)
                    .ThenInclude(ci => ci.Product) // Eagerly load Product
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart == null)
                throw new EntityNotFoundException(nameof(CartEntity), userId);

            var item = cart.CartItems.First(x => x.ProductId == productId);
            if(item.Quantity == 0)
            {
                await RemoveFromCart(productId, userId, cancellationToken);
            }
            item.Quantity -= 1;

            await db.SaveChangesAsync(cancellationToken);
            // Reload the cart or update the total cost directly
            cart = await db.CartEntities
                .Include(x => x.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            cart = await UpdateTotalCost(cart, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        /*public async Task UpdateCartItemQuantity(UpdateCartItem command, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();

            var cart = await db.CartEntities
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (cart == null)
                throw new EntityNotFoundException(nameof(CartEntity), userId);

            if (!(cart.CartItems.Any(x => x.ProductId == command.ProductId)))
                throw new EntityNotFoundException(nameof(CartItem), cart.Id);

            var item = await db.CartItems
                .FirstOrDefaultAsync(x => x.ProductId == command.ProductId, cancellationToken);

            item.Quantity = command.Quantity;

        } Seems like addToCart is doing job */

        public async Task<CartEntity> UpdateTotalCost(CartEntity cart, CancellationToken cancellationToken)
        {
            cart.TotalPrice = cart.CartItems.Sum(item => item.Quantity * item.Product.Price);

            return cart;
        }

        public async Task<OrderModel> PlaceOrder(PlaceOrder command, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();

            var cart = await db.CartEntities
                .FirstOrDefaultAsync(x => x.Id == command.CartId, cancellationToken);

            if (cart == null)
                throw new EntityNotFoundException(nameof(CartEntity), command.CartId);

            decimal totalAmount = 0;

            foreach(var item in cart.CartItems)
            {
                totalAmount += item.Quantity * item.Price;
            }

            var order = new OrderEnity
            {
                UserId = userId,
                TotalPrice = totalAmount,
                ShippingAddress = command.PaymentMethod,
                PaymentMethod = command.PaymentMethod
            };

            var orderItems = new List<OrderItem>();

            foreach (var item in cart.CartItems)
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            order.OrderItems = orderItems;

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return order.ToModel();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            // Find the order by orderId
            // Update the status (e.g., to "Shipped", "Delivered")
        }
        public async Task<OrderModel> GetOrderDetails(Guid orderId, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var order = await db.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            if (order == null)
                throw new EntityNotFoundException(nameof(OrderEnity), orderId);

            return order.ToModel();
        }

        public async Task<List<OrderModel>> GetOrders(ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var userId = CurrentUser.GetUserId();

            var orders = await db.Orders
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            return orders.ToModel().ToList();
        }

    }
}
