using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InforStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    namestorage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkfacbook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkInstagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkyoutube = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linktiktok = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InforStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdressStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InforStorageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdressStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdressStorage_InforStorage_InforStorageId",
                        column: x => x.InforStorageId,
                        principalTable: "InforStorage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannerStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bannerpicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InforStorageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerStorage_InforStorage_InforStorageId",
                        column: x => x.InforStorageId,
                        principalTable: "InforStorage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdressStorage_InforStorageId",
                table: "AdressStorage",
                column: "InforStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerStorage_InforStorageId",
                table: "BannerStorage",
                column: "InforStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdressStorage");

            migrationBuilder.DropTable(
                name: "BannerStorage");

            migrationBuilder.DropTable(
                name: "InforStorage");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "order");
        }
    }
}
