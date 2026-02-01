using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sofia.Web.Migrations
{
    /// <inheritdoc />
    public partial class ExpandTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "TestId", "Text", "Type" },
                values: new object[,]
                {
                    { 2401, 1001, "За последние 2 недели: вы теряли интерес или удовольствие от занятий, которые раньше нравились?", 0 },
                    { 2402, 1001, "За последние 2 недели: вы чувствовали грусть, подавленность или безнадежность?", 0 },
                    { 2403, 1001, "За последние 2 недели: у вас были проблемы со сном (трудности с засыпанием или пробуждение ранним утром)?", 0 },
                    { 2404, 1001, "За последние 2 недели: вы чувствовали усталость или потерю энергии?", 0 },
                    { 2405, 1001, "За последние 2 недели: у вас снизилась способность концентрироваться на задачах?", 0 },
                    { 2406, 1001, "За последние 2 недели: вы испытывали чувство собственной никчемности или чрезмерной вины?", 0 },
                    { 2407, 1001, "За последние 2 недели: вы заметили замедленность движений или, наоборот, беспокойство и суетливость?", 0 },
                    { 2408, 1001, "За последние 2 недели: у вас появлялись мысли о том, что вы лучше бы умерли, или навязчивые мысли о смерти?", 0 },
                    { 2409, 1001, "За последние 2 недели: вы замечали изменения в аппетите (уменьшение или увеличение)?", 0 },
                    { 2410, 1001, "За последние 2 недели: вы испытывали затруднения в принятии решений или планировании?", 0 },
                    { 2411, 1002, "За последние 2 недели: вы часто испытывали беспокойство или тревогу без видимой причины?", 0 },
                    { 2412, 1002, "За последние 2 недели: вам было трудно контролировать своё беспокойство?", 0 },
                    { 2413, 1002, "За последние 2 недели: вы чувствовали напряжение или мышечное напряжение?", 0 },
                    { 2414, 1002, "За последние 2 недели: вы легко уставали или испытывали слабость?", 0 },
                    { 2415, 1002, "За последние 2 недели: вы испытывали затруднения с концентрацией внимания?", 0 },
                    { 2416, 1002, "За последние 2 недели: у вас были проблемы со сном из‑за тревоги?", 0 },
                    { 2417, 1002, "За последние 2 недели: вы чувствовали себя раздражительным(ой) или нервным(ой)?", 0 },
                    { 2418, 1002, "За последние 2 недели: вы старались избегать ситуаций, которые вызывают тревогу?", 0 },
                    { 2419, 1002, "За последние 2 недели: тревога мешала вашей работе, учебе или общению?", 0 },
                    { 2420, 1002, "За последние 2 недели: вы испытывали панические приступы или приступы сильного страха?", 0 },
                    { 2421, 1003, "За последний месяц: вы чувствовали, что не можете контролировать важные вещи в жизни?", 0 },
                    { 2422, 1003, "За последний месяц: вы чувствовали себя нервным(ой) и 'на нервах'", 0 },
                    { 2423, 1003, "За последний месяц: вы чувствовали, что справляетесь с личными проблемами?", 0 },
                    { 2424, 1003, "За последний месяц: вы сталкивались с ситуациями, которые вызывали у вас ощущение перегруженности?", 0 },
                    { 2425, 1003, "За последний месяц: вы находили достаточно времени для отдыха и восстановления?", 0 },
                    { 2426, 1003, "За последний месяц: вы чувствовали, что требования со стороны работы/учебы/домашних слишком велики?", 0 },
                    { 2427, 1003, "За последний месяц: вы замечали, что негативные события занимают ваши мысли дольше, чем хотелось бы?", 0 },
                    { 2428, 1003, "За последний месяц: вы справлялись с неожиданными трудностями?", 0 },
                    { 2429, 1003, "За последний месяц: вы чувствовали себя уверенно в своих решениях?", 0 },
                    { 2430, 1003, "За последний месяц: вы чувствовали, что у вас есть ресурсы (временные, эмоциональные, социальные) для решения проблем?", 0 }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "CreatedByPsychologistId", "Description", "Name", "Type" },
                values: new object[,]
                {
                    { 1004, null, "Краткая шкала устойчивости к стрессу", "Шкала устойчивости (RS-10)", 0 },
                    { 1005, null, "Вопросы о качестве сна", "Краткая шкала сна (Sleep-10)", 0 },
                    { 1006, null, "Оценка навыков регуляции эмоций", "Шкала эмоциональной регуляции", 0 },
                    { 1007, null, "Оценка уровня социальной поддержки", "Шкала социальной поддержки", 0 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Order", "QuestionId", "Text", "Value" },
                values: new object[,]
                {
                    { 4001, 0, 2401, "Никогда", 0 },
                    { 4002, 1, 2401, "Несколько дней", 1 },
                    { 4003, 2, 2401, "Более половины дней", 2 },
                    { 4004, 3, 2401, "Почти каждый день", 3 },
                    { 4005, 0, 2402, "Никогда", 0 },
                    { 4006, 1, 2402, "Несколько дней", 1 },
                    { 4007, 2, 2402, "Более половины дней", 2 },
                    { 4008, 3, 2402, "Почти каждый день", 3 },
                    { 4009, 0, 2403, "Никогда", 0 },
                    { 4010, 1, 2403, "Несколько дней", 1 },
                    { 4011, 2, 2403, "Более половины дней", 2 },
                    { 4012, 3, 2403, "Почти каждый день", 3 },
                    { 4013, 0, 2404, "Никогда", 0 },
                    { 4014, 1, 2404, "Несколько дней", 1 },
                    { 4015, 2, 2404, "Более половины дней", 2 },
                    { 4016, 3, 2404, "Почти каждый день", 3 },
                    { 4017, 0, 2405, "Никогда", 0 },
                    { 4018, 1, 2405, "Несколько дней", 1 },
                    { 4019, 2, 2405, "Более половины дней", 2 },
                    { 4020, 3, 2405, "Почти каждый день", 3 },
                    { 4021, 0, 2406, "Никогда", 0 },
                    { 4022, 1, 2406, "Несколько дней", 1 },
                    { 4023, 2, 2406, "Более половины дней", 2 },
                    { 4024, 3, 2406, "Почти каждый день", 3 },
                    { 4025, 0, 2407, "Никогда", 0 },
                    { 4026, 1, 2407, "Несколько дней", 1 },
                    { 4027, 2, 2407, "Более половины дней", 2 },
                    { 4028, 3, 2407, "Почти каждый день", 3 },
                    { 4029, 0, 2408, "Никогда", 0 },
                    { 4030, 1, 2408, "Несколько дней", 1 },
                    { 4031, 2, 2408, "Более половины дней", 2 },
                    { 4032, 3, 2408, "Почти каждый день", 3 },
                    { 4033, 0, 2409, "Никогда", 0 },
                    { 4034, 1, 2409, "Несколько дней", 1 },
                    { 4035, 2, 2409, "Более половины дней", 2 },
                    { 4036, 3, 2409, "Почти каждый день", 3 },
                    { 4037, 0, 2410, "Никогда", 0 },
                    { 4038, 1, 2410, "Несколько дней", 1 },
                    { 4039, 2, 2410, "Более половины дней", 2 },
                    { 4040, 3, 2410, "Почти каждый день", 3 },
                    { 4041, 0, 2411, "Никогда", 0 },
                    { 4042, 1, 2411, "Несколько дней", 1 },
                    { 4043, 2, 2411, "Более половины дней", 2 },
                    { 4044, 3, 2411, "Почти каждый день", 3 },
                    { 4045, 0, 2412, "Никогда", 0 },
                    { 4046, 1, 2412, "Несколько дней", 1 },
                    { 4047, 2, 2412, "Более половины дней", 2 },
                    { 4048, 3, 2412, "Почти каждый день", 3 },
                    { 4049, 0, 2413, "Никогда", 0 },
                    { 4050, 1, 2413, "Несколько дней", 1 },
                    { 4051, 2, 2413, "Более половины дней", 2 },
                    { 4052, 3, 2413, "Почти каждый день", 3 },
                    { 4053, 0, 2414, "Никогда", 0 },
                    { 4054, 1, 2414, "Несколько дней", 1 },
                    { 4055, 2, 2414, "Более половины дней", 2 },
                    { 4056, 3, 2414, "Почти каждый день", 3 },
                    { 4057, 0, 2415, "Никогда", 0 },
                    { 4058, 1, 2415, "Несколько дней", 1 },
                    { 4059, 2, 2415, "Более половины дней", 2 },
                    { 4060, 3, 2415, "Почти каждый день", 3 },
                    { 4061, 0, 2416, "Никогда", 0 },
                    { 4062, 1, 2416, "Несколько дней", 1 },
                    { 4063, 2, 2416, "Более половины дней", 2 },
                    { 4064, 3, 2416, "Почти каждый день", 3 },
                    { 4065, 0, 2417, "Никогда", 0 },
                    { 4066, 1, 2417, "Несколько дней", 1 },
                    { 4067, 2, 2417, "Более половины дней", 2 },
                    { 4068, 3, 2417, "Почти каждый день", 3 },
                    { 4069, 0, 2418, "Никогда", 0 },
                    { 4070, 1, 2418, "Несколько дней", 1 },
                    { 4071, 2, 2418, "Более половины дней", 2 },
                    { 4072, 3, 2418, "Почти каждый день", 3 },
                    { 4073, 0, 2419, "Никогда", 0 },
                    { 4074, 1, 2419, "Несколько дней", 1 },
                    { 4075, 2, 2419, "Более половины дней", 2 },
                    { 4076, 3, 2419, "Почти каждый день", 3 },
                    { 4077, 0, 2420, "Никогда", 0 },
                    { 4078, 1, 2420, "Несколько дней", 1 },
                    { 4079, 2, 2420, "Более половины дней", 2 },
                    { 4080, 3, 2420, "Почти каждый день", 3 },
                    { 5001, 0, 2421, "Никогда", 0 },
                    { 5002, 1, 2421, "Почти никогда", 1 },
                    { 5003, 2, 2421, "Иногда", 2 },
                    { 5004, 3, 2421, "Часто", 3 },
                    { 5005, 4, 2421, "Очень часто", 4 },
                    { 5006, 0, 2422, "Никогда", 0 },
                    { 5007, 1, 2422, "Почти никогда", 1 },
                    { 5008, 2, 2422, "Иногда", 2 },
                    { 5009, 3, 2422, "Часто", 3 },
                    { 5010, 4, 2422, "Очень часто", 4 },
                    { 5011, 0, 2423, "Никогда", 0 },
                    { 5012, 1, 2423, "Почти никогда", 1 },
                    { 5013, 2, 2423, "Иногда", 2 },
                    { 5014, 3, 2423, "Часто", 3 },
                    { 5015, 4, 2423, "Очень часто", 4 },
                    { 5016, 0, 2424, "Никогда", 0 },
                    { 5017, 1, 2424, "Почти никогда", 1 },
                    { 5018, 2, 2424, "Иногда", 2 },
                    { 5019, 3, 2424, "Часто", 3 },
                    { 5020, 4, 2424, "Очень часто", 4 },
                    { 5021, 0, 2425, "Никогда", 0 },
                    { 5022, 1, 2425, "Почти никогда", 1 },
                    { 5023, 2, 2425, "Иногда", 2 },
                    { 5024, 3, 2425, "Часто", 3 },
                    { 5025, 4, 2425, "Очень часто", 4 },
                    { 5026, 0, 2426, "Никогда", 0 },
                    { 5027, 1, 2426, "Почти никогда", 1 },
                    { 5028, 2, 2426, "Иногда", 2 },
                    { 5029, 3, 2426, "Часто", 3 },
                    { 5030, 4, 2426, "Очень часто", 4 },
                    { 5031, 0, 2427, "Никогда", 0 },
                    { 5032, 1, 2427, "Почти никогда", 1 },
                    { 5033, 2, 2427, "Иногда", 2 },
                    { 5034, 3, 2427, "Часто", 3 },
                    { 5035, 4, 2427, "Очень часто", 4 },
                    { 5036, 0, 2428, "Никогда", 0 },
                    { 5037, 1, 2428, "Почти никогда", 1 },
                    { 5038, 2, 2428, "Иногда", 2 },
                    { 5039, 3, 2428, "Часто", 3 },
                    { 5040, 4, 2428, "Очень часто", 4 },
                    { 5041, 0, 2429, "Никогда", 0 },
                    { 5042, 1, 2429, "Почти никогда", 1 },
                    { 5043, 2, 2429, "Иногда", 2 },
                    { 5044, 3, 2429, "Часто", 3 },
                    { 5045, 4, 2429, "Очень часто", 4 },
                    { 5046, 0, 2430, "Никогда", 0 },
                    { 5047, 1, 2430, "Почти никогда", 1 },
                    { 5048, 2, 2430, "Иногда", 2 },
                    { 5049, 3, 2430, "Часто", 3 },
                    { 5050, 4, 2430, "Очень часто", 4 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "TestId", "Text", "Type" },
                values: new object[,]
                {
                    { 2301, 1004, "Я легко приспосабливаюсь к изменениям.", 0 },
                    { 2302, 1004, "Я могу справляться с трудностями.", 0 },
                    { 2303, 1004, "У меня есть внутренняя сила, чтобы преодолевать проблемы.", 0 },
                    { 2304, 1004, "Я быстро восстанавливаюсь после неудач.", 0 },
                    { 2305, 1004, "Мне удаётся сохранять ясность мышления в стрессовых ситуациях.", 0 },
                    { 2306, 1004, "Я могу найти выход даже из сложной ситуации.", 0 },
                    { 2307, 1004, "Я уверен(а) в своей способности контролировать жизнь.", 0 },
                    { 2308, 1004, "Я нахожу новые способы решения проблем.", 0 },
                    { 2309, 1004, "Я сохраняю чувство юмора в трудные моменты.", 0 },
                    { 2310, 1004, "Я могу опираться на свои ресурсы при стрессе.", 0 },
                    { 2321, 1005, "Вы засыпаете в течение 30 минут или меньше?", 0 },
                    { 2322, 1005, "Вы часто просыпаетесь ночью?", 0 },
                    { 2323, 1005, "Вы чувствуете, что сон восстанавливает силы?", 0 },
                    { 2324, 1005, "Вы просыпаетесь рано и не можете снова заснуть?", 0 },
                    { 2325, 1005, "Вы часто испытываете дневную сонливость?", 0 },
                    { 2326, 1005, "Вы часто используете снотворные или алкоголь, чтобы заснуть?", 0 },
                    { 2327, 1005, "Вы доволены качеством своего сна?", 0 },
                    { 2328, 1005, "Ваш сон влияет на выполнение рабочих/учебных задач?", 0 },
                    { 2329, 1005, "Вы просыпаетесь от громких звуков или движений?", 0 },
                    { 2330, 1005, "Вы удовлетворены продолжительностью своего сна?", 0 },
                    { 2341, 1006, "Я контролирую свои эмоциональные реакции.", 0 },
                    { 2342, 1006, "Я использую техники для успокоения, когда расстроен(а).", 0 },
                    { 2343, 1006, "Мне удаётся переосмыслить негативные события.", 0 },
                    { 2344, 1006, "Я замечаю ранние признаки эмоционального напряжения.", 0 },
                    { 2345, 1006, "Я умею выражать эмоции конструктивно.", 0 },
                    { 2346, 1006, "Я умею отвлекаться от навязчивых мыслей.", 0 },
                    { 2347, 1006, "Я использую дыхательные или релаксационные практики при стрессе.", 0 },
                    { 2348, 1006, "Я могу просить помощи у других при эмоциональной перегрузке.", 0 },
                    { 2349, 1006, "Я способен(на) восстанавливаться после эмоциональных срывов.", 0 },
                    { 2350, 1006, "Я чувствую, что управляю своими эмоциями в повседневной жизни.", 0 },
                    { 2361, 1007, "У меня есть люди, к которым можно обратиться за помощью.", 0 },
                    { 2362, 1007, "Я чувствую эмоциональную поддержку со стороны близких.", 0 },
                    { 2363, 1007, "Мне легко просить помощи, когда это нужно.", 0 },
                    { 2364, 1007, "У меня есть друзья/коллеги, с которыми приятно общаться.", 0 },
                    { 2365, 1007, "Я чувствую, что меня понимают и принимают.", 0 },
                    { 2366, 1007, "У меня есть человек, с кем можно обсудить важные дела.", 0 },
                    { 2367, 1007, "Я могу рассчитывать на помощь в критической ситуации.", 0 },
                    { 2368, 1007, "У меня есть доверенные люди, с которыми можно поделиться переживаниями.", 0 },
                    { 2369, 1007, "Я удовлетворён(на) уровнем своей социальной поддержки.", 0 },
                    { 2370, 1007, "Я чувствую, что не остаюсь один(одна) с проблемами.", 0 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Order", "QuestionId", "Text", "Value" },
                values: new object[,]
                {
                    { 3301, 0, 2301, "Никогда", 0 },
                    { 3302, 1, 2301, "Иногда", 1 },
                    { 3303, 2, 2301, "Часто", 2 },
                    { 3304, 3, 2301, "Всегда", 3 },
                    { 3305, 0, 2302, "Никогда", 0 },
                    { 3306, 1, 2302, "Иногда", 1 },
                    { 3307, 2, 2302, "Часто", 2 },
                    { 3308, 3, 2302, "Всегда", 3 },
                    { 3309, 0, 2303, "Никогда", 0 },
                    { 3310, 1, 2303, "Иногда", 1 },
                    { 3311, 2, 2303, "Часто", 2 },
                    { 3312, 3, 2303, "Всегда", 3 },
                    { 3313, 0, 2304, "Никогда", 0 },
                    { 3314, 1, 2304, "Иногда", 1 },
                    { 3315, 2, 2304, "Часто", 2 },
                    { 3316, 3, 2304, "Всегда", 3 },
                    { 3317, 0, 2305, "Никогда", 0 },
                    { 3318, 1, 2305, "Иногда", 1 },
                    { 3319, 2, 2305, "Часто", 2 },
                    { 3320, 3, 2305, "Всегда", 3 },
                    { 3321, 0, 2306, "Никогда", 0 },
                    { 3322, 1, 2306, "Иногда", 1 },
                    { 3323, 2, 2306, "Часто", 2 },
                    { 3324, 3, 2306, "Всегда", 3 },
                    { 3325, 0, 2307, "Никогда", 0 },
                    { 3326, 1, 2307, "Иногда", 1 },
                    { 3327, 2, 2307, "Часто", 2 },
                    { 3328, 3, 2307, "Всегда", 3 },
                    { 3329, 0, 2308, "Никогда", 0 },
                    { 3330, 1, 2308, "Иногда", 1 },
                    { 3331, 2, 2308, "Часто", 2 },
                    { 3332, 3, 2308, "Всегда", 3 },
                    { 3333, 0, 2309, "Никогда", 0 },
                    { 3334, 1, 2309, "Иногда", 1 },
                    { 3335, 2, 2309, "Часто", 2 },
                    { 3336, 3, 2309, "Всегда", 3 },
                    { 3337, 0, 2310, "Никогда", 0 },
                    { 3338, 1, 2310, "Иногда", 1 },
                    { 3339, 2, 2310, "Часто", 2 },
                    { 3340, 3, 2310, "Всегда", 3 },
                    { 3341, 0, 2321, "Никогда", 0 },
                    { 3342, 1, 2321, "Иногда", 1 },
                    { 3343, 2, 2321, "Часто", 2 },
                    { 3344, 3, 2321, "Всегда", 3 },
                    { 3345, 0, 2322, "Никогда", 0 },
                    { 3346, 1, 2322, "Иногда", 1 },
                    { 3347, 2, 2322, "Часто", 2 },
                    { 3348, 3, 2322, "Всегда", 3 },
                    { 3349, 0, 2323, "Никогда", 0 },
                    { 3350, 1, 2323, "Иногда", 1 },
                    { 3351, 2, 2323, "Часто", 2 },
                    { 3352, 3, 2323, "Всегда", 3 },
                    { 3353, 0, 2324, "Никогда", 0 },
                    { 3354, 1, 2324, "Иногда", 1 },
                    { 3355, 2, 2324, "Часто", 2 },
                    { 3356, 3, 2324, "Всегда", 3 },
                    { 3357, 0, 2325, "Никогда", 0 },
                    { 3358, 1, 2325, "Иногда", 1 },
                    { 3359, 2, 2325, "Часто", 2 },
                    { 3360, 3, 2325, "Всегда", 3 },
                    { 3361, 0, 2326, "Никогда", 0 },
                    { 3362, 1, 2326, "Иногда", 1 },
                    { 3363, 2, 2326, "Часто", 2 },
                    { 3364, 3, 2326, "Всегда", 3 },
                    { 3365, 0, 2327, "Никогда", 0 },
                    { 3366, 1, 2327, "Иногда", 1 },
                    { 3367, 2, 2327, "Часто", 2 },
                    { 3368, 3, 2327, "Всегда", 3 },
                    { 3369, 0, 2328, "Никогда", 0 },
                    { 3370, 1, 2328, "Иногда", 1 },
                    { 3371, 2, 2328, "Часто", 2 },
                    { 3372, 3, 2328, "Всегда", 3 },
                    { 3373, 0, 2329, "Никогда", 0 },
                    { 3374, 1, 2329, "Иногда", 1 },
                    { 3375, 2, 2329, "Часто", 2 },
                    { 3376, 3, 2329, "Всегда", 3 },
                    { 3377, 0, 2330, "Никогда", 0 },
                    { 3378, 1, 2330, "Иногда", 1 },
                    { 3379, 2, 2330, "Часто", 2 },
                    { 3380, 3, 2330, "Всегда", 3 },
                    { 3381, 0, 2341, "Никогда", 0 },
                    { 3382, 1, 2341, "Иногда", 1 },
                    { 3383, 2, 2341, "Часто", 2 },
                    { 3384, 3, 2341, "Всегда", 3 },
                    { 3385, 0, 2342, "Никогда", 0 },
                    { 3386, 1, 2342, "Иногда", 1 },
                    { 3387, 2, 2342, "Часто", 2 },
                    { 3388, 3, 2342, "Всегда", 3 },
                    { 3389, 0, 2343, "Никогда", 0 },
                    { 3390, 1, 2343, "Иногда", 1 },
                    { 3391, 2, 2343, "Часто", 2 },
                    { 3392, 3, 2343, "Всегда", 3 },
                    { 3393, 0, 2344, "Никогда", 0 },
                    { 3394, 1, 2344, "Иногда", 1 },
                    { 3395, 2, 2344, "Часто", 2 },
                    { 3396, 3, 2344, "Всегда", 3 },
                    { 3397, 0, 2345, "Никогда", 0 },
                    { 3398, 1, 2345, "Иногда", 1 },
                    { 3399, 2, 2345, "Часто", 2 },
                    { 3400, 3, 2345, "Всегда", 3 },
                    { 3401, 0, 2346, "Никогда", 0 },
                    { 3402, 1, 2346, "Иногда", 1 },
                    { 3403, 2, 2346, "Часто", 2 },
                    { 3404, 3, 2346, "Всегда", 3 },
                    { 3405, 0, 2347, "Никогда", 0 },
                    { 3406, 1, 2347, "Иногда", 1 },
                    { 3407, 2, 2347, "Часто", 2 },
                    { 3408, 3, 2347, "Всегда", 3 },
                    { 3409, 0, 2348, "Никогда", 0 },
                    { 3410, 1, 2348, "Иногда", 1 },
                    { 3411, 2, 2348, "Часто", 2 },
                    { 3412, 3, 2348, "Всегда", 3 },
                    { 3413, 0, 2349, "Никогда", 0 },
                    { 3414, 1, 2349, "Иногда", 1 },
                    { 3415, 2, 2349, "Часто", 2 },
                    { 3416, 3, 2349, "Всегда", 3 },
                    { 3417, 0, 2350, "Никогда", 0 },
                    { 3418, 1, 2350, "Иногда", 1 },
                    { 3419, 2, 2350, "Часто", 2 },
                    { 3420, 3, 2350, "Всегда", 3 },
                    { 3421, 0, 2361, "Никогда", 0 },
                    { 3422, 1, 2361, "Иногда", 1 },
                    { 3423, 2, 2361, "Часто", 2 },
                    { 3424, 3, 2361, "Всегда", 3 },
                    { 3425, 0, 2362, "Никогда", 0 },
                    { 3426, 1, 2362, "Иногда", 1 },
                    { 3427, 2, 2362, "Часто", 2 },
                    { 3428, 3, 2362, "Всегда", 3 },
                    { 3429, 0, 2363, "Никогда", 0 },
                    { 3430, 1, 2363, "Иногда", 1 },
                    { 3431, 2, 2363, "Часто", 2 },
                    { 3432, 3, 2363, "Всегда", 3 },
                    { 3433, 0, 2364, "Никогда", 0 },
                    { 3434, 1, 2364, "Иногда", 1 },
                    { 3435, 2, 2364, "Часто", 2 },
                    { 3436, 3, 2364, "Всегда", 3 },
                    { 3437, 0, 2365, "Никогда", 0 },
                    { 3438, 1, 2365, "Иногда", 1 },
                    { 3439, 2, 2365, "Часто", 2 },
                    { 3440, 3, 2365, "Всегда", 3 },
                    { 3441, 0, 2366, "Никогда", 0 },
                    { 3442, 1, 2366, "Иногда", 1 },
                    { 3443, 2, 2366, "Часто", 2 },
                    { 3444, 3, 2366, "Всегда", 3 },
                    { 3445, 0, 2367, "Никогда", 0 },
                    { 3446, 1, 2367, "Иногда", 1 },
                    { 3447, 2, 2367, "Часто", 2 },
                    { 3448, 3, 2367, "Всегда", 3 },
                    { 3449, 0, 2368, "Никогда", 0 },
                    { 3450, 1, 2368, "Иногда", 1 },
                    { 3451, 2, 2368, "Часто", 2 },
                    { 3452, 3, 2368, "Всегда", 3 },
                    { 3453, 0, 2369, "Никогда", 0 },
                    { 3454, 1, 2369, "Иногда", 1 },
                    { 3455, 2, 2369, "Часто", 2 },
                    { 3456, 3, 2369, "Всегда", 3 },
                    { 3457, 0, 2370, "Никогда", 0 },
                    { 3458, 1, 2370, "Иногда", 1 },
                    { 3459, 2, 2370, "Часто", 2 },
                    { 3460, 3, 2370, "Всегда", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3301);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3302);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3303);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3304);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3305);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3306);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3307);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3308);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3309);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3310);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3311);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3312);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3313);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3314);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3315);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3316);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3317);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3318);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3319);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3320);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3321);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3322);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3323);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3324);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3325);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3326);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3327);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3328);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3329);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3330);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3331);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3332);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3333);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3334);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3335);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3336);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3337);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3338);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3339);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3340);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3341);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3342);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3343);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3344);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3345);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3346);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3347);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3348);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3349);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3350);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3351);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3352);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3353);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3354);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3355);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3356);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3357);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3358);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3359);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3360);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3361);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3362);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3363);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3364);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3365);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3366);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3367);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3368);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3369);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3370);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3371);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3372);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3373);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3374);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3375);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3376);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3377);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3378);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3379);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3380);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3381);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3382);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3383);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3384);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3385);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3386);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3387);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3388);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3389);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3390);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3391);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3392);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3393);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3394);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3395);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3396);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3397);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3398);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3399);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3400);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3401);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3402);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3403);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3404);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3405);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3406);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3407);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3408);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3409);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3410);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3411);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3412);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3413);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3414);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3415);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3416);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3417);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3418);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3419);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3420);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3421);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3422);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3423);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3424);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3425);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3426);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3427);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3428);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3429);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3430);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3431);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3432);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3433);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3434);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3435);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3436);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3437);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3438);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3439);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3440);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3441);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3442);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3443);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3444);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3445);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3446);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3447);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3448);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3449);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3450);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3451);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3452);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3453);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3454);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3455);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3456);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3457);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3458);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3459);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3460);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4006);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4007);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4008);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4009);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4010);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4011);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4012);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4013);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4014);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4015);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4016);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4017);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4018);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4019);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4020);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4021);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4022);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4023);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4024);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4025);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4026);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4027);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4028);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4029);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4030);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4031);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4032);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4033);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4034);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4035);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4036);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4037);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4038);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4039);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4040);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4041);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4042);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4043);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4044);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4045);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4046);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4047);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4048);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4049);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4050);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4051);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4052);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4053);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4054);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4055);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4056);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4057);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4058);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4059);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4060);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4061);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4062);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4063);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4064);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4065);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4066);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4067);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4068);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4069);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4070);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4071);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4072);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4073);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4074);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4075);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4076);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4077);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4078);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4079);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4080);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5005);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5006);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5007);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5008);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5009);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5010);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5011);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5012);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5013);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5014);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5015);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5016);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5017);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5018);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5019);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5020);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5021);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5022);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5023);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5024);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5025);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5026);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5027);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5028);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5029);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5030);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5031);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5032);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5033);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5034);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5035);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5036);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5037);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5038);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5039);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5040);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5041);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5042);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5043);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5044);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5045);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5046);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5047);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5048);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5049);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5050);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2301);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2302);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2303);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2304);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2305);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2306);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2307);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2308);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2309);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2310);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2321);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2322);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2323);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2324);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2325);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2326);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2327);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2328);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2329);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2330);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2341);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2342);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2343);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2344);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2345);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2346);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2347);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2348);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2349);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2350);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2361);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2362);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2363);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2364);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2365);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2366);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2367);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2368);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2369);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2370);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2401);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2402);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2403);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2404);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2405);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2406);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2407);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2408);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2409);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2410);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2411);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2412);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2413);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2414);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2415);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2416);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2417);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2418);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2419);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2420);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2421);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2422);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2423);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2424);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2425);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2426);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2427);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2428);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2429);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2430);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Tests",
                keyColumn: "Id",
                keyValue: 1007);

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
    }
}
