using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class variation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "qty_inStock",
                table: "product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "variation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_variation_category_categoryId",
                        column: x => x.categoryId,
                        principalTable: "category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "variation_option",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    variationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variation_option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_variation_option_variation_variationId",
                        column: x => x.variationId,
                        principalTable: "variation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "productvariation_option",
                columns: table => new
                {
                    productsId = table.Column<int>(type: "int", nullable: false),
                    variation_OptionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productvariation_option", x => new { x.productsId, x.variation_OptionsId });
                    table.ForeignKey(
                        name: "FK_productvariation_option_product_productsId",
                        column: x => x.productsId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productvariation_option_variation_option_variation_OptionsId",
                        column: x => x.variation_OptionsId,
                        principalTable: "variation_option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productvariation_option_variation_OptionsId",
                table: "productvariation_option",
                column: "variation_OptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_variation_categoryId",
                table: "variation",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_variation_option_variationId",
                table: "variation_option",
                column: "variationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productvariation_option");

            migrationBuilder.DropTable(
                name: "variation_option");

            migrationBuilder.DropTable(
                name: "variation");

            migrationBuilder.DropColumn(
                name: "qty_inStock",
                table: "product");
        }
    }
}
