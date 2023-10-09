using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateCalculateRestaurantRevenueDbFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE FUNCTION fn_CalculateRestaurantRevenue(@restaurant_id int)
                RETURNS DECIMAL(8, 2)
                AS
                BEGIN
                    RETURN (
                        SELECT SUM(oi.quantity * mi.price)
                        FROM OrderItems oi
                        JOIN Orders o ON oi.order_id = o.order_id
                        JOIN MenuItems mi ON oi.item_id = mi.item_id
                        JOIN Reservations r ON o.reservation_id = r.reservation_id
                        WHERE r.restaurant_id = @restaurant_id
                    );
                END;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION fn_CalculateRestaurantRevenue");
        }
    }
}
