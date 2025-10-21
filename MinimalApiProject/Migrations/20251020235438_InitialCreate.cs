using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuANA_VICENTE.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: false),
                    Mes = table.Column<int>(type: "INTEGER", nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    M3Consumidos = table.Column<double>(type: "REAL", nullable: false),
                    ConsumoFaturado = table.Column<double>(type: "REAL", nullable: false),
                    Tarifa = table.Column<double>(type: "REAL", nullable: false),
                    ValorAgua = table.Column<double>(type: "REAL", nullable: false),
                    AdicionalBandeira = table.Column<double>(type: "REAL", nullable: false),
                    TaxaEsgoto = table.Column<double>(type: "REAL", nullable: false),
                    Total = table.Column<double>(type: "REAL", nullable: false),
                    Bandeira = table.Column<string>(type: "TEXT", nullable: false),
                    PossuiEsgoto = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consumos_Cpf_Mes_Ano",
                table: "Consumos",
                columns: new[] { "Cpf", "Mes", "Ano" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consumos");
        }
    }
}
