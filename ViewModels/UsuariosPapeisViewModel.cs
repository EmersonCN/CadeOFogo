using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.ViewModels
{
    public class UsuariosPapeisViewModel
    {
        [Display(Name = "Id")]
        public string UserId { get; set; }
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }
        [Display(Name = "Nome de Usuario")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Papeis")]
        public IEnumerable<string> Papeis { get; set; }
    }
}
