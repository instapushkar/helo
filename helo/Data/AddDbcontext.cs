using helo.Models.Entities;
using helo.Models.CustomLogin;
using Microsoft.EntityFrameworkCore;
using helo.Models.Product;
using helo.Models.Sales;

namespace helo.Data
{
    public class AddDbcontext: DbContext
    {
        public AddDbcontext(DbContextOptions<AddDbcontext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<ProductData> ProductDatas { get; set; }
        public DbSet<SalesProduct> SalesProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesProduct>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
