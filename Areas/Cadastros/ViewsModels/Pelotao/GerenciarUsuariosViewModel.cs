using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.ViewsModels.Pelotao
{
    public class GerenciarUsuariosViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "Usuario")]
        public string NomeCompleto { get; set; }
        public bool Selecionado { get; set; }


    }
}
