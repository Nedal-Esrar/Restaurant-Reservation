using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateReservationView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW ReservationsWithDetails
                AS
                SELECT
                    r.reservation_id AS ReservationId,
                    r.table_id AS ReservationTableId,
                    r.reservation_date AS ReservationDate,
                    r.party_size AS PartySize,
                    c.customer_id AS CustomerId,
                    c.first_name AS CustomerFirstName,
                    c.last_name AS CustomerLastName,
                    c.email AS CustomerEmail,
                    c.phone_number AS CustomerPhoneNumber,
                    rs.restaurant_id AS RestaurantId,
                    rs.name AS RestaurantName,
                    rs.address AS RestaurantAddress,
                    rs.phone_number AS RestaurantPhoneNumber,
                    rs.opening_hours AS RestaurantOpeningHours
                FROM Reservations AS r
                JOIN Customers AS c ON r.customer_id = c.customer_id
                JOIN Restaurants AS rs ON r.restaurant_id = rs.restaurant_id;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW ReservationsWithDetails");
        }
    }
}
