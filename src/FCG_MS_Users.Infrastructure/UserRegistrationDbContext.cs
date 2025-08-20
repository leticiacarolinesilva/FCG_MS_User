using FCG_MS_Users.Domain.Entities;
using FCG_MS_Users.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FCG_MS_Users.Infra;

public class UserRegistrationDbContext : DbContext
{
    public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options)
    {

    }

    /// <summary>
    /// Used for EF Core
    /// </summary>
    protected UserRegistrationDbContext()
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserAuthorization> UserAuthorizations => Set<UserAuthorization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set default schema for all entities
        modelBuilder.HasDefaultSchema("fcg_user");

        //modelBuilder.HasAnnotation("Relational:HistoryTableSchema", "fcg_user");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserRegistrationDbContext).Assembly);

        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<UserAuthorization>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        });

        var adminId = Guid.Parse("bacbbe47-017e-49a0-bd1a-5bbc2a2ffaca");

        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = adminId,
                Name = "Admin FIAP"
            }
        );

        modelBuilder.Entity<User>().OwnsOne(u => u.Email).HasData(
            new
            {
                UserId = adminId,
                Value = "admin@fiap.com"
            }
        );

        modelBuilder.Entity<User>().OwnsOne(u => u.Password).HasData(
            new
            {
                UserId = adminId,
                HasedValue = "100000.pbPQTpVLfm103U12g0gaTQ==.J3pVae2Sl9rKsuJDC7jci69KXk0/X21y0M0lYZmBo+E="
            }
        );

        modelBuilder.Entity<UserAuthorization>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    UserId = adminId,
                    Permission = AuthorizationPermissions.Admin
                }
            );
        });

        base.OnModelCreating(modelBuilder);
    }
}
