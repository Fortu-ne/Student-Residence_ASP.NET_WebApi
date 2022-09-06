using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WepApiWithToken.Migrations
{
    public partial class init15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dateOfBirth",
                table: "Students",
                newName: "BirthDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Students",
                newName: "dateOfBirth");
        }
    }
}
