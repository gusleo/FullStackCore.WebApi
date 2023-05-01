using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Scaffolding.Auth.Entities;
using Scaffolding.Data.Entities;
using Scaffolding.Data.Entities.Abstract;

namespace Scaffolding.Data
{
    /// <summary>
    /// Data context class
    /// </summary>
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDataContext
    {

       /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options){}




        /// <summary>
        /// By default table behaviour is cascade delete, but we need some table not set as auto delete
        /// Set delete behaviour from cascade delete to cascade restrict
        /// Define manualy which table will cascade delete
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //Remove Cascade Delate Bahaviour
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


        }

        /// <summary>
        /// Update data to database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Updated<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            EntityEntry dbEntityEntry = this.Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                this.Set<TEntity>().Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }
    }
}
