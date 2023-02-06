using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationForDidacticPurpose.DAL.Models;

namespace WebApplicationForDidacticPurpose.DAL.Seed
{
    public class SeedAttendees
    {
        public static void Seed(IServiceProvider serviceProvider, int count)
        {
            List<string> groups = Enum.GetNames(typeof(GroupType)).ToList();
            var context = serviceProvider.GetRequiredService<WebApplicationDbContext>();
            context.Database.EnsureCreated();

            if (context.Attendees.Count() < count)
            {
                for (int i = 0; i < count; ++i)
                {
                    string firstName = "Ion";
                    string lastName = "Ion";
                    string email = "";
                    string repozitoryLink = "";

                    var attendee = new Faker<AttendeeEntity>()
                        .RuleFor(a => a.FirstName, f => firstName = "Ion" + i)
                        .RuleFor(a => a.LastName, f => lastName = "Ion")
                        .RuleFor(a => a.Email, f => email = firstName + "_" + lastName + "@yahoo.com")
                        .RuleFor(a => a.Group, f => groups.ElementAt(i))
                        .RuleFor(a => a.RepozitoryLink, f => repozitoryLink = "");

                    context.Attendees.Add(attendee);
                    context.SaveChanges();
                }
            }
        }

    }
}
