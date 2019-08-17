using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishingYear { get; set; }
        public string Path { get; set; }
        public User User { get; set; }
        public List<PublicationCategory> PublicationCategories { get; set; }
        public Publication()
        {
            PublicationCategories = new List<PublicationCategory>();
        }
    }
}
