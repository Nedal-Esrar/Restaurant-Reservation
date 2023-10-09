using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class TablesCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    restaurants_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opening_hours = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.restaurants_id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_Employee_Restaurant_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurant",
                        principalColumn: "restaurants_id");
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.item_id);
                    table.ForeignKey(
                        name: "FK_MenuItem_Restaurant_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurant",
                        principalColumn: "restaurants_id");
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    table_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.table_id);
                    table.ForeignKey(
                        name: "FK_Table_Restaurant_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurant",
                        principalColumn: "restaurants_id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    reservation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    restaurant_id = table.Column<int>(type: "int", nullable: false),
                    table_id = table.Column<int>(type: "int", nullable: false),
                    reservation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    party_size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.reservation_id);
                    table.ForeignKey(
                        name: "FK_Reservation_Customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customer",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK_Reservation_Restaurant_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurant",
                        principalColumn: "restaurants_id");
                    table.ForeignKey(
                        name: "FK_Reservation_Table_table_id",
                        column: x => x.table_id,
                        principalTable: "Table",
                        principalColumn: "table_id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reservation_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_Order_Employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "Employee",
                        principalColumn: "employee_id");
                    table.ForeignKey(
                        name: "FK_Order_Reservation_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "Reservation",
                        principalColumn: "reservation_id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    order_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.order_item_id);
                    table.ForeignKey(
                        name: "FK_OrderItem_MenuItem_item_id",
                        column: x => x.item_id,
                        principalTable: "MenuItem",
                        principalColumn: "item_id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_order_id",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "order_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_restaurant_id",
                table: "Employee",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_restaurant_id",
                table: "MenuItem",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_employee_id",
                table: "Order",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_reservation_id",
                table: "Order",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_item_id",
                table: "OrderItem",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_order_id",
                table: "OrderItem",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_customer_id",
                table: "Reservation",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_restaurant_id",
                table: "Reservation",
                column: "restaurant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_table_id",
                table: "Reservation",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "IX_Table_restaurant_id",
                table: "Table",
                column: "restaurant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "Restaurant");
        }
    }
}
