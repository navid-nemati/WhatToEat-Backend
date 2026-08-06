using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_Practice.Migrations
{
    /// <inheritdoc />
    public partial class addimagePathtofood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Foods");
        }
    }
}
