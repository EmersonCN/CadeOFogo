using Microsoft.EntityFrameworkCore.Migrations;

namespace CadeOFogo.Migrations
{
    public partial class inicial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Equipes");

            migrationBuilder.UpdateData(
                table: "Equipes",
                keyColumn: "EquipeId",
                keyValue: 1,
                column: "Ativa",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Equipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Equipes",
                keyColumn: "EquipeId",
                keyValue: 1,
                columns: new[] { "Ativa", "UserId" },
                values: new object[] { false, "1" });
        }
    }
}
