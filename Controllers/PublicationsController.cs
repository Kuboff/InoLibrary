using InoLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InoLibrary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace InoLibrary.Controllers
{
    //Отвечает за все действия с карточками публикаций
    [Authorize]
    public class PublicationsController : Controller
    {
        InoLibraryDbContext _db;
        UserManager<User> _userManager;
        IHostingEnvironment _appEnvironment;
        List<int> _availablePublishingYears;
        public PublicationsController(InoLibraryDbContext context, UserManager<User> userManager, IHostingEnvironment appEnvironment)
        {
            _userManager = userManager;
            _db = context;
            _appEnvironment = appEnvironment;
            _availablePublishingYears = new List<int>();
            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                _availablePublishingYears.Add(i);
            }
        }

        //Просмотр
        [HttpGet]
        public async Task<IActionResult> ObservePublication(string id)
        {
            if (id != null)
            {
                Publication publication = await _db.Publications.Include(p => p.User).Include(p => p.PublicationCategories).ThenInclude(pc => pc.Category).FirstOrDefaultAsync(p => p.Id == id);
                ObservePublicationViewModel viewModel = new ObservePublicationViewModel{
                    Id = publication.Id,
                    Name = publication.Name,
                    Annotation = publication.Annotation,
                    AuthorNickname = publication.User.Nickname,
                    PublishingYear = publication.PublishingYear,
                    DownloadURL = publication.Path,
                    UserName = publication.User.UserName
                };
                viewModel.Categories = new List<string>();
                foreach (PublicationCategory pc in publication.PublicationCategories)
                {
                    viewModel.Categories.Add(pc.Category.Name);
                }
                if (publication != null)
                {
                    return View(viewModel);
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult CreatePublication()
        {
            List<string> catNames = new List<string>();
            foreach(Category cat in _db.Categories.ToList())
            {
                catNames.Add(cat.Name);
            }
            ViewBag.PublishingYears = _availablePublishingYears;
            ViewBag.Categories = catNames;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublication(CreatePublicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model != null && model.File != null)
                {
                    Publication publication = new Publication
                    {
                        Name = model.Name,
                        Annotation = model.Annotation,
                        User = await _userManager.FindByNameAsync(User.Identity.Name),
                        PublishingYear = model.PublishingYear,
                        Path = model.FilePath,
                        CreationTime = DateTime.Now
                    };

                    _db.Publications.Add(publication);
                    //await _db.SaveChangesAsync();

                    Category tempCat;
                    foreach (string cat in model.Categories)
                    {
                        tempCat = _db.Categories.FirstOrDefault(c => c.Name == cat);
                        publication.PublicationCategories.Add(new PublicationCategory { Publication = publication, CategoryId = tempCat.Id });
                    }

                    // путь к папке Files
                    string path = "/Files/" + model.File.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                        publication.Path = fileStream.Name;
                    }

                    await _db.SaveChangesAsync();
                    return RedirectToAction("ObservePublication", "Publications", new { id = publication.Id });
                }
                return NotFound();
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPublication(string id)
        {
            if (id != null)
            {
                Publication publication = await _db.Publications.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
                if (publication != null)
                {
                    EditPublicationViewModel model = new EditPublicationViewModel
                    {
                        Id = publication.Id,
                        Name = publication.Name,
                        Annotation = publication.Annotation,
                        PublishingYear = publication.PublishingYear,
                        OldFilePath = publication.Path
                    };
                    List<string> categories = new List<string>();
                    foreach(PublicationCategory pc in publication.PublicationCategories)
                    {
                        categories.Add(pc.Category.Name);
                    }
                    model.Categories = categories;

                    List<string> catNames = new List<string>();
                    foreach (Category cat in _db.Categories.ToList())
                    {
                        catNames.Add(cat.Name);
                    }
                    ViewBag.PublishingYears = _availablePublishingYears;
                    ViewBag.Categories = catNames;

                    return View(model);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditPublication(EditPublicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    Publication publication = await _db.Publications.Include(pub => pub.PublicationCategories).ThenInclude(pc => pc.Category).FirstOrDefaultAsync(p => p.Id == model.Id);
                    publication.Name = model.Name;
                    publication.Annotation = model.Annotation;
                    publication.PublishingYear = model.PublishingYear;

                    publication.PublicationCategories.RemoveRange(0, publication.PublicationCategories.Count);

                    Category tempCat;
                    foreach (string cat in model.Categories)
                    {
                        tempCat = _db.Categories.FirstOrDefault(c => c.Name == cat);
                        publication.PublicationCategories.Add(new PublicationCategory { PublicationId = publication.Id, CategoryId = tempCat.Id });
                    }

                    if (model.File != null)
                    {
                        // путь к папке Files
                        string newPath = "/Files/" + model.File.FileName;
                        string oldPath = model.OldFilePath;
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + newPath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(fileStream);
                            publication.Path = fileStream.Name;
                        }
                        System.IO.File.Delete(oldPath);
                    }

                    _db.SaveChanges();

                    return RedirectToAction("ObservePublication", "Publications", new { id = publication.Id });
                }
                return NotFound();
            }
            return View(model);
        }

        //Подтверждение удаления
        [HttpGet]
        [ActionName("DeletePublication")]
        public IActionResult ConfirmDeletePublication(string id)
        {
            if (id != null)
            {
                ViewBag.Id = id;
                return View();
            }
            return NotFound();
        }

        //Удаление
        [HttpPost]
        public async Task<IActionResult> DeletePublication(string id)
        {
            if (id != null)
            {
                Publication publication = await _db.Publications.Include(p => p.PublicationCategories).FirstOrDefaultAsync(p => p.Id == id);
                System.IO.File.Delete(publication.Path);
                _db.Publications.Remove(publication);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }

        public PhysicalFileResult DownloadFile(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLower();
            return PhysicalFile(path, types[ext]);
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/msword"},
                {".docx", "application/msword"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"}
            };
        }

        [Authorize]
        //Вывод публикаций текущего пользователя
        public IActionResult MyPublications()
        {
            List<Publication> myPublications = _db.Publications.Where(p => p.User.UserName == User.Identity.Name).Include(p => p.User).ToList();
            return View(myPublications);
        }
    }
}
