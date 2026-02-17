using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Delete_Product_Image_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
