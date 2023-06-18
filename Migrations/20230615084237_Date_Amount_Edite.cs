using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ali_Store.Migrations
{
    public partial class Date_Amount_Edite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsSall",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsSall",
                table: "Products");
        }
    }
}
