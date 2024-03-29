﻿using Microsoft.EntityFrameworkCore;
using ProjectOMatic.Models;

namespace ProjectOMatic.Data
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<Framework> Frameworks { get; set; } = null!;
        public DbSet<SkillLevel> SkillLevels { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Solution> Solutions { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<Program>()
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? configuration.GetConnectionString("Database:ConnectionString");

                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 17)));
            }
        }
    }
}
