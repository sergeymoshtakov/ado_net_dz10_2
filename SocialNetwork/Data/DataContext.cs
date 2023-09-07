using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entity.Gender> Genders { get; set; }
        public DbSet<Entity.User> Users { get; set; }

        public DataContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=SocialNetwork;Integrated Security=True"
                );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity.User>() //
                .HasOne(u => u.Gender) // name of property
                .WithMany() // type of connection one to many
                .HasForeignKey(u => u.IdGender) // foreign key
                .HasPrincipalKey(g => g.Id); //main key
        }
    }
}