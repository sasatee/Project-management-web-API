using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLeaveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_EmployeeId",
                table: "Leaves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leaves",
                table: "Leaves");

            migrationBuilder.RenameTable(
                name: "Leaves",
                newName: "Leave");

            migrationBuilder.RenameIndex(
                name: "IX_Leaves_EmployeeId",
                table: "Leave",
                newName: "IX_Leave_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leave",
                table: "Leave",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leave_Employees_EmployeeId",
                table: "Leave",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leave_Employees_EmployeeId",
                table: "Leave");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leave",
                table: "Leave");

            migrationBuilder.RenameTable(
                name: "Leave",
                newName: "Leaves");

            migrationBuilder.RenameIndex(
                name: "IX_Leave_EmployeeId",
                table: "Leaves",
                newName: "IX_Leaves_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leaves",
                table: "Leaves",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_EmployeeId",
                table: "Leaves",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
