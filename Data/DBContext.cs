using Treinando.Models;
using Treinando.Controllers;
//using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace Treinando.Data
{
    
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options){ }
              

        public DbSet<Address> Adresses {get; set;}
        public DbSet<Product> Items {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<User> Users {get; set;}
    }
}