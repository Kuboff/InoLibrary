using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using InoLibrary.Models;

namespace InoLibrary.ViewModels
{
    public class ObservePublicationViewModel
    {
        public string Id { get; set; }

        [Display(Name ="Название")]
        public string Name { get; set; }

        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }

        [Display(Name = "Автор")]
        public string AuthorNickname { get; set; }

        [Display(Name = "Год издания")]
        public int PublishingYear { get; set; }

        [Display(Name = "Рубрики")]
        public List<string> Categories { get; set; }

        [DataType(DataType.Url)]
        public string DownloadURL { get; set; }

        public string UserName { get; set; }
    }
}
