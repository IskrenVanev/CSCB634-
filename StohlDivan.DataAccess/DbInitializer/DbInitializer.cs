﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StohlDivan.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using StohlDivan.Models;
using StohlDivan.Utility;

namespace StohlDivan.DataAccess.DbInitializer
{


    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;

        }
        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Moderator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();



                //if roles are not created, then we will create admin user as well

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@iskren.com",
                    Email = "admin@iskren.com",
                    Name = "Iskren Vanev",
                    PhoneNumber = "123123123",
                    StreetAddress = "test 123 iskrr",
                    State = "MA",
                    PostalCode = "23123",
                    City = "Boston"


                }, "Qqq123*").GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "moderator@ivo.com",
                    Email = "moderator@ivo.com",
                    Name = "Ivo Han",
                    PhoneNumber = "123123123",
                    StreetAddress = "test 123 iskrr",
                    State = "MA",
                    PostalCode = "23123",
                    City = "Boston"


                }, "Qqq123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.applicationUsers.FirstOrDefault(u => u.Email == "admin@iskren.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                ApplicationUser user1 = _db.applicationUsers.FirstOrDefault(u => u.Email == "moderator@ivo.com");
                _userManager.AddToRoleAsync(user1, SD.Role_Moderator).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
