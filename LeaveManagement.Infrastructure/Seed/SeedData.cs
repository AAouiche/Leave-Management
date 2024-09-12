using LeaveManagement.Domain.LeaveRequests;
using LeaveManagement.Domain.LeaveTypes;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Authentication;

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

            if (!context.LeaveTypes.Any())
            {
                var leaveTypes = new List<LeaveType>
                {
                    new LeaveType { Name = "Vacation", Days = 10 },
                    new LeaveType { Name = "Sick", Days = 15 }
                };

                context.LeaveTypes.AddRange(leaveTypes);
                await context.SaveChangesAsync();
            }

            try
            {
                if (!context.LeaveRequests.Any())
                {
                    var vacationType = context.LeaveTypes.SingleOrDefault(lt => lt.Name == "Vacation");
                    var sickType = context.LeaveTypes.SingleOrDefault(lt => lt.Name == "Sick");

                    var leaveRequests = new List<LeaveRequest>
                    {
                        new LeaveRequest
                        {
                            StartDate = new DateTime(2024, 9, 15),
                            EndDate = new DateTime(2024, 9, 20),
                            LeaveTypeId = vacationType.Id,
                            DateRequested = DateTime.Now,
                            RequestComments = "Going on vacation.",
                            Approved = true,
                            Cancelled = false,
                            Employee = employee 
                        },
                        new LeaveRequest
                        {
                            StartDate = new DateTime(2024, 10, 10),
                            EndDate = new DateTime(2024, 10, 15),
                            LeaveTypeId = sickType.Id,
                            DateRequested = DateTime.Now,
                            RequestComments = "Family event.",
                            Approved = null,
                            Cancelled = false,
                            Employee = employee 
                        },
                        new LeaveRequest
                        {
                            StartDate = new DateTime(2024, 11, 5),
                            EndDate = new DateTime(2024, 11, 10),
                            LeaveTypeId = vacationType.Id,
                            DateRequested = DateTime.Now,
                            RequestComments = "Medical leave.",
                            Approved = false,
                            Cancelled = false,
                            Employee = employee 
                        }
                    };

                    context.LeaveRequests.AddRange(leaveRequests);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Leave requests seeded successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding leave requests: " + ex.Message);
            }
        }
    }
}