using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerIdToProductRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_ratings_product_id",
                schema: "store",
                table: "product_ratings");

            migrationBuilder.Sql("DELETE FROM [store].[product_ratings]");

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                schema: "store",
                table: "product_ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_customer_id",
                schema: "store",
                table: "product_ratings",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ux_product_ratings_product_customer_id",
                schema: "store",
                table: "product_ratings",
                columns: new[] { "product_id", "customer_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_ratings_customer_id",
                schema: "store",
                table: "product_ratings");

            migrationBuilder.DropIndex(
                name: "ux_product_ratings_product_customer_id",
                schema: "store",
                table: "product_ratings");

            migrationBuilder.DropColumn(
                name: "customer_id",
                schema: "store",
                table: "product_ratings");

            migrationBuilder.CreateIndex(
                name: "ix_product_ratings_product_id",
                schema: "store",
                table: "product_ratings",
                column: "product_id");
        }
    }
}
