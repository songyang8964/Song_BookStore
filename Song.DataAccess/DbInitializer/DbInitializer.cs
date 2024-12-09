using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Song.DataAccess.Data;
using Song.Models;
using Song.Utility;

namespace Song.DataAccess.DbInitializer {
    public class DbInitializer : IDbInitializer {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db) {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }


        public void Initialize() {


            //migrations if they are not applied
            try {
                if (_db.Database.GetPendingMigrations().Count() > 0) {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex) { }


            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult()) {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                var category = new Category { Name = "Action", DisplayOrder = 1 };
                _db.Categories.Add(category);
                _db.SaveChanges();

                //if roles are not created, then we will create admin user as well
                var user = new ApplicationUser {
                    UserName = "tianhuilin45@gmail.com",
                    Email = "tianhuilin45@gmail.com",
                    Name = "Song Yang",
                    PhoneNumber = "1112223333",
                    StreetAddress = "university of limerick",
                    State = "Limerick",
                    PostalCode = "1234",
                    City = "Limerick"
                };

                _userManager.CreateAsync(user, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                var product = new Product {
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero...",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = category.Id
                };
                _db.Products.Add(product);
                _db.SaveChanges();
            }

            return;
        }
    }
}
