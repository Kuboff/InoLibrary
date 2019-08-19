using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PublicationCategory> PublicationCategories { get; set; }
        public Category()
        {
            PublicationCategories = new List<PublicationCategory>();
        }
    }
}
