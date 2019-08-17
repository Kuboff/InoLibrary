using InoLibrary.Interfaces;
using InoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InoLibrary.Repos
{
    public class CategoryRepo : ICategories
    {
        private readonly InoLibraryDbContext lbDbContext;
        public CategoryRepo(InoLibraryDbContext lbDbContext)
        {
            this.lbDbContext = lbDbContext;
        }
        public IEnumerable<Category> GetAllCategories => lbDbContext.Categories.Include(c => c.PublicationCategories).ThenInclude(pc => pc.Publication);

        public void AddCategory(Category category)
        {
            lbDbContext.Categories.Add(category);
            lbDbContext.SaveChanges();
        }

        public Category GetCategory(int id)
        {
            return lbDbContext.Categories.Include(c => c.PublicationCategories).ThenInclude(pc => pc.Publication).FirstOrDefault(c => c.Id == id);
        }

        public Category GetCategory(string name)
        {
            return lbDbContext.Categories.Include(c => c.PublicationCategories).ThenInclude(pc => pc.Publication).FirstOrDefault(c => c.Name == name);
        }
    }
}
