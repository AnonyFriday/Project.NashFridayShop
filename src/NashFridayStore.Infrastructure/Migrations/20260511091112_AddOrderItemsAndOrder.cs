using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemsAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orders",
                schema: "store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    customer_email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    delivery_address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    total_price_in_usd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    stripe_checkout_session_id = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    stripe_payment_intent_id = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    order_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    payment_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    category_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    product_unit_price_in_usd = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "store",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_items_category_id",
                schema: "store",
                table: "order_items",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                schema: "store",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id",
                schema: "store",
                table: "order_items",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items",
                schema: "store");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "store");
        }
    }
}
