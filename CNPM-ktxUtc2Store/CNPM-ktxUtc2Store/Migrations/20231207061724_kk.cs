using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Migrations
{
    /// <inheritdoc />
    public partial class kk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discount",
                table: "product",
                newName: "oldprice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "oldprice",
                table: "product",
                newName: "discount");
        }
    }
}
