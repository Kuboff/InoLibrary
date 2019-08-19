using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.ViewModels
{
    public class ConfirmPasswordViewModel
    {
        public string Email { get; set; }

        [Display(Name ="Подтвердите пароль")]
        [Required(ErrorMessage ="Введите пароль")]
        public string Password { get; set; }
    }
}
