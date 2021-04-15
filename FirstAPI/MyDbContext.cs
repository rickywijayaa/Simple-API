using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CategoryModel;
using ProductModel;
using Microsoft.EntityFrameworkCore;

namespace Database.MyDbContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
