using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGT_Web_Account.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAccountUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "accountType",
                table: "Accounts",
                newName: "userType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userType",
                table: "Accounts",
                newName: "accountType");
        }
    }
}
