using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlVer.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameForBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Bills",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Bills");
        }
    }
}
