using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sofia.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActionUrl = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ActionText = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DailyReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    DailyReminderTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    GoalReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    MoodCheckReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    MoodCheckTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    WeeklyReport = table.Column<bool>(type: "INTEGER", nullable: false),
                    WeeklyReportDay = table.Column<int>(type: "INTEGER", nullable: false),
                    PracticeReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    PsychologistReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    PushNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Practices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PsychologistProfileId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmotionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Emotion = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmotionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmotionEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFromPsychologist = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    Emotion = table.Column<int>(type: "INTEGER", nullable: false),
                    Activity = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsPinned = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShareWithPsychologist = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Psychologists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Specialization = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Education = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Experience = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Languages = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Methods = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PricePerHour = table.Column<decimal>(type: "TEXT", nullable: true),
                    ContactPhone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ContactEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psychologists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Psychologists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PsychologistAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PsychologistId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsychologistAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsychologistAppointments_Psychologists_PsychologistId",
                        column: x => x.PsychologistId,
                        principalTable: "Psychologists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PsychologistAppointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsychologistReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PsychologistId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsychologistReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsychologistReviews_Psychologists_PsychologistId",
                        column: x => x.PsychologistId,
                        principalTable: "Psychologists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsychologistSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PsychologistId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsychologistSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsychologistSchedules_Psychologists_PsychologistId",
                        column: x => x.PsychologistId,
                        principalTable: "Psychologists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsychologistTimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PsychologistId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBooked = table.Column<bool>(type: "INTEGER", nullable: false),
                    BookedByUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsychologistTimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsychologistTimeSlots_Psychologists_PsychologistId",
                        column: x => x.PsychologistId,
                        principalTable: "Psychologists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PsychologistTimeSlots_Users_BookedByUserId",
                        column: x => x.BookedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Goals",
                columns: new[] { "Id", "CreatedAt", "Date", "Description", "IsFromPsychologist", "Progress", "Status", "TargetDate", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 29, 9, 9, 52, 384, DateTimeKind.Local).AddTicks(9534), new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local), "Выполнять хотя бы одну практику в день", false, 30, 1, null, "Ежедневные практики", "Wellness", null },
                    { 2, new DateTime(2025, 10, 29, 9, 9, 52, 386, DateTimeKind.Local).AddTicks(2405), new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local), "Записывать мысли и эмоции каждый день", false, 60, 1, null, "Ведение дневника", "Personal", null },
                    { 3, new DateTime(2025, 10, 29, 9, 9, 52, 386, DateTimeKind.Local).AddTicks(2408), new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Local), "Применять техники КПТ при тревоге", true, 25, 1, null, "Работа с тревогой", "Therapy", null }
                });

            migrationBuilder.InsertData(
                table: "Practices",
                columns: new[] { "Id", "Category", "Description", "DurationMinutes", "Instructions", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Breathing", "Техника успокоения через дыхание", 5, "Вдох на 4 счета, задержка на 7, выдох на 8", true, "Дыхание 4-7-8" },
                    { 2, "Relaxation", "Постепенное расслабление мышц", 15, "Напрягайте и расслабляйте каждую группу мышц", true, "Прогрессивная релаксация" },
                    { 3, "Visualization", "Создание мысленного убежища", 10, "Представьте место, где чувствуете себя в безопасности", true, "Визуализация безопасного места" },
                    { 4, "CBT", "Анализ и изменение негативных мыслей", 20, "Запишите мысль, оцените её реалистичность, найдите альтернативу", true, "КПТ: Работа с мыслями" },
                    { 5, "Mindfulness", "Фокус на настоящем моменте", 10, "Следите за дыханием, возвращайте внимание к настоящему", true, "Медитация осознанности" }
                });

            migrationBuilder.InsertData(
                table: "Psychologists",
                columns: new[] { "Id", "ContactEmail", "ContactPhone", "CreatedAt", "Description", "Education", "Experience", "IsActive", "Languages", "Methods", "Name", "PricePerHour", "Specialization", "UserId" },
                values: new object[,]
                {
                    { 1, "anna.petrova@psychology.ru", "+7 (495) 123-45-67", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Опытный психолог с 8-летним стажем", "МГУ", "8 лет", true, "Русский, английский", "КПТ, осознанность", "Анна Петрова", 3000m, "КПТ, тревожные расстройства", null },
                    { 2, "mikhail.sokolov@family-psych.ru", "+7 (812) 234-56-78", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "12 лет опыта", "СПбГУ", "12 лет", true, "Русский, французский", "Системный подход", "Михаил Соколов", 4000m, "Семейная терапия", null },
                    { 3, "elena.volkova@trauma-therapy.ru", "+7 (495) 345-67-89", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10 лет опыта", "МГПУ", "10 лет", true, "Русский, немецкий", "EMDR, соматика", "Елена Волкова", 5000m, "EMDR терапия", null }
                });

            migrationBuilder.InsertData(
                table: "PsychologistReviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "PsychologistId", "Rating" },
                values: new object[,]
                {
                    { 1, "Отличный специалист!", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5 },
                    { 2, "Профессиональный подход", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4 },
                    { 3, "Помог решить семейные проблемы", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5 },
                    { 4, "EMDR терапия действительно работает", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmotionEntries_UserId",
                table: "EmotionEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PsychologistAppointments_PsychologistId",
                table: "PsychologistAppointments",
                column: "PsychologistId");

            migrationBuilder.CreateIndex(
                name: "IX_PsychologistAppointments_UserId",
                table: "PsychologistAppointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PsychologistReviews_PsychologistId",
                table: "PsychologistReviews",
                column: "PsychologistId");

            migrationBuilder.CreateIndex(
                name: "IX_Psychologists_UserId",
                table: "Psychologists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsychologistSchedules_PsychologistId",
                table: "PsychologistSchedules",
                column: "PsychologistId");

            migrationBuilder.CreateIndex(
                name: "IX_PsychologistTimeSlots_BookedByUserId",
                table: "PsychologistTimeSlots",
                column: "BookedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PsychologistTimeSlots_PsychologistId",
                table: "PsychologistTimeSlots",
                column: "PsychologistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmotionEntries");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationSettings");

            migrationBuilder.DropTable(
                name: "Practices");

            migrationBuilder.DropTable(
                name: "PsychologistAppointments");

            migrationBuilder.DropTable(
                name: "PsychologistReviews");

            migrationBuilder.DropTable(
                name: "PsychologistSchedules");

            migrationBuilder.DropTable(
                name: "PsychologistTimeSlots");

            migrationBuilder.DropTable(
                name: "Psychologists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
