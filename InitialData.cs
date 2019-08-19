using InoLibrary.Models;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary
{
    public static class InitialData
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
                    PasswordHash = hasher.HashPassword(null, "123456Ab-"),
                    Nickname = "Kuboff",
                    UserName = "kubasovra@gmail.com",
                    NormalizedEmail = "KUBASOVRA@GMAIL.COM",
                    NormalizedUserName = "KUBASOVRA@GMAIL.COM"
                };
                if (hasher.VerifyHashedPassword(newUser, newUser.PasswordHash, "123456Ab-") != PasswordVerificationResult.Success)
                {
                    throw new Exception("Initial user's password corrupted");
                }
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


                Publication p1 = new Publication
                {
                    Name = "Publication #1",
                    PublishingYear = DateTime.Now.Year,
                    Path = @"C:\Users\Roman\source\repos\InoLibrary\InoLibrary\Data\#1.docx",
                    User = newUser,
                    Annotation = "Первая публикация"
                };
                Publication p2 = new Publication
                {
                    Name = "Publication #2",
                    PublishingYear = DateTime.Now.Year,
                    Path = @"C:\Users\Roman\source\repos\InoLibrary\InoLibrary\Data\#2.docx",
                    User = newUser,
                    Annotation = "Вторая публикация"
                };
                Publication p3 = new Publication
                {
                    Name = "Publication #3",
                    PublishingYear = DateTime.Now.Year,
                    Path = @"C:\Users\Roman\source\repos\InoLibrary\InoLibrary\Data\#3.docx",
                    User = newUser,
                    Annotation = "Третья публикация"
                };
                context.Categories.AddRange(categories);
                context.Publications.AddRange(p1,p2,p3);
                context.Users.Add(newUser);
                context.PublicationCategories.AddRange(
                    new PublicationCategory
                    {
                        PublicationId = p1.Id,
                        CategoryId = categories[0].Id,
                        Category = categories[0]
                    },
                    new PublicationCategory
                    {
                        PublicationId = p1.Id,
                        CategoryId = categories[1].Id,
                        Category = categories[1]
                    },
                    new PublicationCategory
                    {
                        PublicationId = p2.Id,
                        CategoryId = categories[2].Id,
                        Category = categories[2]
                    },
                    new PublicationCategory
                    {
                        PublicationId = p3.Id,
                        CategoryId = categories[3].Id,
                        Category = categories[3]
                    });
                context.SaveChanges();
            }
            else if (!context.Categories.Any())
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
