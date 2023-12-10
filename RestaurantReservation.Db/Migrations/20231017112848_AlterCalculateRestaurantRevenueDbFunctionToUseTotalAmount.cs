using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AlterCalculateRestaurantRevenueDbFunctionToUseTotalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE OR ALTER FUNCTION fn_CalculateRestaurantRevenue(@restaurant_id int)
                RETURNS DECIMAL(8, 2)
                AS
                BEGIN
                    RETURN (
                        SELECT SUM(o.total_amount)
                        FROM Orders o
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
