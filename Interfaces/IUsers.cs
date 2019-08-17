using InoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Interfaces
{
    public interface IUsers
    {
        IEnumerable<User> GetAllUsers { get; }
        void AddUser(User user);
        User GetUser(string id);
        User GetUser(string email, string password);
    }
}
