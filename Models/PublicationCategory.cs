using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Models
{
    public class PublicationCategory
    {
        public string PublicationId { get; set; }
        public Publication Publication { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
