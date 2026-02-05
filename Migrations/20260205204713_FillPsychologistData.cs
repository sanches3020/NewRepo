using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sofia.Web.Migrations
{
    /// <inheritdoc />
    public partial class FillPsychologistData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Psychologists",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContactEmail", "ContactPhone", "Description", "Education", "Experience", "Languages", "Methods", "PhotoUrl", "PricePerHour", "Specialization" },
                values: new object[] { "irina.smirnova@sofia.com", "+375291234567", "Специалист в лечении тревожных расстройств и депрессии с 12-летним опытом работы", "Московский государственный университет им. М.В. Ломоносова, факультет психологии", "12 лет в клинической психологии", "Русский, Английский", "КПТ, Экспозиционная терапия, Техники релаксации", "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200&h=200&fit=crop", 60m, "Когнитивно-поведенческая терапия" });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContactEmail", "ContactPhone", "Description", "Education", "Experience", "Languages", "Methods", "PhotoUrl", "PricePerHour", "Specialization" },
                values: new object[] { "alexey.ivanov@sofia.com", "+375291234568", "Помогаю парам и семьям улучшить отношения и преодолеть конфликты", "Институт психологии АН России, специализация в семейной системной терапии", "9 лет опыта в семейном консультировании", "Русский, Немецкий", "Системная терапия, Эмоционально-фокусированная терапия, Коммуникативные техники", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200&h=200&fit=crop", 55m, "Семейная психология и консультирование" });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContactEmail", "ContactPhone", "Description", "Education", "Experience", "Languages", "Methods", "PhotoUrl", "PricePerHour", "Specialization" },
                values: new object[] { "maria.koval@sofia.com", "+375291234569", "Помогаю клиентам развить стрессоустойчивость и найти внутренний баланс", "Санкт-Петербургский государственный университет, кафедра психологии развития", "7 лет в области позитивной психологии и mindfulness", "Русский, Французский, Английский", "Mindfulness, Медитация, Позитивная психология, Дыхательные практики", "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=200&h=200&fit=crop", 50m, "Психология здоровья и благополучия" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Psychologists");

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContactEmail", "ContactPhone", "Description", "Education", "Experience", "Languages", "Methods", "PricePerHour", "Specialization" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContactEmail", "ContactPhone", "Description", "Education", "Experience", "Languages", "Methods", "PricePerHour", "Specialization" },
                values: new object[] { null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Psychologists",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContactEmail", "ContactPhone", "Description", "Education", "Experience", "Languages", "Methods", "PricePerHour", "Specialization" },
                values: new object[] { null, null, null, null, null, null, null, null, null });
        }
    }
}
