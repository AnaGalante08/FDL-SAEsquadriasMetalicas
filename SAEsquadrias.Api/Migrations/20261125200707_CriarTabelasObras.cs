using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAEsquadrias.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasObras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosCampo_Obras_ObraId",
                table: "RegistrosCampo");

            migrationBuilder.DropTable(
                name: "FotoRegistro");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosCampo_ObraId",
                table: "RegistrosCampo");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "RegistrosCampo");

            migrationBuilder.RenameColumn(
                name: "DataRegistro",
                table: "RegistrosCampo",
                newName: "Data");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RegistrosCampo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Fotos",
                table: "RegistrosCampo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotos",
                table: "RegistrosCampo");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "RegistrosCampo",
                newName: "DataRegistro");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RegistrosCampo",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "RegistrosCampo",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_RegistrosCampo_ObraId",
                table: "RegistrosCampo",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_FotoRegistro_RegistroCampoId",
                table: "FotoRegistro",
                column: "RegistroCampoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosCampo_Obras_ObraId",
                table: "RegistrosCampo",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
