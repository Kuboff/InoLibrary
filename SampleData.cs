using InoLibrary.Models;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary
{
    public static class SampleData
    {
        public static void Initialize(InoLibraryDbContext context)
        {
            if (!context.Users.Any() && !context.Publications.Any() && !context.Categories.Any() && !context.PublicationCategories.Any())
            {
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                User newUser = new User
                {
                    FullName = "Кубасов Роман Александрович",
                    Email = "kubasovra@gmail.com",
                    PasswordHash = hasher.HashPassword(null, "123456")
                };
                if (hasher.VerifyHashedPassword(newUser, newUser.PasswordHash, "123456") != PasswordVerificationResult.Success)
                {
                    throw new Exception("Initial user's password corrupted");
                }
                Category c1 = new Category
                {
                    Name = "Научпоп",
                };
                Category c2 = new Category
                {
                    Name = "Фанфик",
                };

                Publication p1 = new Publication
                {
                    Name = "Publication #1",
                    PublishingYear = DateTime.Now.Date,
                    Path = @"C:\Users\Roman\source\repos\InoLibrary\InoLibrary\Data\#1.docx",
                    User = newUser
                };
                Publication p2 = new Publication
                {
                    Name = "Publication #2",
                    PublishingYear = DateTime.Now.Date,
                    Path = @"C:\Users\Roman\source\repos\InoLibrary\InoLibrary\Data\#2.docx",
                    User = newUser

                };
                Publication p3 = new Publication
                {
                    Name = "Publication #3",
                    PublishingYear = DateTime.Now.Date,
                    Path = @"C:\Users\Roman\source\repos\InoLibrary\InoLibrary\Data\#3.docx",
                    User = newUser
                };
                context.Categories.AddRange(c1,c2);
                context.Publications.AddRange(p1,p2,p3);
                context.Users.Add(newUser);
                context.PublicationCategories.AddRange(
                    new PublicationCategory
                    {
                        PublicationId = p1.Id,
                        CategoryId = c1.Id
                    },
                    new PublicationCategory
                    {
                        PublicationId = p1.Id,
                        CategoryId = c2.Id
                    },
                    new PublicationCategory
                    {
                        PublicationId = p2.Id,
                        CategoryId = c1.Id
                    },
                    new PublicationCategory
                    {
                        PublicationId = p3.Id,
                        CategoryId = c2.Id
                    });
                context.SaveChanges();
            }
        }
    }
}
