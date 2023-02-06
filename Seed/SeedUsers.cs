using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.DAL.Seed
{
    public class SeedUsers
    {
        public static void Seed(IServiceProvider serviceProvider, int count)
        {
            var userContext = serviceProvider.GetRequiredService<UserManager<User>>();
            var rolesContext = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Trainer", "User" };
            var password = "trainer";

            foreach (var roleName in roleNames)
            {
                var roleExist = rolesContext.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    _ = rolesContext.CreateAsync(new IdentityRole(roleName)).Result;
                }
            }
            
            if (userContext.Users.Count(o => o.Attendee == null) < 2)
            {
                for (int i = 0; i < count; i++)
                {
                    var user = new Faker<User>().RuleFor(u => u.Email, f => f.Internet.Email(null, null, "yahoo.com"))
                                                .RuleFor(u => u.UserName, f => f.Internet.UserName());
                    var generatedUser = user.Generate();
                    var u = userContext.CreateAsync(generatedUser, password).Result;
                    var retrievedUser = userContext.FindByEmailAsync(generatedUser.Email).Result;

                    var x = userContext.AddToRoleAsync(retrievedUser, "Trainer").Result;
                }
            }
            
        }
    }
}
