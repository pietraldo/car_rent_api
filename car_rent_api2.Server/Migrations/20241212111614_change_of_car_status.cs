using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent_api2.Server.Migrations
{
    /// <inheritdoc />
    public partial class change_of_car_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a temporary integer column to store the enum values
            migrationBuilder.AddColumn<int>(
                name: "StatusEnum",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Step 2: Populate the new column based on the current string values
            migrationBuilder.Sql(@"
            UPDATE Cars
            SET StatusEnum = 
                CASE 
                    WHEN Status = 'available' THEN 1
                    WHEN Status = 'rented' THEN 2
                    WHEN Status = 'norented' THEN 3
                    WHEN Status = 'in_repair' THEN 4
                    ELSE 0 -- Default or invalid value
                END
        ");

            // Step 3: Drop the old string column
            migrationBuilder.DropColumn(name: "Status", table: "Cars");

            // Step 4: Rename the new column to replace the old column
            migrationBuilder.RenameColumn(
                name: "StatusEnum",
                table: "Cars",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a temporary string column to restore the original string values
            migrationBuilder.AddColumn<string>(
                name: "StatusString",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            // Step 2: Populate the new column based on the current integer values
            migrationBuilder.Sql(@"
            UPDATE Cars
            SET StatusString = 
                CASE 
                    WHEN Status = 1 THEN 'available'
                    WHEN Status = 2 THEN 'rented'
                    WHEN Status = 3 THEN 'norented'
                    WHEN Status = 4 THEN 'in_repair'
                    ELSE NULL
                END
        ");

            // Step 3: Drop the integer column
            migrationBuilder.DropColumn(name: "Status", table: "Cars");

            // Step 4: Rename the string column to replace the old column
            migrationBuilder.RenameColumn(
                name: "StatusString",
                table: "Cars",
                newName: "Status");
        }
    }
}
