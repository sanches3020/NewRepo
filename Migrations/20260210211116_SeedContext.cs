using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofia.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContactEmail", "Description", "Education", "Methods", "Name", "Specialization" },
                values: new object[] { "irina.smirnova@user.com", "Работаю с семейными конфликтами, кризисами, проблемами в отношениях с 12-летним опытом работы", "Белорусский государственный университет (БГУ), : психолог, системный семейный терапевт", "Системная семейная терапия, схема‑терапия", "Майорова Ольга", "Семейная терапия, системная терапия" });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Education", "Languages", "Methods", "Name", "Specialization" },
                values: new object[] { "Индивидуальная долгосрочная терапия, работа с тревогой, самооценкой, внутренними конфликтами", "Психолог, психоаналитическая подготовка", "Русский, Английский, Французский", " Психоанализ, психодинамическая терапия", "Красильников Семён", " Психоаналитическая терапия" });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Education", "Name", "Specialization" },
                values: new object[] { " Аналитическая терапия", "Белорусский государственный педагогический университет имени Максима Танка (БГПУ), Минск", "Алена Церашчук", " Юнгианский анализ" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContactEmail", "Description", "Education", "Methods", "Name", "Specialization" },
                values: new object[] { "irina.smirnova@sofia.com", "Специалист в лечении тревожных расстройств и депрессии с 12-летним опытом работы", "Московский государственный университет им. М.В. Ломоносова, факультет психологии", "КПТ, Экспозиционная терапия, Техники релаксации", "Ирина Смирнова", "Когнитивно-поведенческая терапия" });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Education", "Languages", "Methods", "Name", "Specialization" },
                values: new object[] { "Помогаю парам и семьям улучшить отношения и преодолеть конфликты", "Институт психологии АН России, специализация в семейной системной терапии", "Русский, Немецкий", "Системная терапия, Эмоционально-фокусированная терапия, Коммуникативные техники", "Алексей Иванов", "Семейная психология и консультирование" });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Education", "Name", "Specialization" },
                values: new object[] { "Помогаю клиентам развить стрессоустойчивость и найти внутренний баланс", "Санкт-Петербургский государственный университет, кафедра психологии развития", "Мария Коваль", "Психология здоровья и благополучия" });
        }
    }
}
