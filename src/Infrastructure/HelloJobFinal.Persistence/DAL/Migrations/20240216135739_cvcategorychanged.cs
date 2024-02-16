using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloJobFinal.Persistence.DAL.Migrations
{
    public partial class cvcategorychanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Cvs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Cvs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
