using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ChemistShop.Models
{
    public class MedicamentsContext: DbContext
    {
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Reception> Receptions { get; set; }
        public DbSet<Consumption> Consumptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("SqliteConnection");
            var options = optionsBuilder
                .UseSqlite(connectionString)
                .Options;
        }
    }
}
