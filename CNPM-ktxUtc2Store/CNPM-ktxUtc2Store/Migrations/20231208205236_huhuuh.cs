using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_ktxUtc2Store.Migrations
{
    /// <inheritdoc />
    public partial class huhuuh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdressStorage_InforStorage_InforStorageId",
                table: "AdressStorage");

            migrationBuilder.DropForeignKey(
                name: "FK_BannerStorage_InforStorage_InforStorageId",
                table: "BannerStorage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BannerStorage",
                table: "BannerStorage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdressStorage",
                table: "AdressStorage");

            migrationBuilder.RenameTable(
                name: "BannerStorage",
                newName: "bannerStorage");

            migrationBuilder.RenameTable(
                name: "AdressStorage",
                newName: "adressStorage");

            migrationBuilder.RenameIndex(
                name: "IX_BannerStorage_InforStorageId",
                table: "bannerStorage",
                newName: "IX_bannerStorage_InforStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_AdressStorage_InforStorageId",
                table: "adressStorage",
                newName: "IX_adressStorage_InforStorageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bannerStorage",
                table: "bannerStorage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_adressStorage",
                table: "adressStorage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_adressStorage_InforStorage_InforStorageId",
                table: "adressStorage",
                column: "InforStorageId",
                principalTable: "InforStorage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bannerStorage_InforStorage_InforStorageId",
                table: "bannerStorage",
                column: "InforStorageId",
                principalTable: "InforStorage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adressStorage_InforStorage_InforStorageId",
                table: "adressStorage");

            migrationBuilder.DropForeignKey(
                name: "FK_bannerStorage_InforStorage_InforStorageId",
                table: "bannerStorage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bannerStorage",
                table: "bannerStorage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_adressStorage",
                table: "adressStorage");

            migrationBuilder.RenameTable(
                name: "bannerStorage",
                newName: "BannerStorage");

            migrationBuilder.RenameTable(
                name: "adressStorage",
                newName: "AdressStorage");

            migrationBuilder.RenameIndex(
                name: "IX_bannerStorage_InforStorageId",
                table: "BannerStorage",
                newName: "IX_BannerStorage_InforStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_adressStorage_InforStorageId",
                table: "AdressStorage",
                newName: "IX_AdressStorage_InforStorageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BannerStorage",
                table: "BannerStorage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdressStorage",
                table: "AdressStorage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdressStorage_InforStorage_InforStorageId",
                table: "AdressStorage",
                column: "InforStorageId",
                principalTable: "InforStorage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BannerStorage_InforStorage_InforStorageId",
                table: "BannerStorage",
                column: "InforStorageId",
                principalTable: "InforStorage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
