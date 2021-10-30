using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.ViewsModels
{
    public class CompanhiaEditViewModel
    {
        [Display(Name = "Código da Companhia", ShortName = "Cód")]
        public int CompanhiaId { get; set; }

        [Required(ErrorMessage = "É obrigatório preencher a Nome")]
        [StringLength(maximumLength: 280)]
        [Display(Name = "Companhia")]
        public string CompanhiaNome { get; set; }

        [Display(Name = "Batalhão")]
        public SelectList BatalhaoInputSelect { get; set; }

        public int BatalhaoId { get; set; }
    }
}
