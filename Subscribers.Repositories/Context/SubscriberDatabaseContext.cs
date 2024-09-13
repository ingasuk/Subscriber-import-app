using Microsoft.EntityFrameworkCore;
using Subscribers.Repositories.Entities;

namespace Subscribers.Repositories.Context;
public class SubscriberDatabaseContext(DbContextOptions<SubscriberDatabaseContext> options) : DbContext(options)
{
    public DbSet<Subscriber> Subscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscriber>().HasKey(x => x.Id);
        modelBuilder.Entity<Subscriber>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Subscriber>().HasIndex(x => x.ExpirationDate);
        modelBuilder.Entity<Subscriber>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Subscriber>().Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<Subscriber>().Property(x => x.UpdatedDate).HasDefaultValueSql("GETUTCDATE()").IsConcurrencyToken();
    }
}
