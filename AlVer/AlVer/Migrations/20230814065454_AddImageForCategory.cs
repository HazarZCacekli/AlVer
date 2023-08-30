using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlVer.Migrations
{
    /// <inheritdoc />
    public partial class AddImageForCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Category",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Category");
        }
    }
}
