using Microsoft.EntityFrameworkCore.Migrations;

namespace Vets.Migrations
{
    public partial class correcaoDadosVeterinarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumCedulaProf",
                table: "Veterinarios",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Veterinarios",
                keyColumn: "ID",
                keyValue: 1,
                column: "NumCedulaProf",
                value: "vet-34589");

            migrationBuilder.UpdateData(
                table: "Veterinarios",
                keyColumn: "ID",
                keyValue: 2,
                column: "NumCedulaProf",
                value: "vet-34590");

            migrationBuilder.UpdateData(
                table: "Veterinarios",
                keyColumn: "ID",
                keyValue: 3,
                column: "NumCedulaProf",
                value: "vet-56732");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumCedulaProf",
                table: "Veterinarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9);

            migrationBuilder.UpdateData(
                table: "Veterinarios",
                keyColumn: "ID",
                keyValue: 1,
                column: "NumCedulaProf",
                value: " vet-34589");

            migrationBuilder.UpdateData(
                table: "Veterinarios",
                keyColumn: "ID",
                keyValue: 2,
                column: "NumCedulaProf",
                value: " vet-34590");

            migrationBuilder.UpdateData(
                table: "Veterinarios",
                keyColumn: "ID",
                keyValue: 3,
                column: "NumCedulaProf",
                value: " vet-56732");
        }
    }
}
