using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberPulse.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlertaTemprana : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SentStatus",
                schema: "Chip",
                table: "Chips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Alertas",
                schema: "Admi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaAlerta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoEnviado = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas",
                schema: "Admi");

            migrationBuilder.DropColumn(
                name: "SentStatus",
                schema: "Chip",
                table: "Chips");
        }
    }
}
