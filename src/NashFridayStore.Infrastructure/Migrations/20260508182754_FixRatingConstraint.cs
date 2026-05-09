using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRatingConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_product_ratings_stars",
                schema: "store",
                table: "product_ratings");

            migrationBuilder.AddCheckConstraint(
                name: "ck_product_ratings_stars",
                schema: "store",
                table: "product_ratings",
                sql: "[Stars] >= 1 AND [Stars] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_product_ratings_stars",
                schema: "store",
                table: "product_ratings");

            migrationBuilder.AddCheckConstraint(
                name: "ck_product_ratings_stars",
                schema: "store",
                table: "product_ratings",
                sql: "[Stars] >= 1 AND [Stars] <= 10");
        }
    }
}
