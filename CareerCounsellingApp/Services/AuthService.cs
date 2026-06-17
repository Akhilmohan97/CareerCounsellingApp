using CareerCounsellingApp.Data;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services;

public class AuthService
{
    public User? Login(string username, string password)
    {
        using var db = new AppDbContext();

        return db.Users.FirstOrDefault(x =>
            x.Username == username &&
            x.Password == password);
    }
}
