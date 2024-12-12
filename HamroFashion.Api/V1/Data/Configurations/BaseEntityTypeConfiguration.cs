using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HamroFashion.Api.V1.Data.Configurations
{
    /// <summary>
    /// Helper extensions related to our base entity
    /// </summary>
    public static class BaseEntityTypeConfiguration
    {
        /// <summary>
        /// Allows enities that inherit from BaseEntity to configure their BaseEntity fields...
        /// </summary>
        /// <typeparam name="T">The type of entity model we are configuring (must derrive from BaseEntity)</typeparam>
        /// <param name="builder">The entity type builder to configure</param>
        public static void ConfigureBaseModel<T>(this EntityTypeBuilder<T> builder)
            where T : BaseEntity
        {
            builder.HasKey(x => x.Id);
        }
    }
}
