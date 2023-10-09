using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "customer_id", "email", "first_name", "last_name", "phone_number" },
                values: new object[,]
                {
                    { 1, "customer1@example.com", "John", "Doe", "123-456-7890" },
                    { 2, "customer2@example.com", "Jane", "Smith", "987-654-3210" },
                    { 3, "customer3@example.com", "Mike", "Johnson", "555-123-4567" },
                    { 4, "customer4@example.com", "Emily", "Wilson", "555-987-6543" },
                    { 5, "customer5@example.com", "Chris", "Brown", "555-555-5555" }
                });

            migrationBuilder.InsertData(
                table: "Restaurant",
                columns: new[] { "restaurants_id", "address", "name", "opening_hours", "phone_number" },
                values: new object[,]
                {
                    { 1, "123 Main St", "Restaurant A", "9:00 AM - 10:00 PM", "555-123-4567" },
                    { 2, "456 Elm St", "Restaurant B", "10:00 AM - 9:00 PM", "555-987-6543" },
                    { 3, "789 Oak St", "Restaurant C", "8:00 AM - 11:00 PM", "555-111-2222" },
                    { 4, "101 Pine St", "Restaurant D", "11:00 AM - 8:00 PM", "555-333-4444" },
                    { 5, "222 Cedar St", "Restaurant E", "7:00 AM - 9:00 PM", "555-666-7777" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "employee_id", "first_name", "last_name", "position", "restaurant_id" },
                values: new object[,]
                {
                    { 1, "Alice", "Smith", 4, 1 },
                    { 2, "Bob", "Johnson", 2, 2 },
                    { 3, "Charlie", "Brown", 4, 3 },
                    { 4, "David", "Wilson", 3, 4 },
                    { 5, "Eva", "Davis", 1, 5 }
                });

            migrationBuilder.InsertData(
                table: "MenuItem",
                columns: new[] { "item_id", "description", "name", "price", "restaurant_id" },
                values: new object[,]
                {
                    { 1, "Delicious Burger", "Burger", 10.99m, 1 },
                    { 2, "Classic Margherita Pizza", "Margherita Pizza", 12.99m, 2 },
                    { 3, "Fresh Garden Salad", "Garden Salad", 7.99m, 3 },
                    { 4, "Spaghetti with Meatballs", "Spaghetti", 14.99m, 4 },
                    { 5, "Chocolate Brownie Sundae", "Brownie Sundae", 8.99m, 5 }
                });

            migrationBuilder.InsertData(
                table: "Table",
                columns: new[] { "table_id", "capacity", "restaurant_id" },
                values: new object[,]
                {
                    { 1, 4, 1 },
                    { 2, 2, 2 },
                    { 3, 6, 3 },
                    { 4, 3, 4 },
                    { 5, 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Reservation",
                columns: new[] { "reservation_id", "customer_id", "party_size", "reservation_date", "restaurant_id", "table_id" },
                values: new object[,]
                {
                    { 1, 1, 4, new DateTime(2023, 9, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, 2, 2, new DateTime(2023, 10, 2, 19, 30, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 3, 3, 6, new DateTime(2023, 10, 5, 20, 15, 0, 0, DateTimeKind.Unspecified), 3, 3 },
                    { 4, 4, 3, new DateTime(2023, 10, 7, 17, 45, 0, 0, DateTimeKind.Unspecified), 4, 4 },
                    { 5, 5, 5, new DateTime(2023, 10, 10, 21, 0, 0, 0, DateTimeKind.Unspecified), 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "order_id", "employee_id", "order_date", "reservation_id", "total_amount" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 9, 30, 19, 30, 0, 0, DateTimeKind.Unspecified), 1, 25 },
                    { 2, 2, new DateTime(2023, 10, 2, 20, 0, 0, 0, DateTimeKind.Unspecified), 2, 12 },
                    { 3, 3, new DateTime(2023, 10, 5, 21, 15, 0, 0, DateTimeKind.Unspecified), 3, 23 },
                    { 4, 4, new DateTime(2023, 10, 7, 18, 45, 0, 0, DateTimeKind.Unspecified), 4, 29 },
                    { 5, 5, new DateTime(2023, 10, 10, 22, 0, 0, 0, DateTimeKind.Unspecified), 5, 8 }
                });

            migrationBuilder.InsertData(
                table: "OrderItem",
                columns: new[] { "order_item_id", "item_id", "order_id", "quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 2, 2, 1 },
                    { 3, 3, 3, 3 },
                    { 4, 4, 4, 2 },
                    { 5, 5, 5, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "order_item_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "order_item_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "order_item_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "order_item_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItem",
                keyColumn: "order_item_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItem",
                keyColumn: "item_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItem",
                keyColumn: "item_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItem",
                keyColumn: "item_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItem",
                keyColumn: "item_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItem",
                keyColumn: "item_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "order_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "order_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "order_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "order_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "order_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "employee_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "employee_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "employee_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "employee_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "employee_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "reservation_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "reservation_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "reservation_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "reservation_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "reservation_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "customer_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "customer_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "customer_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "customer_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "customer_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Table",
                keyColumn: "table_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Table",
                keyColumn: "table_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Table",
                keyColumn: "table_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Table",
                keyColumn: "table_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Table",
                keyColumn: "table_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurant",
                keyColumn: "restaurants_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurant",
                keyColumn: "restaurants_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurant",
                keyColumn: "restaurants_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Restaurant",
                keyColumn: "restaurants_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurant",
                keyColumn: "restaurants_id",
                keyValue: 5);
        }
    }
}
