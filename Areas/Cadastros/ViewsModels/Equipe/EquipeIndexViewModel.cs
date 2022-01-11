using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.ViewsModels.Equipe
{
    public class EquipeIndexViewModel
    {
        [Display(Name = "Codigo da Equipe", ShortName = "Cód")]
        public int EquipeId{ get; set; }

        [Required(ErrorMessage = "É obrigatório preencher o Nome")]
        [StringLength(maximumLength: 80)]
        [Display(Name = "Equipe")]
        public string EquipeNome { get; set; }

        [Display(Name = "Batalhão")]
        public string NomeBatalhao { get; set; }

        [Display(Name = "Companhia")]
        public string CompanhiaNome { get; set; }

        [Display(Name = "Pelotao")]
        public string PelotaoNome { get; set; }

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; }

        [Display(Name = "Nome Policiais")]
        public string Nome { get; set; }

    }
}
