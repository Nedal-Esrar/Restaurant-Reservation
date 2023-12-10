using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateFindCustomersWithPartySizeLargerThanStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE PROCEDURE sp_FindCustomersWithPartySizeLargerThan
                @min_party_size INT
                AS
                BEGIN
                    SELECT DISTINCT c.*
                    FROM Customers c
                    JOIN Reservations r ON c.customer_id = r.customer_id
                    WHERE r.party_size > @min_party_size;
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_FindCustomersWithPartySizeLargerThan");
        }
    }
}
