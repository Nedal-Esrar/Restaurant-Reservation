using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTotalAmountTypeToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "Orders",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 1,
                column: "total_amount",
                value: 25m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 2,
                column: "total_amount",
                value: 12m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 3,
                column: "total_amount",
                value: 23m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 4,
                column: "total_amount",
                value: 29m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 5,
                column: "total_amount",
                value: 8m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 6,
                column: "total_amount",
                value: 10m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "total_amount",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 1,
                column: "total_amount",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 2,
                column: "total_amount",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 3,
                column: "total_amount",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 4,
                column: "total_amount",
                value: 29);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 5,
                column: "total_amount",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "order_id",
                keyValue: 6,
                column: "total_amount",
                value: 10);
        }
    }
}
