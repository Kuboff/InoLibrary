using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage ="Введите старый пароль")]
        [Display(Name ="Старый пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Новый пароль не может быть пустым")]
        [Display(Name = "Новый пароль")]
        [Unlike("OldPassword", ErrorMessage = "Старый и новый пароли не должны совпадать")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Display(Name = "Повторите новый пароль")]
        [Compare("NewPassword", ErrorMessage ="Введённые пароли не совпадают" )]
        [DataType(DataType.Password)]
        public string RepeatNewPassword { get; set; }
    }
}
