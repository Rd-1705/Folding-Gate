using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foldingGate.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTableWithSnapToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SnapToken",
                table: "orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnapToken",
                table: "orders");
        }
    }
}
