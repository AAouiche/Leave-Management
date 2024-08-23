using LeaveManagement.Domain.Identity;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, LRDataBaseContext context)
        {
            string[] roleNames = { "Admin", "Employee" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var admin = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                FirstName = "Admin",
                LastName = "User"
            };

            string adminPassword = "Admin123!";
            var user = await userManager.FindByEmailAsync(admin.Email);

            if (user == null)
            {
                var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            var employee = new ApplicationUser
            {
                UserName = "employee@company.com",
                Email = "employee@company.com",
                FirstName = "John",
                LastName = "Doe"
            };

            string employeePassword = "Employee123!";
            user = await userManager.FindByEmailAsync(employee.Email);

            if (user == null)
            {
                var createEmployee = await userManager.CreateAsync(employee, employeePassword);
                if (createEmployee.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee, "Employee");
                }
            }
        }
    }
}
