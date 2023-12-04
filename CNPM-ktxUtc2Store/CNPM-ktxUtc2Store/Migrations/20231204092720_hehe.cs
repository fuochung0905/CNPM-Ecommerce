using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Migrations
{
    /// <inheritdoc />
    public partial class hehe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDefine",
                table: "userAdresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDefine",
                table: "userAdresses");
        }
    }
}
