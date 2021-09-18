using Microsoft.EntityFrameworkCore;
using MURDoX.Model;
using MURDoX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<DiscordUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dataService = new DataService();
            var conString = dataService.GetApplicationConfig();

            options.UseNpgsql(conString.ConnectionString);
        }
         //options.UseNpgsql("Username=postgres;Password=451145_Gl;Host=localhost;Database=MURDoXDb");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
