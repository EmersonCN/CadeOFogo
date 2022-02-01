using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.ViewsModels.Equipe
{
    public class GerenciarEquipesUsuariosViewModel
    {
        public string ApplicationUserUserId { get; set; }
        [Display(Name = "Usuario")]
        public string UserNome { get; set; }

        public bool Selecionado { get; set; }
    }
}
