using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ali_Store.Migrations
{
    public partial class aproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoodFor",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproval",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodFor",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsApproval",
                table: "Products");
        }
    }
}
