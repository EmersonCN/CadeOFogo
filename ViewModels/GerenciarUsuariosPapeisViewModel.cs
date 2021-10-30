using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.ViewModels
{
    public class GerenciarUsuariosPapeisViewModel
    {
        public string PapelId{ get; set; }
        [Display(Name = "Papel")]
        public string PapelNome { get; set; }
        public bool Selecionado { get; set; }
    }
}
