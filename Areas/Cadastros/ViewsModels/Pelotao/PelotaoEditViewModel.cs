using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CadeOFogo.Areas.Cadastros.ViewsModels.Pelotao
{
    public class PelotaoEditViewModel
    {
        [Display(Name = "Código do Pelotão", ShortName = "Cód")]
        public int PelotaoId { get; set; }


        [Required(ErrorMessage = "É obrigatório preencher o Nome")]
        [StringLength(maximumLength: 280)]
        [Display(Name = "Pelotão")]
        public string PelotaoNome { get; set; }

        [Display(Name = "Companhia")]
        public SelectList CompanhiaInputSelect { get; set; }

        public int CompanhiaId { get; set; }

        [Display(Name = "Batalhao")]
        public SelectList BatalhaoInputSelect { get; set; }

        public int BatalhaoId { get; set; }
    }
}
