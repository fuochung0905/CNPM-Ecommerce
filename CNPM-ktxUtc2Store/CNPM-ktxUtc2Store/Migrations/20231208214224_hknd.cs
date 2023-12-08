using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Migrations
{
    /// <inheritdoc />
    public partial class hknd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "emailcskh",
                table: "InforStorage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "emailwork",
                table: "InforStorage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phonenumbershop",
                table: "InforStorage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "timework",
                table: "InforStorage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emailcskh",
                table: "InforStorage");

            migrationBuilder.DropColumn(
                name: "emailwork",
                table: "InforStorage");

            migrationBuilder.DropColumn(
                name: "phonenumbershop",
                table: "InforStorage");

            migrationBuilder.DropColumn(
                name: "timework",
                table: "InforStorage");
        }
    }
}
