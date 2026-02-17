using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Product_Image_Url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContent",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageContent",
                table: "Products",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
