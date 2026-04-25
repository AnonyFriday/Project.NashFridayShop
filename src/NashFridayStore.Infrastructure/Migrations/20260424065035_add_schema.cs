using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "products",
                newSchema: "store");

            migrationBuilder.RenameTable(
                name: "product_ratings",
                newName: "product_ratings",
                newSchema: "store");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "categories",
                newSchema: "store");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "products",
                schema: "store",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "product_ratings",
                schema: "store",
                newName: "product_ratings");

            migrationBuilder.RenameTable(
                name: "categories",
                schema: "store",
                newName: "categories");
        }
    }
}
