using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Models.Inpe
{
    public class FocoDetalhe : Foco
    {
        [Display(Name = "Bioma")]
        public string Bioma { get; set; }

        [Display(Name = "Municipi")]
        public string Municipi { get; set; }

        [Display(Name = "Policial Responsavel Pelo Atendimento")]
        public string PolicialResponsavel { get; set; }

        [Display(Name = "Ocorrência SIOPM")]
        public string OcorrênciaSIOPM { get; set; }

        [Display(Name = "Nº BOPAmb")]
        public string NºBOPAmb { get; set; }

        [Display(Name = "Nº TVA")]
        public string NºTVA { get; set; }

        [Display(Name = "RSO")]
        public string RSO { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data Atend.")]
        public DateTime DataAtendimento { get; set; }

        [Display(Name = "Status Do Foco")]
        public string StatusDoFoco { get; set; }

        [Display(Name = "Indicios De Início Do Foco")]
        public string IndicioDeInicioDoFoco { get; set; }

        [Display(Name = "Causa Provável")]
        public string CausaProvavel { get; set; }

        [Display(Name = "Causador Provável")]
        public string CausadorProvavel { get; set; }

        [Display(Name = "Responsavel Pela Propriedade")]
        public string ResponsavelPelaPropriedade { get; set; }

        [Display(Name = "Pioneiro (APP) - ÁREA EM HECTARES")]
        public string PioneroAPPAreaEmHectares { get; set; }

        [Display(Name = "Inicial (APP) - ÁREA EM HECTARES")]
        public string InicialAPPAreaEmHectares { get; set; }

        [Display(Name = "Medio (APP) - ÁREA EM HECTARES")]
        public string MedioAPPAreaEmHectares { get; set; }

        [Display(Name = "Avançado (APP) - ÁREA EM HECTARES")]
        public string AvancadoAPPAreaEmHectares { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbiental { get; set; }

        [Display(Name = "Multa APP")]
        public string MultaAPP { get; set; }
    }
}
