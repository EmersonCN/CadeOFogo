using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CadeOFogo.Migrations
{
    public partial class inicial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Focos",
                columns: new[] { "FocoId", "ArvoresIsoladas", "AutoDeInflacaoAmbiental", "AutoDeInflacaoAmbientalA", "AutoDeInflacaoAmbientalAPP", "AutoDeInflacaoAmbientalL", "AutoDeInflacaoAmbientalRL", "AutoDeInflacaoAmbientalUC", "AutoDeInflacaoAmbientalV", "Autorizado", "Avancado", "AvancadoAPPAreaEmHectares", "AvancadoRL", "AvancadoUC", "Bioma", "CanaDeAcucar", "CausaFogoId", "CausadorProvavelId", "Citrus", "DataAtendimento", "DataSnapshot", "EstadoId", "FocoConfirmado", "FocoDataUtc", "FocoLatitude", "FocoLongitude", "IndicioInicioFocoId", "Inicial", "InicialAPPAreaEmHectares", "InicialRL", "InicialUC", "InpeFocoId", "Medio", "MedioAPPAreaEmHectares", "MedioRL", "MedioUC", "MultaA", "MultaAPP", "MultaL", "MultaR", "MultaRL", "MultaUC", "MultaV", "Municipi", "MunicipioId", "NºBOPAmb", "NºTVA", "OcorrênciaSIOPM", "Outras", "OutrasRL", "OutrasUC", "PalhaDeCana", "Pasto", "Pioneiro", "PioneiroAPPAreaEmHectares", "PioneiroRL", "PioneiroUC", "PolicialResponsavel", "RSO", "Refiscalizacao", "ResponsavelPropriedadeId", "SateliteId", "SnapshotProvider", "SnapshotSatelite", "StatusFocoId" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 1, 28, 8, 30, 52, 0, DateTimeKind.Unspecified), 1, true, new DateTime(2022, 1, 1, 8, 30, 52, 0, DateTimeKind.Unspecified), -49.0146791260056m, -21.1094449648007m, 1, null, null, null, null, "15", null, null, null, null, null, null, null, null, null, null, null, null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 1, "2184dsadas", null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Focos",
                keyColumn: "FocoId",
                keyValue: 1);
        }
    }
}
