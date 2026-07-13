using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestekTalebiYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class DosyaYuklemeEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DosyaAdi",
                table: "DestekTalepleri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DosyaYolu",
                table: "DestekTalepleri",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DosyaAdi",
                table: "DestekTalepleri");

            migrationBuilder.DropColumn(
                name: "DosyaYolu",
                table: "DestekTalepleri");
        }
    }
}