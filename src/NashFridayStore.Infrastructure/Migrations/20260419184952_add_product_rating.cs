using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_product_rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_ratings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stars = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    deleted_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_ratings", x => x.id);
                    table.CheckConstraint("ck_product_ratings_stars", "[Stars] >= 1 AND [Stars] <= 10");
                    table.ForeignKey(
                        name: "fk_product_ratings_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_is_deleted",
                table: "product_ratings",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_product_id",
                table: "product_ratings",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_stars",
                table: "product_ratings",
                column: "stars");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_ratings");
        }
    }
}
