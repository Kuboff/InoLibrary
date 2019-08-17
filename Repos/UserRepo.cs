using InoLibrary.Interfaces;
using InoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InoLibrary.Repos
{
    public class UserRepo : IUsers
    {
        private readonly InoLibraryDbContext lbDbContext;
        public UserRepo(InoLibraryDbContext lbDbContext)
        {
            this.lbDbContext = lbDbContext;
        }
        public IEnumerable<User> GetAllUsers => lbDbContext.Users.Include(u => u.Publications);

        public void AddUser(User user)
        {
            lbDbContext.Users.Add(user);
            lbDbContext.SaveChanges();
        }

        public User GetUser(string id)
        {
            return lbDbContext.Users.Include(u => u.Publications).FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string email, string password)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            return lbDbContext.Users.Include(u => u.Publications).FirstOrDefault(u => u.Email == email && u.PasswordHash == hasher.HashPassword(null, password));
        }
    }
}
