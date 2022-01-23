using Treinando.Models;
using Treinando.Controllers;
//using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace Treinando.Data
{
    //Classe de Configuração do Banco de Dados
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options){ }

        //Aqui foi comentado porque foi utilizado outra forma para configurar o MySql
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite(new SqliteConnection("Filename=DBContext.db"));
            optionsBuilder.UseMySql(new MySqlConnection("server=localhost;database=DBContext;uid=root;pwd=MS1778amA"));
        }
        */

        public DbSet<Address> Adresses {get; set;}
        public DbSet<Product> Items {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<User> Users {get; set;}

        //public DbSet<NewItem> NewItem {get; set;}
        //public DbSet<OrderReport> OrderReports {get; set;}
        //public DbSet<OrderSearch> OrderSearch {get; set;}
    }
}