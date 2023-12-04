using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Migrations
{
    /// <inheritdoc />
    public partial class cartdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "cartDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "cartDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "cartDetail");

            migrationBuilder.DropColumn(
                name: "size",
                table: "cartDetail");
        }
    }
}
