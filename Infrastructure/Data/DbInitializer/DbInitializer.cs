using Core.Entities;
using Infrastructure.Data.Static;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.DbInitializer
{
    public class DbInitializer
    {

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Context>();

                context.Database.EnsureCreated();

                // Customers
                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(new List<Customer>()
            {
                new Customer()
                {
                    FullName = "John Doe",
                    Mobile = "1234567890",
                    Address = "123 Main St"
                },
                new Customer()
                {
                    FullName = "Jane Smith",
                    Mobile = "9876543210",
                    Address = "456 Elm St"
                }
            });
                }

                // Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
            {
                new Product()
                {
                    Name = "Product 1",
                    Price = 10.99m,
                    CustomerID = 1
                },
                new Product()
                {
                    Name = "Product 2",
                    Price = 19.99m,
                    CustomerID = 2
                }
            });
                }

                context.SaveChanges();
            }
        }

        public static async Task SeedUserAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManger = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManger.RoleExistsAsync(Roles.Admin))
                    await roleManger.CreateAsync(new IdentityRole(Roles.Admin));
                if (!await roleManger.RoleExistsAsync(Roles.User))
                    await roleManger.CreateAsync(new IdentityRole(Roles.User));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@coreations.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        FullName = "Sherif",
                        UserName = "Sherif",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Sherif98@?");
                    await userManager.AddToRoleAsync(newAdminUser, Roles.Admin);
                }
            }
        }
    }
}