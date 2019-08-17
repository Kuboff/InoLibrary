using InoLibrary.Interfaces;
using InoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InoLibrary.Repos
{
    public class PublicationRepo : IPublications
    {
        private readonly InoLibraryDbContext lbDbContext;
        public PublicationRepo(InoLibraryDbContext lbDbContext)
        {
            this.lbDbContext = lbDbContext;
        }
        public IEnumerable<Publication> GetAllPublications => lbDbContext.Publications.Include(p => p.PublicationCategories).ThenInclude(pc => pc.Category);

        public void AddPublication(Publication publication)
        {
            lbDbContext.Publications.Add(publication);
            lbDbContext.SaveChanges();
        }

        public Publication GetPublication(int id)
        {
            return lbDbContext.Publications.Include(p => p.User).Include(p => p.PublicationCategories).ThenInclude(pc => pc.Category).FirstOrDefault(p => p.Id == id);
        }
    }
}
