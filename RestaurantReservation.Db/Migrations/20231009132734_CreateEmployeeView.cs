using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmployeeView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW EmployeesWithDetails
                AS
                SELECT
                    e.employee_id AS EmployeeId,
                    e.first_name AS EmployeeFirstName,
                    e.last_name AS EmployeeLastName,
                    e.position AS EmployeePosition,
                    rs.restaurant_id AS RestaurantId,
                    rs.name AS RestaurantName,
                    rs.address AS RestaurantAddress,
                    rs.phone_number AS RestaurantPhoneNumber,
                    rs.opening_hours AS RestaurantOpeningHours
                FROM Employees AS e
                JOIN Restaurants AS rs ON rs.restaurant_id = e.restaurant_id;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW EmployeesWithDetails");
        }
    }
}
