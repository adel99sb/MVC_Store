using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ali_Store.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Products",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // migrationBuilder.AddColumn<bool>(
            //     name: "IsSall",
            //     table: "Products",
            //     type: "bit",
            //     nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // migrationBuilder.AlterColumn<string>(
            //     name: "Discriminator",
            //     table: "Products",
            //     type: "nvarchar(max)",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(8)",
            //     oldMaxLength: 8);
        }
    }
}
