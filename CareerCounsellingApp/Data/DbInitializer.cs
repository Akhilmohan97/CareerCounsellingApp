using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Data;

    public static class DbInitializer
    {
    public static void Seed(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Username = "admin",
                Password = "admin123",
                Role = "Admin"
            });

            db.SaveChanges();
        }
    }
}

