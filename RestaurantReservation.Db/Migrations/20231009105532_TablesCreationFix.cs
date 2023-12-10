using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class TablesCreationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Restaurant_restaurant_id",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_Restaurant_restaurant_id",
                table: "MenuItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Employee_employee_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Reservation_reservation_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_MenuItem_item_id",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_order_id",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Customer_customer_id",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Restaurant_restaurant_id",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Table_table_id",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_Restaurant_restaurant_id",
                table: "Table");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Table",
                table: "Table");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItem",
                table: "MenuItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Table",
                newName: "Tables");

            migrationBuilder.RenameTable(
                name: "Restaurant",
                newName: "Restaurants");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "MenuItem",
                newName: "MenuItems");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Table_restaurant_id",
                table: "Tables",
                newName: "IX_Tables_restaurant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_table_id",
                table: "Reservations",
                newName: "IX_Reservations_table_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_restaurant_id",
                table: "Reservations",
                newName: "IX_Reservations_restaurant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_customer_id",
                table: "Reservations",
                newName: "IX_Reservations_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_order_id",
                table: "OrderItems",
                newName: "IX_OrderItems_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_item_id",
                table: "OrderItems",
                newName: "IX_OrderItems_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_reservation_id",
                table: "Orders",
                newName: "IX_Orders_reservation_id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_employee_id",
                table: "Orders",
                newName: "IX_Orders_employee_id");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_restaurant_id",
                table: "MenuItems",
                newName: "IX_MenuItems_restaurant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_restaurant_id",
                table: "Employees",
                newName: "IX_Employees_restaurant_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tables",
                table: "Tables",
                column: "table_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants",
                column: "restaurants_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "reservation_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "order_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "order_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems",
                column: "item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "employee_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Restaurants_restaurant_id",
                table: "Employees",
                column: "restaurant_id",
                principalTable: "Restaurants",
                principalColumn: "restaurants_id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Restaurants_restaurant_id",
                table: "MenuItems",
                column: "restaurant_id",
                principalTable: "Restaurants",
                principalColumn: "restaurants_id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_item_id",
                table: "OrderItems",
                column: "item_id",
                principalTable: "MenuItems",
                principalColumn: "item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_order_id",
                table: "OrderItems",
                column: "order_id",
                principalTable: "Orders",
                principalColumn: "order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_employee_id",
                table: "Orders",
                column: "employee_id",
                principalTable: "Employees",
                principalColumn: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Reservations_reservation_id",
                table: "Orders",
                column: "reservation_id",
                principalTable: "Reservations",
                principalColumn: "reservation_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_customer_id",
                table: "Reservations",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Restaurants_restaurant_id",
                table: "Reservations",
                column: "restaurant_id",
                principalTable: "Restaurants",
                principalColumn: "restaurants_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tables_table_id",
                table: "Reservations",
                column: "table_id",
                principalTable: "Tables",
                principalColumn: "table_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Restaurants_restaurant_id",
                table: "Tables",
                column: "restaurant_id",
                principalTable: "Restaurants",
                principalColumn: "restaurants_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Restaurants_restaurant_id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Restaurants_restaurant_id",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_item_id",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_order_id",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_employee_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Reservations_reservation_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_customer_id",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Restaurants_restaurant_id",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tables_table_id",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Restaurants_restaurant_id",
                table: "Tables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tables",
                table: "Tables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Tables",
                newName: "Table");

            migrationBuilder.RenameTable(
                name: "Restaurants",
                newName: "Restaurant");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItem");

            migrationBuilder.RenameTable(
                name: "MenuItems",
                newName: "MenuItem");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Tables_restaurant_id",
                table: "Table",
                newName: "IX_Table_restaurant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_table_id",
                table: "Reservation",
                newName: "IX_Reservation_table_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_restaurant_id",
                table: "Reservation",
                newName: "IX_Reservation_restaurant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_customer_id",
                table: "Reservation",
                newName: "IX_Reservation_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_reservation_id",
                table: "Order",
                newName: "IX_Order_reservation_id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_employee_id",
                table: "Order",
                newName: "IX_Order_employee_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_order_id",
                table: "OrderItem",
                newName: "IX_OrderItem_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_item_id",
                table: "OrderItem",
                newName: "IX_OrderItem_item_id");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_restaurant_id",
                table: "MenuItem",
                newName: "IX_MenuItem_restaurant_id");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_restaurant_id",
                table: "Employee",
                newName: "IX_Employee_restaurant_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table",
                table: "Table",
                column: "table_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant",
                column: "restaurants_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "reservation_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "order_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "order_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItem",
                table: "MenuItem",
                column: "item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "employee_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Restaurant_restaurant_id",
                table: "Employee",
                column: "restaurant_id",
                principalTable: "Restaurant",
                principalColumn: "restaurants_id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_Restaurant_restaurant_id",
                table: "MenuItem",
                column: "restaurant_id",
                principalTable: "Restaurant",
                principalColumn: "restaurants_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Employee_employee_id",
                table: "Order",
                column: "employee_id",
                principalTable: "Employee",
                principalColumn: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Reservation_reservation_id",
                table: "Order",
                column: "reservation_id",
                principalTable: "Reservation",
                principalColumn: "reservation_id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_MenuItem_item_id",
                table: "OrderItem",
                column: "item_id",
                principalTable: "MenuItem",
                principalColumn: "item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_order_id",
                table: "OrderItem",
                column: "order_id",
                principalTable: "Order",
                principalColumn: "order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Customer_customer_id",
                table: "Reservation",
                column: "customer_id",
                principalTable: "Customer",
                principalColumn: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Restaurant_restaurant_id",
                table: "Reservation",
                column: "restaurant_id",
                principalTable: "Restaurant",
                principalColumn: "restaurants_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Table_table_id",
                table: "Reservation",
                column: "table_id",
                principalTable: "Table",
                principalColumn: "table_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Restaurant_restaurant_id",
                table: "Table",
                column: "restaurant_id",
                principalTable: "Restaurant",
                principalColumn: "restaurants_id");
        }
    }
}
