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
        public string AutoDeInflacaoAmbientalAPP { get; set; }

        [Display(Name = "Multa APP")]
        public string MultaAPP { get; set; }

        [Display(Name = "Pioneiro")]
        public string Pioneiro { get; set; }

        [Display(Name = "Inicial")]
        public string Inicial { get; set; }

        [Display(Name = "Médio")]
        public string Medio { get; set; }

        [Display(Name = "Avançado")]
        public string Avancado { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbiental { get; set; }

        [Display(Name = "Multa")]
        public string MultaR { get; set; }

        [Display(Name = "Pasto")]
        public string Pasto { get; set; }

        [Display(Name = "Citrus")]
        public string Citrus { get; set; }

        [Display(Name = "Outras (Eucalipto,Pinus,Etc)")]
        public string Outras { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalV { get; set; }

        [Display(Name = "Multa")]
        public string MultaV { get; set; }

        [Display(Name = "Arvvores Isoladas")]
        public string ArvoresIsoladas { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalA { get; set; }

        [Display(Name = "Multa")]
        public string MultaA { get; set; }

        [Display(Name = "Plaha de Cana")]
        public string PalhaDeCana { get; set; }

        [Display(Name = "Cana-de-Açucar")]
        public string CanaDeAcucar { get; set; }

        [Display(Name = "Atorizado (Sim/Não)")]
        public string Altorizado { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalL { get; set; }

        [Display(Name = "Multa")]
        public string MultaL { get; set; }

        [Display(Name = "Pioneiro(UC)")]
        public string PioneiroUC { get; set; }

        [Display(Name = "Inicial(UC)")]
        public string InicialUC { get; set; }

        [Display(Name = "Médio(UC)")]
        public string MedioUC { get; set; }

        [Display(Name = "Avançado(UC)")]
        public string AvancadoUC { get; set; }

        [Display(Name = "Outras(UC) (Eucalipto,Pinus,Etc)")]
        public string OutrasUC { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalUC { get; set; }

        [Display(Name = "Multa")]
        public string MultaUC { get; set; }

        [Display(Name = "Pioneiro(RL)")]
        public string PioneiroRL { get; set; }

        [Display(Name = "Inicial(RL)")]
        public string InicialRL { get; set; }

        [Display(Name = "Médio(RL)")]
        public string MedioRL { get; set; }

        [Display(Name = "Avançado(RL)")]
        public string AvancadoRL { get; set; }

        [Display(Name = "Outras(RL) (Eucalipto,Pinus,Etc)")]
        public string OutrasRL { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalRL { get; set; }

        [Display(Name = "Multa")]
        public string MultaRL { get; set; }

        [Display(Name = "Refiscalização")]
        public string Refiscalizacao { get; set; }
    }
}
