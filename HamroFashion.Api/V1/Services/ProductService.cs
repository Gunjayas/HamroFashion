using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Data.Entities;
using HamroFashion.Api.V1.Exceptions;
using HamroFashion.Api.V1.Extensions;
using HamroFashion.Api.V1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HamroFashion.Api.V1.Services
{
    /// <summary>
    /// Our Product persistence service. Use this to read/write Products to the database
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// The HamroFashionContext we persist to
        /// </summary>
        public readonly HamroFashionContext db;

        //Imageservice

        public ProductService(HamroFashionContext db)
        {
            this.db = db;
        }

        public async Task<ProductModel> CreateAsync(CreateProduct command, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken = default)
        {
            //validate user first

            var product = new ProductEntity
            {
                Name = command.Name,
                Description = command.Description,
                Color = command.Color,
                ImageUrls = command?.ImageUrls,
                Size = command?.Size,
                Quantity = command.Quantity,
                Availability = command.Availability
            };

            if (command.ProductCollection != null)
            {
                product.ProductCollection = command.ProductCollection
                    .Select(x => new ProductCollection { ProductId = product.Id, TagId = x })
                    .ToList();
            }

            if (command.ProductCategory.HasValue != null)
            {
                product.ProductCategory = new ProductCategory { ProductId = product.Id, TagId = command.ProductCategory.Value };
            }
            if (command.ProductCollection != null)
            {
                product.ProductCollection = command.ProductCollection
                    .Select(x => new ProductCollection { ProductId = product.Id, TagId = x })
                    .ToList();
            }

            db.Products.Add(product);
            await db.SaveChangesAsync(cancellationToken);

            return product.ToModel();
        }

        public async Task<ProductModel> UpdateAsync(UpdateProduct command, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken = default)
        {
            var product = db.Products
                .Where(x => x.Id  == command.Id)
                .FirstOrDefault();

            if (product == null)
                throw new EntityNotFoundException(nameof(ProductEntity), command.Id);

            product.Description = command.Description;
            product.Name = command.Name;
            product.Color = command.Color;
            product.Availability = command.Availability;
            product.Quantity = command.Quantity;
            product.Size = command.Size;

            if (command.ProductCollection != null)
            {
                var currentCollection = await db.ProductCollections.Where(x => x.ProductId == product.Id).ToListAsync(cancellationToken);

                var toRemove = currentCollection.Where(x => !command.ProductCollection.Contains(x.TagId));

                db.ProductCollections.RemoveRange(toRemove);

                var toAdd1 = command.ProductCollection.Where(x => !currentCollection.Any(c => c.TagId == x));

                var Add2 = toAdd1
                    .Select(x => new ProductCollection { ProductId = product.Id, TagId = x})
                    .ToList();

                db.ProductCollections.AddRange(Add2);
            }
            await db.SaveChangesAsync(cancellationToken);

            return product.ToModel();

        }
        public async Task DeleteAsync(Guid productId, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var product = await db.Products
                .FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
                throw new EntityNotFoundException(nameof(ProductEntity), productId);

            db.Products.Remove(product);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<ProductModel> GetByIdAsync(Guid id, ClaimsPrincipal CurrentUser, CancellationToken cancellationToken)
        {
            var product = await db.Products
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (product == null)
                throw new EntityNotFoundException(nameof(ProductEntity), id);

            return product.ToModel();
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts(CancellationToken cancellationToken)
        {
            var products = await db.Products
                .ToListAsync(cancellationToken);

            return products.ToModel();
        }
        
        public async Task<PagedResponse<ProductModel>?> ListAsync(PagedQuery<ListProducts> query, CancellationToken cancellationToken)
        {
            var products = await db.Products
                .ToPage(query)
                .WhereIf(x => x.Name.Contains(query.FilterCommand.Search), !string.IsNullOrWhiteSpace(query.FilterCommand.Search))
                .WhereIf(x => x.Color == query.FilterCommand.Color, !string.IsNullOrWhiteSpace(query.FilterCommand.Color))
                .WhereIf(x => x.ProductCollection.Any(c => c.Tag.Name == query.FilterCommand.Collection), !string.IsNullOrWhiteSpace(query.FilterCommand.Collection))
                .WhereIf(x => x.ProductCategory.Tag.Name == query.FilterCommand.Category, !string.IsNullOrWhiteSpace(query.FilterCommand.Category))
                .ToListAsync(cancellationToken);

            var models = products
                .ToModel()
                .ToList();

            return PagedResponse<ProductModel>
                .FromQuery(query, models); 
        }
    }
}
