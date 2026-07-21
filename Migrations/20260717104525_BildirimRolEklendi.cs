using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestekTalebiYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class BildirimRolEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Bildirimler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Bildirimler");
        }
    }
}
