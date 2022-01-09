using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aula6.Migrations
{
    public partial class ScriptMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "decimal(10, 2)",
                table: "Product",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "decimal(10, 2)",
                table: "Orders",
                newName: "TotalValue");

            migrationBuilder.RenameColumn(
                name: "decimal(10, 2)",
                table: "Item",
                newName: "Price");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Item",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Product",
                newName: "decimal(10, 2)");

            migrationBuilder.RenameColumn(
                name: "TotalValue",
                table: "Orders",
                newName: "decimal(10, 2)");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Item",
                newName: "decimal(10, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "decimal(10, 2)",
                table: "Product",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "decimal(10, 2)",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "decimal(10, 2)",
                table: "Item",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }
    }
}
