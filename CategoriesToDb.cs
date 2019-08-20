using InoLibrary.Models;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary
{
    public static class CategoriesToDb
    {
        public static void Initialize(InoLibraryDbContext context)
        {
            if (!context.Categories.Any())
            {
                List<Category> categories = new List<Category>{
                    new Category{
                        Name = "Физика"
                    },
                    new Category{
                        Name = "Химия"
                    },
                    new Category{
                        Name = "Биология"
                    },
                    new Category{
                        Name = "Астрономия"
                    },
                    new Category{
                        Name = "Геология"
                    },
                    new Category{
                        Name = "География"
                    },
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}
