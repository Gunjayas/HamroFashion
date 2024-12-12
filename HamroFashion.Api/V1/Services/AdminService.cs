using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Data.Configurations;
using HamroFashion.Api.V1.Data.Entities;
using HamroFashion.Api.V1.Exceptions;
using HamroFashion.Api.V1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HamroFashion.Api.V1.Services
{

    /// <summary>
    /// Our Admin persistence service. Use this to read/write admin stuffs to the database
    /// </summary>
    public class AdminService
    {
        /// <summary>
        /// The HamroFashionContext we persist to
        /// </summary>
        public readonly HamroFashionContext db;

        //Imageservice

        public AdminService(HamroFashionContext db)
        {
            this.db = db;
        }

        public async Task CreateTagAsync(CreateTag command, CancellationToken cancellationToken)
        {
            var tag = new Tag { Name = command.Name, Description = command.Description, TagType = command.TagType };
            await db.Tags.AddAsync(tag);
            await db.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(Guid tagId, CancellationToken cancellationToken)
        {
            var tag = await db.Tags
                .FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);

            if (tag == null)
                throw new EntityNotFoundException(nameof(Tag), tagId);

            db.Tags.Remove(tag);
            await db.SaveChangesAsync();
        }

        public async Task<List<TagModel>> GetTagsByType(string tagType, CancellationToken cancellationToken)
        {
            var tags = db.Tags
                .Where(x => x.TagType == tagType)
                .ToModel();

            return tags.ToList();
        }

        public async Task BanUser(Guid userId, CancellationToken cancellationToken)
        {
            var user = await db.Users
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
                throw new EntityNotFoundException(nameof(UserEntity), userId);

            var userRole = user.UserRoles.FirstOrDefault(x => x.RoleId == SeedData.UserRoleId);
            if (userRole == null)
            {
                throw new EntityNotFoundException(nameof(userRole), userId);
            }
            db.UserRoles.Remove(userRole);
            await db.SaveChangesAsync();
        }

        public async Task<List<OrderModel>> GetOrders(CancellationToken cancellationToken)
        {
            var orders = db.Orders
                .ToModel();

            return orders.ToList();
        }

        public async Task<OrderModel> GetOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await db.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            if (order == null)
                throw new EntityNotFoundException(nameof(OrderEnity), orderId);

            return order.ToModel();
        }

        public async Task<OrderModel> UpdateOrderAsync(UpdateOrder command, CancellationToken cancellationToken)
        {
            var order = await db.Orders
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (order == null)
                throw new EntityNotFoundException(nameof(OrderEnity), command.Id);

            order.PaymentMethod = command.PaymentMethod;
            order.ShippingAddress = command.Address;
            order.Status = command.Status;

            await db.SaveChangesAsync();

            return order.ToModel();
        }
    } 
}
