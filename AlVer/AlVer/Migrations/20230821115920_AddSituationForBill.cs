using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlVer.Migrations
{
    /// <inheritdoc />
    public partial class AddSituationForBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Situation",
                table: "Bills",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situation",
                table: "Bills");
        }
    }
}
