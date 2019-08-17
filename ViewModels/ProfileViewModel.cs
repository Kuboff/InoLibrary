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
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "Не указаны ФИО")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат")]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }
    }
}
