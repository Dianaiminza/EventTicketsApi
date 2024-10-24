using EventsTicket.Domain.Entities;
using Infrastructure.shared.Contracts;
using Infrastructure.shared.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace EventsTicket.Infrastructure.Context
{
    public class DatabaseContext : DbContext
    {
        private readonly ICurrentDateProvider _currentDateProvider;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, ICurrentDateProvider currentDateProvider) :
          base(options)
        {
            _currentDateProvider = currentDateProvider;
            Options = options;
        }

        public DbContextOptions<DatabaseContext> Options { get; }
        public DbSet<EventTicket> EventTickets { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventTicket>(entity =>
            {
                entity.HasKey(e => e.Id).IsClustered(true);
               
            });

            AddEnumConstraints(modelBuilder);
            SetDecimalDefaultPrecision(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetEntitiesAuditInfo();
            TryUpdateEntitiesVersion();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
          CancellationToken cancellationToken = new())
        {
            SetEntitiesAuditInfo();
            TryUpdateEntitiesVersion();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SetEntitiesAuditInfo()
        {
            SetEntitiesCreatedOnSave();
            SetEntitiesUpdatedOnSave();
        }

        private void SetEntitiesCreatedOnSave()
        {
            var entitiesToCreate = FilterTrackedEntriesByState(EntityState.Added);

            foreach (var entity in entitiesToCreate)
            {
                entity.CreatedOn = _currentDateProvider.NowUtc;
            }
        }

        private void SetEntitiesUpdatedOnSave()
        {
            var entitiesToUpdate = FilterTrackedEntriesByState(EntityState.Modified);

            foreach (var entity in entitiesToUpdate)
            {
                entity.LastModifiedOn = _currentDateProvider.NowUtc;
            }
        }

        private void TryUpdateEntitiesVersion()
        {
            var entries = ChangeTracker
              .Entries()
              .Where(entry => entry.Entity is IBaseEntity && entry.State is EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseVersionedEntity versionedEntity)
                {
                    var shouldIncreaseVersion = entry.Properties.Any(prop =>
                      prop.IsModified &&
                      versionedEntity.VersionedFields.Contains(prop.Metadata.Name));

                    if (shouldIncreaseVersion)
                    {
                        versionedEntity.Version++;
                    }
                }
            }
        }

        private IEnumerable<IBaseEntity> FilterTrackedEntriesByState(EntityState entityState)
        {
            return ChangeTracker
              .Entries()
              .Where(e => e.Entity is IBaseEntity && e.State == entityState)
              .Select(e => (IBaseEntity)e.Entity);
        }

        private static void AddEnumConstraints(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties();

                foreach (var property in properties)
                {
                    var nullableSubType = Nullable.GetUnderlyingType(property.ClrType);
                    var propertyType = nullableSubType ?? property.ClrType;

                    if (propertyType.IsEnum)
                    {
                        var enumValues = Enum.GetValues(propertyType).Cast<int>().ToList();
                        var enumValuesString = string.Join(", ", enumValues);
                        var tableName = entityType.GetTableName();

                        modelBuilder
                          .Entity(entityType.ClrType)
                          .HasCheckConstraint(
                            $"CK_{entityType.GetTableName()}_{property.GetColumnName(StoreObjectIdentifier.Table(tableName))}",
                            $"\"{property.GetColumnName(StoreObjectIdentifier.Table(tableName))}\" IN ({enumValuesString})");
                    }
                }
            }
        }

        private static void SetDecimalDefaultPrecision(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                       .SelectMany(t => t.GetProperties())
                       .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(13,2)");
            }
        }
    }
}
