using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails{ get; set; }


        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TagCategory> TagSubCategories { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<Review> Reviews { get; set; }

    }
}
