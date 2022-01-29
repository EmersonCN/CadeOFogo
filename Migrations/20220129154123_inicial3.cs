using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CadeOFogo.Migrations
{
    public partial class inicial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Focos",
                keyColumn: "FocoId",
                keyValue: 1,
                column: "DataSnapshot",
                value: new DateTime(2022, 1, 1, 8, 30, 52, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Focos",
                keyColumn: "FocoId",
                keyValue: 1,
                column: "DataSnapshot",
                value: new DateTime(2022, 1, 28, 8, 30, 52, 0, DateTimeKind.Unspecified));
        }
    }
}
