﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.ViewsModels.Pelotao
{
    public class PelotaoIndexViewModel
    {
        [Display(Name = "Código do Pelotão", ShortName = "Cód")]
        public int PelotaoId { get; set; }


        [Required(ErrorMessage = "É obrigatório preencher o Nome")]
        [StringLength(maximumLength: 280)]
        [Display(Name = "Pelotão")]
        public string PelotaoNome { get; set; }

        [Display(Name = "Companhia")]
        public string CompanhiaNome { get; set; }

        [Display(Name = "Batalhão")]
        public string NomeBatalhao { get; set; }

    }
}
