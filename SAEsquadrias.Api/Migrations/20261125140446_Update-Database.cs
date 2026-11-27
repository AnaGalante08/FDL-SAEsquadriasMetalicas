using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAEsquadrias.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pendencias_Projeto_ProjetoId",
                table: "Pendencias");

            migrationBuilder.DropTable(
                name: "Projeto");

            migrationBuilder.RenameColumn(
                name: "ProjetoId",
                table: "Pendencias",
                newName: "ObraId");

            migrationBuilder.RenameIndex(
                name: "IX_Pendencias_ProjetoId",
                table: "Pendencias",
                newName: "IX_Pendencias_ObraId");

            migrationBuilder.CreateTable(
                name: "RegistrosCampo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObraId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosCampo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosCampo_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perfil = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FotoRegistro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistroCampoId = table.Column<int>(type: "int", nullable: false),
                    Caminho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeOriginal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoRegistro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotoRegistro_RegistrosCampo_RegistroCampoId",
                        column: x => x.RegistroCampoId,
                        principalTable: "RegistrosCampo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FotoRegistro_RegistroCampoId",
                table: "FotoRegistro",
                column: "RegistroCampoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosCampo_ObraId",
                table: "RegistrosCampo",
                column: "ObraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pendencias_Obras_ObraId",
                table: "Pendencias",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pendencias_Obras_ObraId",
                table: "Pendencias");

            migrationBuilder.DropTable(
                name: "FotoRegistro");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "RegistrosCampo");

            migrationBuilder.RenameColumn(
                name: "ObraId",
                table: "Pendencias",
                newName: "ProjetoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pendencias_ObraId",
                table: "Pendencias",
                newName: "IX_Pendencias_ProjetoId");

            migrationBuilder.CreateTable(
                name: "Projeto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Pendencias_Projeto_ProjetoId",
                table: "Pendencias",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
