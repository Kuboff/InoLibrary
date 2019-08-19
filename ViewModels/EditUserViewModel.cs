using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.ViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Не указаны ФИО")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат")]
        [Required(ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
