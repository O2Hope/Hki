using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace hki.web.Migrations
{
    public partial class Ordenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Asignado = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    Estatus2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estatus3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaReq = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Finalizadas = table.Column<int>(type: "int", nullable: false),
                    Levantamiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalHrs = table.Column<float>(type: "real", nullable: false),
                    Ubicacion = table.Column<int>(type: "int", nullable: false),
                    ValorHrs = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorHrs = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
