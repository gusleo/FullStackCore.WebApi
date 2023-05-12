using Core.Data.Entities;
using Core.Data.Extensions;
using Core.Data.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using Scaffolding.Data;
using Scaffolding.Data.Entities.Abstract;
using System.Linq.Expressions;

namespace Core.Data
{
    public class CoreContext : DataContext, ICoreContext
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #region Constructor
        public CoreContext(DbContextOptions<CoreContext> options, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor) : base(ChangeOptionsType<DataContext>(options))
        {
            this.loggerFactory = loggerFactory;
            this._httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Static Method
        public static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {

            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);
            if (sqlExt == null)
            {
                throw (new Exception("Failed to retrieve SQL connection string for base Context"));
            }
            else
            {
                var connection = ((SqlServerOptionsExtension)sqlExt).ConnectionString ?? "";
                return new DbContextOptionsBuilder<T>()
                       .UseSqlServer(connection,
                        b => b.MigrationsAssembly("FullStackCore.WebApi"))
                        .Options;
            }


        }
        #endregion

        #region public DbSet
        public DbSet<UserDetail> UserDetails { get;} = default!;
        public DbSet<LogEntry> LogEntries { get; } = default!;
        #endregion

        #region Override
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseOpenIddict();


            // Prevent cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            // add the filter for "isDeleted" to all entity
            // do not filter manually
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // for entity derived from IEntityBase
                if (typeof(IEntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    // build lamda expression for (e => !e.isDeleted)
                    // example modelBuilder.Entity<Article>().HasQueryFilter(e => !e.IsDeleted)
                    var parameter = Expression.Parameter(entityType.ClrType, "x");
                    var property = Expression.Property(parameter, nameof(IEntityBase.IsDeleted));
                    var constant = Expression.Constant(false);
                    var lambda = Expression.Lambda(Expression.Equal(property, constant), parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }


            //Define manual which table will cascade delete
            //modelBuilder.Entity<ArticleComment>().HasOne(x => x.Article)
            //    .WithMany(x => x.Comments)
            //    .OnDelete(DeleteBehavior.Cascade);

            //Seed Data
            modelBuilder.Seed();
        }

        /// <summary>
        /// Override exisiting entity data
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                CheckBeforeSaving();
                return base.SaveChanges();
            }
            catch
            {
                return base.SaveChanges();
            }

        }

        #endregion

        private void CheckBeforeSaving()
        {

            // Get logged userid
            var sub = _httpContextAccessor?.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("sub"));
            var id = sub == null ? String.Empty : sub.Value;
            Guid.TryParse(id, out Guid userGuid);

            // override entity data
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        ((IEntityBase)entry.Entity).IsDeleted = true;
                        ((IEntityBase)entry.Entity).DeletedById = userGuid;
                        ((IEntityBase)entry.Entity).DeletedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        ((IEntityBase)entry.Entity).UpdatedById = userGuid;
                        ((IEntityBase)entry.Entity).UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        ((IEntityBase)entry.Entity).IsDeleted = false;
                        ((IEntityBase)entry.Entity).CreatedById = userGuid;
                        ((IEntityBase)entry.Entity).CreatedDate = DateTime.Now;
                        ((IEntityBase)entry.Entity).UpdatedById = userGuid;
                        ((IEntityBase)entry.Entity).UpdatedDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}