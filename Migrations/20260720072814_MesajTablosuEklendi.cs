using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestekTalebiYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class MesajTablosuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestekTalebiId = table.Column<int>(type: "int", nullable: false),
                    Gonderen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MesajMetni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesajlar_DestekTalepleri_DestekTalebiId",
                        column: x => x.DestekTalebiId,
                        principalTable: "DestekTalepleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_DestekTalebiId",
                table: "Mesajlar",
                column: "DestekTalebiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mesajlar");
        }
    }
}
