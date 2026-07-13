using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestekTalebiYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class DosyaBilgileriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DosyaBoyutu",
                table: "DestekTalepleri",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DosyaYuklenmeTarihi",
                table: "DestekTalepleri",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosyaBoyutu",
                table: "DestekTalepleri");

            migrationBuilder.DropColumn(
                name: "DosyaYuklenmeTarihi",
                table: "DestekTalepleri");
        }
    }
}
