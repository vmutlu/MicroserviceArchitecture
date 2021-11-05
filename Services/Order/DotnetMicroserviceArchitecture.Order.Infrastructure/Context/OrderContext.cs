using Microsoft.EntityFrameworkCore;

namespace DotnetMicroserviceArchitecture.Order.Infrastructure.Context
{
    public class OrderContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ORDER";

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.Order> Orders { get; set; }
        public DbSet<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

            modelBuilder.Entity<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.OrderItem>().Property(X => X.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.Order>().OwnsOne(x => x.Adress).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}
