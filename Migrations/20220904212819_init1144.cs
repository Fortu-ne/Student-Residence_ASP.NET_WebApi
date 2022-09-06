using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WepApiWithToken.Migrations
{
    public partial class init1144 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_MaintenanceTypeId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_StatusTypes_StatusId",
                table: "Maintenances");

            migrationBuilder.AlterColumn<string>(
                name: "StudId",
                table: "Maintenances",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Maintenances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaintenanceTypeId",
                table: "Maintenances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_MaintenanceTypeId",
                table: "Maintenances",
                column: "MaintenanceTypeId",
                principalTable: "MaintenanceTypes",
                principalColumn: "MaintenanceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_StatusTypes_StatusId",
                table: "Maintenances",
                column: "StatusId",
                principalTable: "StatusTypes",
                principalColumn: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_MaintenanceTypeId",
                table: "Maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_StatusTypes_StatusId",
                table: "Maintenances");

            migrationBuilder.AlterColumn<string>(
                name: "StudId",
                table: "Maintenances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaintenanceTypeId",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_MaintenanceTypes_MaintenanceTypeId",
                table: "Maintenances",
                column: "MaintenanceTypeId",
                principalTable: "MaintenanceTypes",
                principalColumn: "MaintenanceTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_StatusTypes_StatusId",
                table: "Maintenances",
                column: "StatusId",
                principalTable: "StatusTypes",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
