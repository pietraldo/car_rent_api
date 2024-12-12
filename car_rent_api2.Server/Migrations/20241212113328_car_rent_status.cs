using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent_api2.Server.Migrations
{
    /// <inheritdoc />
    public partial class car_rent_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a temporary integer column to store the enum values
            migrationBuilder.AddColumn<int>(
                name: "StatusEnum",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Step 2: Populate the new column based on the current string values
            migrationBuilder.Sql(@"
            UPDATE Rents
            SET StatusEnum = 
                CASE 
                    WHEN Status = 'Reserved' THEN 0
                    WHEN Status = 'Pending' THEN 1
                    WHEN Status = 'Active' THEN 2
                    WHEN Status = 'Finished' THEN 3
                    WHEN Status = 'Canceled' THEN 4
                    ELSE 0 -- Default or invalid value
                END
        ");

            // Step 3: Drop the old string column
            migrationBuilder.DropColumn(name: "Status", table: "Rents");

            // Step 4: Rename the new column to replace the old column
            migrationBuilder.RenameColumn(
                name: "StatusEnum",
                table: "Rents",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a temporary string column to restore the original string values
            migrationBuilder.AddColumn<string>(
                name: "StatusString",
                table: "Rents",
                type: "nvarchar(max)",
                nullable: true);

            // Step 2: Populate the new column based on the current integer values
            migrationBuilder.Sql(@"
            UPDATE Rents
            SET StatusString = 
                CASE 
                    WHEN Status = 0 THEN 'Reserved'
                    WHEN Status = 1 THEN 'Pending'
                    WHEN Status = 2 THEN 'Active'
                    WHEN Status = 3 THEN 'Finished'
                    WHEN Status = 4 THEN 'Canceled'
                    ELSE NULL
                END
        ");

            // Step 3: Drop the integer column
            migrationBuilder.DropColumn(name: "Status", table: "Rents");

            // Step 4: Rename the string column to replace the old column
            migrationBuilder.RenameColumn(
                name: "StatusString",
                table: "Rents",
                newName: "Status");
        }
    }
}
