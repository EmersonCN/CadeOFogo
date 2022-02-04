using Microsoft.EntityFrameworkCore.Migrations;

namespace CadeOFogo.Migrations
{
    public partial class inicial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoVegetacaoId",
                table: "Focos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Focos_TipoVegetacaoId",
                table: "Focos",
                column: "TipoVegetacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Focos_TiposVegetacao_TipoVegetacaoId",
                table: "Focos",
                column: "TipoVegetacaoId",
                principalTable: "TiposVegetacao",
                principalColumn: "TipoVegetacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Focos_TiposVegetacao_TipoVegetacaoId",
                table: "Focos");

            migrationBuilder.DropIndex(
                name: "IX_Focos_TipoVegetacaoId",
                table: "Focos");

            migrationBuilder.DropColumn(
                name: "TipoVegetacaoId",
                table: "Focos");
        }
    }
}
