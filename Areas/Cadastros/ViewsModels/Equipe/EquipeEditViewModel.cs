using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.ViewsModels.Equipe
{
    public class EquipeEditViewModel
    {
        [Display(Name = "Código da Equipe", ShortName = "Cód")]
        public int EquipeId { get; set; }

        [Required(ErrorMessage = "É obrigatório preencher a Nome")]
        [StringLength(maximumLength: 80)]
        [Display(Name = "Equipe")]
        public string EquipeNome { get; set; }

        [Display(Name = "Batalhão")]
        public SelectList BatalhaoInputSelect { get; set; }

        public int BatalhaoId { get; set; }

        [Display(Name = "Companhia")]
        public SelectList CompanhiaInputSelect { get; set; }

        public int CompanhiaId { get; set; }

        [Display(Name = "Pelotão")]
        public SelectList PelotaoInputSelect { get; set; }

        public int PelotaoId { get; set; }

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; }

        [Display(Name = "Nome Usuarios")]
        public SelectList UsuarioInputSelect { get; set; }

        public string ApplicationUserUserId { get; set; }

    }
}
