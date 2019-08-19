using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InoLibrary.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name ="ФИО")]
        public string FullName { get; set; }

        [Display(Name ="Email")]
        public string Email { get; set; }

        [Display(Name ="Ник")]
        public string Nickname { get; set; }
    }
}
