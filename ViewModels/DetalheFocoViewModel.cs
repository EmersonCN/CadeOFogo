using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CadeOFogo.ViewModels
{
  public class DetalheFocoViewModel
  {
    public int FocoId { get; set; }

    [Display(Name = "Coordenadas do foco")]
    public string Coordenadas { get; set; }

    public string Latitude { get; set; }
    public string Longitude { get; set; }

    [Display(Name = "Data e hora UTC do foco")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime FocoDataUtc { get; set; }

    [Display(Name = "Localidade")]
    public string Localidade { get; set; }
    
    [Display(Name = "Foco já atendido")]
    public bool FocoAtendido { get; set; }

    [Display(Name = "Foco confirmado")]
    public bool FocoConfirmado { get; set; }
    
    [Display(Name = "Mapa de localização do foco")]
    public byte[] SnapshotSatelite { get; set; }
    
    [Display(Name = "Satélite")]
    public string Satelite { get; set; }
    
    [Display(Name = "Identificação do foco no INPE")]
    public string FocoIdInpe { get; set; }
    
    [Display(Name = "Endereço aproximado")]
    public string ReverseGeocode { get; set; }
    
    [Display(Name = "Attribution")]
    public string Attribution { get; set; }

        [Display(Name = "Bioma :")]
        public string Bioma { get; set; }

        [Display(Name = "Municipe :")]
        public string Municipi { get; set; }

        [Display(Name = "Policial R. Pelo Atendimento:")]
        public string PolicialResponsavel { get; set; }

        [Display(Name = "Ocorrência SIOPM :")]
        public string OcorrênciaSIOPM { get; set; }

        [Display(Name = "Nº BOPAmb :")]
        public string NºBOPAmb { get; set; }

        [Display(Name = "Nº TVA :")]
        public string NºTVA { get; set; }

        [Display(Name = "RSO :")]
        public string RSO { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data Atendendimento :")]
        public DateTime DataAtendimento { get; set; }

        public int EquipeId { get; set; }
        [Display(Name = "Equipe :")]
        public string EquipeNome { get; set; }

        public int StatusFocoId { get; set; }
        [Display(Name = "Status Do Foco :")]
        public string StatusFocoDescricao { get; set; }

        public int IndicioInicioFocoId { get; set; }
        [Display(Name = "Indicios De Início Do Foco :")]
        public string IndicioInicioFocoDescricao { get; set; }

        public int CausaFogoId { get; set; }
        [Display(Name = "Causa Provável :")]
        public string CausaFogoDescricao { get; set; }

        public int CausadorProvavelId { get; set; }
        [Display(Name = "Causador Provável : ")]
        public string CausadorProvavelDescricacao { get; set; }

        public int ResponsavelPropriedadeId { get; set; }
        [Display(Name = "Responsavel Pela Propriedade :")]
        public string ResponsavelPropriedadeDescricao { get; set; }

        public int TipoVegetacaoId { get; set; }
        [Display(Name = "Tipo de Vegetação :")]
        public string TipoVegetacaoDescricao { get; set; }

        [Display(Name = "Pioneiro (APP) - ÁREA EM HECTARES :")]
        public string PioneiroAPPAreaEmHectares { get; set; }

        [Display(Name = "Inicial (APP) - ÁREA EM HECTARES :")]
        public string InicialAPPAreaEmHectares { get; set; }

        [Display(Name = "Medio (APP) - ÁREA EM HECTARES :")]
        public string MedioAPPAreaEmHectares { get; set; }

        [Display(Name = "Avançado (APP) - ÁREA EM HECTARES :")]
        public string AvancadoAPPAreaEmHectares { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbientalAPP { get; set; }

        [Display(Name = "Multa APP :")]
        public string MultaAPP { get; set; }

        [Display(Name = "Pioneiro :")]
        public string Pioneiro { get; set; }

        [Display(Name = "Inicial :")]
        public string Inicial { get; set; }

        [Display(Name = "Médio :")]
        public string Medio { get; set; }

        [Display(Name = "Avançado :")]
        public string Avancado { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbiental { get; set; }

        [Display(Name = "Multa :")]
        public string MultaR { get; set; }

        [Display(Name = "Pasto :")]
        public string Pasto { get; set; }

        [Display(Name = "Citrus :")]
        public string Citrus { get; set; }

        [Display(Name = "Outras (Eucalipto,Pinus,Etc) :")]
        public string Outras { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbientalV { get; set; }

        [Display(Name = "Multa :")]
        public string MultaV { get; set; }

        [Display(Name = "Arvvores Isoladas :")]
        public string ArvoresIsoladas { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbientalA { get; set; }

        [Display(Name = "Multa :")]
        public string MultaA { get; set; }

        [Display(Name = "Plaha de Cana :")]
        public string PalhaDeCana { get; set; }

        [Display(Name = "Cana-de-Açucar :")]
        public string CanaDeAcucar { get; set; }

        [Display(Name = "Autorizado (Sim/Não) :")]
        public string Autorizado { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbientalL { get; set; }

        [Display(Name = "Multa :")]
        public string MultaL { get; set; }

        [Display(Name = "Pioneiro(UC) :")]
        public string PioneiroUC { get; set; }

        [Display(Name = "Inicial(UC) :")]
        public string InicialUC { get; set; }

        [Display(Name = "Médio(UC) : ")]
        public string MedioUC { get; set; }

        [Display(Name = "Avançado(UC) :")]
        public string AvancadoUC { get; set; }

        [Display(Name = "Outras(UC) (Eucalipto,Pinus,Etc) :")]
        public string OutrasUC { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbientalUC { get; set; }

        [Display(Name = "Multa :")]
        public string MultaUC { get; set; }

        [Display(Name = "Pioneiro(RL) :")]
        public string PioneiroRL { get; set; }

        [Display(Name = "Inicial(RL) :")]
        public string InicialRL { get; set; }

        [Display(Name = "Médio(RL) :")]
        public string MedioRL { get; set; }

        [Display(Name = "Avançado(RL) :")]
        public string AvancadoRL { get; set; }

        [Display(Name = "Outras(RL) (Eucalipto,Pinus,Etc) :")]
        public string OutrasRL { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades) :")]
        public string AutoDeInflacaoAmbientalRL { get; set; }

        [Display(Name = "Multa :")]
        public string MultaRL { get; set; }

        [Display(Name = "Refiscalização :")]
        public string Refiscalizacao { get; set; }

    }
}