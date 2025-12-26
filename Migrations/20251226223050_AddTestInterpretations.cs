using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofia.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTestInterpretations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestInterpretations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TestId = table.Column<int>(type: "INTEGER", nullable: false),
                    MinPercent = table.Column<double>(type: "REAL", nullable: false),
                    MaxPercent = table.Column<double>(type: "REAL", nullable: false),
                    Level = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    InterpretationText = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestInterpretations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestInterpretations_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestInterpretations_TestId",
                table: "TestInterpretations",
                column: "TestId");

            // Seed interpretation rules for built-in scales (IDs from seed)
            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1001, 0.0, 49.9999, "Низкий риск", "Не выявлено выраженных симптомов депрессии. При ухудшении рекомендована полная оценка (PHQ-9)." });

            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1001, 50.0, 100.0, "Показательные симптомы", "Результат выше порога — рекомендуется полная оценка (PHQ-9) или консультация специалиста." });

            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1002, 0.0, 49.9999, "Низкий риск", "Не выявлено выраженных симптомов тревоги. При ухудшении рекомендована полная оценка (GAD-7)." });

            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1002, 50.0, 100.0, "Показательные симптомы", "Результат выше порога — рекомендуется полная оценка (GAD-7) или консультация специалиста." });

            // PSS-4 splits
            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1003, 0.0, 33.0, "Низкий уровень стресса", "Низкий уровень воспринимаемого стресса. Поддерживающие практики и самоконтроль могут помочь." });

            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1003, 33.0001, 66.0, "Средний уровень стресса", "Средний уровень стресса — рекомендуется обратить внимание на технику релаксации и регулярное отслеживание." });

            migrationBuilder.InsertData(
                table: "TestInterpretations",
                columns: new[] { "TestId", "MinPercent", "MaxPercent", "Level", "InterpretationText" },
                values: new object[] { 1003, 66.0001, 100.0, "Высокий уровень стресса", "Высокий уровень воспринимаемого стресса — рекомендуется консультация специалиста и целенаправленные интервенции." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestInterpretations");
        }
    }
}
