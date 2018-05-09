using AnimeCollections.Authorization;
using AnimeCollections.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCollections.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string UserPWD, string AdminPWD)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                var adminID = await EnsureUser(serviceProvider, AdminPWD, "compe561@admin.com");
                await EnsureRole(serviceProvider, adminID, Constants.ContactAdministratorsRole);

                var uid = await EnsureUser(serviceProvider, UserPWD, "compe561@manager.com");
                await EnsureRole(serviceProvider, uid, Constants.ContactManagersRole);


                SeedDB(context, adminID);
            }
        }



        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                             string UserPWD, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName };
                await userManager.CreateAsync(user, UserPWD);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                  string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }


        public static void SeedDB(ApplicationDbContext context, string adminID)
        {


            // Look for any movies.
            if (context.Anime.Any())
            {
                return;   // DB has been seeded
            }


            #region SampleData
            context.Anime.AddRange(
                new Anime
                {
                    Title = "Tokyo Ghoul 3 (Sub)",
                    ReleaseDate = DateTime.Parse("2018-4-3"),
                    Genre = "Action Mystery",
                    Price = 0,
                    Rating = "Excellent",
                    Status = Status.Allowed,
                    OwnerID = adminID
                },

     
                        new Anime
                        {
                            Title = "Food Wars! The Third Plate 2nd courr",
                            ReleaseDate = DateTime.Parse("2018-4-9"),
                            Genre = "School",
                            Price = 100,
                            Rating = "Good",
                            Status = Status.Allowed,
                            OwnerID = adminID
                        },

                    new Anime
                    {
                        Title = "High School DxD 4th Season (Sub)",
                        ReleaseDate = DateTime.Parse("2018-4-10"),
                        Genre = "Comedy",
                        Price = 102,
                        Rating = "Hot",
                        OwnerID = adminID
                    },


                    new Anime
                    {
                        Title = "Wotaku ni Koi wa Muzukashii",
                        ReleaseDate = DateTime.Parse("2018-4-13"),
                        Genre = "Comedy Romance",
                        Price = 103,
                        Rating = "Good",
                        OwnerID = adminID
                    },


                    new Anime
                    {
                        Title = "Persona 5 the Animation",
                        ReleaseDate = DateTime.Parse("2018-4-8"),
                        Genre = "Action Fantasy Supernatural",
                        Price = 104,
                        Rating = "Great",
                        OwnerID = adminID
                    },

                    new Anime
                    {
                        Title = "Sword Art Online Alternative: Gun Gale Online",
                        ReleaseDate = DateTime.Parse("2018-4-8"),
                        Genre = "Fantasy Game Sci-Fi",
                        Price = 110,
                        Rating = "Good",
                        OwnerID = adminID
                    }
                );
            context.SaveChanges();
            #endregion
        }

    }
}


    

