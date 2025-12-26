using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sofia.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedBuiltInScales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "CreatedByPsychologistId", "Description", "Name", "Type" },
                values: new object[,]
                {
                    { 1001, null, "Краткий скрининг симптомов депрессии", "Шкала депрессии (PHQ-2)", 0 },
                    { 1002, null, "Краткий скрининг тревожности", "Шкала тревожности (GAD-2)", 0 },
                    { 1003, null, "Краткая шкала восприятия стресса", "Шкала стресса (PSS-4)", 0 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "TestId", "Text", "Type" },
                values: new object[,]
                {
                    { 2001, 1001, "За последние 2 недели: испытывали ли вы слабый интерес или удовольствие от занятий?", 0 },
                    { 2002, 1001, "За последние 2 недели: чувствовали ли вы себя подавленным/унылым?", 0 },
                    { 2101, 1002, "За последние 2 недели: сколько вы беспокоились и тревожились?", 0 },
                    { 2102, 1002, "За последние 2 недели: было ли вам трудно перестать беспокоиться или контролировать беспокойство?", 0 },
                    { 2201, 1003, "На прошлой неделе вы чувствовали, что не в состоянии контролировать важные вещи в жизни?", 0 },
                    { 2202, 1003, "На прошлой неделе вы чувствовали напряжение и нервозность?", 0 },
                    { 2203, 1003, "На прошлой неделе вы чувствовали, что справляетесь с личными проблемами?", 0 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Order", "QuestionId", "Text", "Value" },
                values: new object[,]
                {
                    { 3001, 0, 2001, "Никогда", 0 },
                    { 3002, 1, 2001, "Несколько дней", 1 },
                    { 3003, 2, 2001, "Более половины дней", 2 },
                    { 3004, 3, 2001, "Почти каждый день", 3 },
                    { 3011, 0, 2002, "Никогда", 0 },
                    { 3012, 1, 2002, "Несколько дней", 1 },
                    { 3013, 2, 2002, "Более половины дней", 2 },
                    { 3014, 3, 2002, "Почти каждый день", 3 },
                    { 3101, 0, 2101, "Никогда", 0 },
                    { 3102, 1, 2101, "Несколько дней", 1 },
                    { 3103, 2, 2101, "Более половины дней", 2 },
                    { 3104, 3, 2101, "Почти каждый день", 3 },
                    { 3111, 0, 2102, "Никогда", 0 },
                    { 3112, 1, 2102, "Несколько дней", 1 },
                    { 3113, 2, 2102, "Более половины дней", 2 },
                    { 3114, 3, 2102, "Почти каждый день", 3 },
                    { 3201, 0, 2201, "Никогда", 0 },
                    { 3202, 1, 2201, "Почти никогда", 1 },
                    { 3203, 2, 2201, "Иногда", 2 },
                    { 3204, 3, 2201, "Часто", 3 },
                    { 3205, 4, 2201, "Очень часто", 4 },
                    { 3211, 0, 2202, "Никогда", 0 },
                    { 3212, 1, 2202, "Почти никогда", 1 },
                    { 3213, 2, 2202, "Иногда", 2 },
                    { 3214, 3, 2202, "Часто", 3 },
                    { 3215, 4, 2202, "Очень часто", 4 },
                    { 3221, 0, 2203, "Никогда", 0 },
                    { 3222, 1, 2203, "Почти никогда", 1 },
                    { 3223, 2, 2203, "Иногда", 2 },
                    { 3224, 3, 2203, "Часто", 3 },
                    { 3225, 4, 2203, "Очень часто", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3011);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3012);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3013);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3014);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3101);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3102);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3103);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3104);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3111);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3112);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3113);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3114);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3201);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3202);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3203);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3204);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3205);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3211);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3212);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3213);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3214);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3215);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3221);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3222);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3223);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3224);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3225);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2101);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2102);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2201);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2202);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2203);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1003);
        }
    }
}
