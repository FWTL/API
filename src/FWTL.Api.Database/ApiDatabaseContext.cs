using FWTL.Api.Database.Configuration;
using FWTL.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FWTL.Api.Database
{
    public class ApiDatabaseContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly ApiDatabaseCredentials _credentials;

        public ApiDatabaseContext(ApiDatabaseCredentials credentials)
        {
            _credentials = credentials;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_credentials.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            base.OnModelCreating(builder);
        }
    }
}