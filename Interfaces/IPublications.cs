using InoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Interfaces
{
    public interface IPublications
    {
        IEnumerable<Publication> GetAllPublications { get; }
        void AddPublication(Publication publication);
        Publication GetPublication(int id);
    }
}
