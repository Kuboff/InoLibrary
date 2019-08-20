using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InoLibrary.Models;

namespace InoLibrary.Controllers
{
    //Отвечает за страницу "Каталог публикаций"
    [Authorize]
    public class HomeController : Controller
    {
        InoLibraryDbContext _db;

        public HomeController(InoLibraryDbContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            //Список Id публикаций, которые нужно отобразить
            var publicationsIds = TempData["PublicationsIds"] as string[];

            List<Publication> publications = new List<Publication>();
            if (publicationsIds != null && publicationsIds.Length != 0)
            {
                for (int i = 0; i < publicationsIds.Length; i++)
                {
                    publications.Add(await _db.Publications.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == publicationsIds[i]));
                }
            }
            else
            {
                publications.AddRange(_db.Publications.Include(p => p.User).ToList());
            }

            //Создание списка категорий с актуальным количеством публикаций для каждой
            List<CategoryMenuElement> categoryMenu = new List<CategoryMenuElement>();
            List<PublicationCategory> publicationCategories = _db.PublicationCategories.ToList();
            List<Category> categories = _db.Categories.ToList();
            foreach (Category cat in categories)
            {
                bool menuFlag = true;
                foreach(PublicationCategory pc in publicationCategories)
                {
                    if (pc.Category.Name == cat.Name)
                    {
                        if (menuFlag)
                        {
                            categoryMenu.Add(new CategoryMenuElement { Name = cat.Name, Amount = 1 });
                            menuFlag = false;
                        }
                        else
                        {
                            categoryMenu.First(cm => cm.Name == cat.Name).Amount++;
                        }
                    }
                }
            }

            //Сортировка списка рубрик по алфавиту
            categoryMenu.Sort(CompareWords);

            ViewBag.CategoryMenu = categoryMenu;

            //Сортировка публикаций по времени создания
            publications.Sort(CompareByCreationTime);

            return View(publications);
        }

        public IActionResult Search(string searchOption, string searchString)
        {
            List<Publication> foundPublications = new List<Publication>();
            switch (searchOption)
            {
                case ("Автор"):
                    foundPublications = SearchByAuthor(searchString);
                    break;
                case ("Название"):
                    foundPublications = SearchByName(searchString);
                    break;
                case ("Год издания"):
                    Int32.TryParse(searchString, out int res);
                    foundPublications = SearchByYear(res);
                    break;
                case ("Category"):
                    foundPublications = SearchByCategory(searchString);
                    break;
            }

            string[] publicationsIds = new string[foundPublications.Count];

            for(int i = 0; i < foundPublications.Count; i++)
            {
                publicationsIds[i] = foundPublications[i].Id;
            }

            TempData["PublicationsIds"] = publicationsIds;
            return RedirectToAction("Index", "Home");
        }
        public List<Publication> SearchByName(string name)
        {
            List<Publication> publications = _db.Publications.Where(p => p.Name.Contains(name)).ToList();
            return publications;
        }
        public List<Publication> SearchByAuthor(string author)
        {
            List<Publication> publications = _db.Publications.Where(p => p.User.Nickname.Contains(author)).ToList();
            return publications;
        }
        public List<Publication> SearchByYear(int year)
        {
            List<Publication> publications = _db.Publications.Where(p => p.PublishingYear.ToString().Contains(year.ToString())).ToList();
            return publications;
        }
        public List<Publication> SearchByCategory(string categoryName)
        {
            List<PublicationCategory> publicationCategories = _db.PublicationCategories.Include(pc => pc.Category).Include(pc => pc.Publication).ToList();
            List<Publication> publications = new List<Publication>();
            foreach (PublicationCategory pc in publicationCategories)
            {
                if (pc.Category.Name == categoryName)
                {
                    publications.Add(pc.Publication);
                }
            }
            return publications;
        }

        public int CompareByCreationTime(Publication p1, Publication p2)
        {
            return p1.CreationTime.CompareTo(p2.CreationTime);
        }

        public int CompareWords(CategoryMenuElement c1, CategoryMenuElement c2)
        {
            return c1.Name.CompareTo(c2.Name);
        }

    }

    public class CategoryMenuElement
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }

}
