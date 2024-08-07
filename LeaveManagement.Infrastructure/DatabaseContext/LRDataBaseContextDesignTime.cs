using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.DatabaseContext
{
   /* public class LRDbContextFactory : IDesignTimeDbContextFactory<LRDataBaseContext>
    {
        public LRDataBaseContext CreateDbContext(string[] args)
        {
           
            *//*var basePath = "C:/Users/Akram/OneDrive/Documents/React_NET_Projects/Leave Management/LeaveManagement.API";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();*//*

            var optionsBuilder = new DbContextOptionsBuilder<LRDataBaseContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LeaveManagementDB;Trusted_Connection=True;");

            return new LRDataBaseContext(optionsBuilder.Options);
        }
    }*/
}
