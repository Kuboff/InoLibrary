using InoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Interfaces
{
    public interface ICategories
    {
        IEnumerable<Category> GetAllCategories { get; }
        void AddCategory(Category category);
        Category GetCategory(int id);
        Category GetCategory(string name);
    }
}
