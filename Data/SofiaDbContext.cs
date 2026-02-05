using Microsoft.EntityFrameworkCore;
using Sofia.Web.Models;

namespace Sofia.Web.Data;

public class SofiaDbContext : DbContext
{
    public SofiaDbContext(DbContextOptions<SofiaDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Practice> Practices { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Psychologist> Psychologists { get; set; }
    public DbSet<PsychologistReview> PsychologistReviews { get; set; }
    public DbSet<PsychologistAppointment> PsychologistAppointments { get; set; }
    public DbSet<PsychologistSchedule> PsychologistSchedules { get; set; }
    public DbSet<PsychologistTimeSlot> PsychologistTimeSlots { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationSettings> NotificationSettings { get; set; }
    public DbSet<EmotionEntry> EmotionEntries { get; set; }
    public DbSet<UserStatistics> UserStatistics { get; set; }
    // Tests / assessments
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<TestResult> TestResults { get; set; }

    // Interpretation thresholds per test (percent-based)
    public DbSet<TestInterpretation> TestInterpretations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конвертация enum → string
        modelBuilder.Entity<Practice>()
            .Property(p => p.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Goal>()
            .Property(g => g.Type)
            .HasConversion<string>();

        // Конвертация EmotionType → integer (храним как int в БД)
        modelBuilder.Entity<Note>()
            .Property(n => n.Emotion)
            .HasConversion<int>();

        modelBuilder.Entity<EmotionEntry>()
            .Property(e => e.Emotion)
            .HasConversion<int>();

        // Связи
        modelBuilder.Entity<User>()
            .HasOne(u => u.PsychologistProfile)
            .WithOne(p => p.User)
            .HasForeignKey<Psychologist>(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Goal>()
            .HasOne(g => g.User)
            .WithMany(u => u.Goals)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistAppointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmotionEntry>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserStatistics>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistSchedule>()
            .HasOne(s => s.Psychologist)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistTimeSlot>()
            .HasOne(t => t.Psychologist)
            .WithMany(p => p.TimeSlots)
            .HasForeignKey(t => t.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistTimeSlot>()
            .HasOne(t => t.BookedByUser)
            .WithMany()
            .HasForeignKey(t => t.BookedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<PsychologistReview>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // --- SEEDING ---

        var createdDate = DateTime.SpecifyKind(DateTime.Parse("2024-01-01T00:00:00"), DateTimeKind.Utc);

        // Seed: Practices
        modelBuilder.Entity<Practice>().HasData(
            new Practice { Id = 1, Name = "Дыхание 4-7-8", Description = "Техника успокоения через дыхание", Category = PracticeCategory.Breathing, DurationMinutes = 5, Instructions = "Вдох на 4 счета, задержка на 7, выдох на 8", IsActive = true },
            new Practice { Id = 2, Name = "Прогрессивная релаксация", Description = "Постепенное расслабление мышц", Category = PracticeCategory.Relaxation, DurationMinutes = 15, Instructions = "Напрягайте и расслабляйте каждую группу мышц", IsActive = true },
            new Practice { Id = 3, Name = "Визуализация безопасного места", Description = "Создание мысленного убежища", Category = PracticeCategory.Visualization, DurationMinutes = 10, Instructions = "Представьте место, где чувствуете себя в безопасности", IsActive = true },
            new Practice { Id = 4, Name = "КПТ: Работа с мыслями", Description = "Анализ и изменение негативных мыслей", Category = PracticeCategory.CBT, DurationMinutes = 20, Instructions = "Запишите мысль, оцените её реалистичность, найдите альтернативу", IsActive = true },
            new Practice { Id = 5, Name = "Медитация осознанности", Description = "Фокус на настоящем моменте", Category = PracticeCategory.Mindfulness, DurationMinutes = 10, Instructions = "Следите за дыханием, возвращайте внимание к настоящему", IsActive = true }
        );

        // Seed: Goals
        modelBuilder.Entity<Goal>().HasData(
            new Goal { Id = 1, Title = "Ежедневные практики", Description = "Выполнять хотя бы одну практику в день", Type = GoalType.Wellness, Progress = 30 },
            new Goal { Id = 2, Title = "Ведение дневника", Description = "Записывать мысли и эмоции каждый день", Type = GoalType.Personal, Progress = 60 },
            new Goal { Id = 3, Title = "Работа с тревогой", Description = "Применять техники КПТ при тревоге", Type = GoalType.Therapy, IsFromPsychologist = true, Progress = 25 }
        );

        //  Seed: Psychologists
        modelBuilder.Entity<Psychologist>().HasData(
            new Psychologist
            {
                Id = 1,
                Name = "Ирина Смирнова",
                Specialization = "Когнитивно-поведенческая терапия",
                Description = "Специалист в лечении тревожных расстройств и депрессии с 12-летним опытом работы",
                Education = "Московский государственный университет им. М.В. Ломоносова, факультет психологии",
                Experience = "12 лет в клинической психологии",
                Languages = "Русский, Английский",
                Methods = "КПТ, Экспозиционная терапия, Техники релаксации",
                PhotoUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200&h=200&fit=crop",
                PricePerHour = 60,
                ContactPhone = "+375291234567",
                ContactEmail = "irina.smirnova@sofia.com",
                IsActive = true,
                CreatedAt = createdDate
            },
            new Psychologist
            {
                Id = 2,
                Name = "Алексей Иванов",
                Specialization = "Семейная психология и консультирование",
                Description = "Помогаю парам и семьям улучшить отношения и преодолеть конфликты",
                Education = "Институт психологии АН России, специализация в семейной системной терапии",
                Experience = "9 лет опыта в семейном консультировании",
                Languages = "Русский, Немецкий",
                Methods = "Системная терапия, Эмоционально-фокусированная терапия, Коммуникативные техники",
                PhotoUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200&h=200&fit=crop",
                PricePerHour = 55,
                ContactPhone = "+375291234568",
                ContactEmail = "alexey.ivanov@sofia.com",
                IsActive = true,
                CreatedAt = createdDate
            },
            new Psychologist
            {
                Id = 3,
                Name = "Мария Коваль",
                Specialization = "Психология здоровья и благополучия",
                Description = "Помогаю клиентам развить стрессоустойчивость и найти внутренний баланс",
                Education = "Санкт-Петербургский государственный университет, кафедра психологии развития",
                Experience = "7 лет в области позитивной психологии и mindfulness",
                Languages = "Русский, Французский, Английский",
                Methods = "Mindfulness, Медитация, Позитивная психология, Дыхательные практики",
                PhotoUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=200&h=200&fit=crop",
                PricePerHour = 50,
                ContactPhone = "+375291234569",
                ContactEmail = "maria.koval@sofia.com",
                IsActive = true,
                CreatedAt = createdDate
            }
        );

        // --- SEED: Built-in Tests (Depression, Anxiety, Stress) ---
        modelBuilder.Entity<Test>().HasData(
            new Test { Id = 1001, Name = "Шкала депрессии (PHQ-2)", Description = "Краткий скрининг симптомов депрессии", Type = Models.TestType.BuiltIn },
            new Test { Id = 1002, Name = "Шкала тревожности (GAD-2)", Description = "Краткий скрининг тревожности", Type = Models.TestType.BuiltIn },
            new Test { Id = 1003, Name = "Шкала стресса (PSS-4)", Description = "Краткая шкала восприятия стресса", Type = Models.TestType.BuiltIn }
        );

        // Questions for PHQ (1001) — expanded to realistic 10-item set
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2401, TestId = 1001, Text = "За последние 2 недели: вы теряли интерес или удовольствие от занятий, которые раньше нравились?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2402, TestId = 1001, Text = "За последние 2 недели: вы чувствовали грусть, подавленность или безнадежность?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2403, TestId = 1001, Text = "За последние 2 недели: у вас были проблемы со сном (трудности с засыпанием или пробуждение ранним утром)?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2404, TestId = 1001, Text = "За последние 2 недели: вы чувствовали усталость или потерю энергии?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2405, TestId = 1001, Text = "За последние 2 недели: у вас снизилась способность концентрироваться на задачах?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2406, TestId = 1001, Text = "За последние 2 недели: вы испытывали чувство собственной никчемности или чрезмерной вины?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2407, TestId = 1001, Text = "За последние 2 недели: вы заметили замедленность движений или, наоборот, беспокойство и суетливость?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2408, TestId = 1001, Text = "За последние 2 недели: у вас появлялись мысли о том, что вы лучше бы умерли, или навязчивые мысли о смерти?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2409, TestId = 1001, Text = "За последние 2 недели: вы замечали изменения в аппетите (уменьшение или увеличение)?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2410, TestId = 1001, Text = "За последние 2 недели: вы испытывали затруднения в принятии решений или планировании?", Type = Models.AnswerType.SingleChoice}
        );

        // Questions for GAD (1002) — expanded to 10 realistic items
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2411, TestId = 1002, Text = "За последние 2 недели: вы часто испытывали беспокойство или тревогу без видимой причины?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2412, TestId = 1002, Text = "За последние 2 недели: вам было трудно контролировать своё беспокойство?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2413, TestId = 1002, Text = "За последние 2 недели: вы чувствовали напряжение или мышечное напряжение?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2414, TestId = 1002, Text = "За последние 2 недели: вы легко уставали или испытывали слабость?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2415, TestId = 1002, Text = "За последние 2 недели: вы испытывали затруднения с концентрацией внимания?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2416, TestId = 1002, Text = "За последние 2 недели: у вас были проблемы со сном из‑за тревоги?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2417, TestId = 1002, Text = "За последние 2 недели: вы чувствовали себя раздражительным(ой) или нервным(ой)?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2418, TestId = 1002, Text = "За последние 2 недели: вы старались избегать ситуаций, которые вызывают тревогу?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2419, TestId = 1002, Text = "За последние 2 недели: тревога мешала вашей работе, учебе или общению?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2420, TestId = 1002, Text = "За последние 2 недели: вы испытывали панические приступы или приступы сильного страха?", Type = Models.AnswerType.SingleChoice}
        );

        // Questions for PSS (1003) — expanded to 10 realistic items (0..4 scale)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2421, TestId = 1003, Text = "За последний месяц: вы чувствовали, что не можете контролировать важные вещи в жизни?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2422, TestId = 1003, Text = "За последний месяц: вы чувствовали себя нервным(ой) и 'на нервах'", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2423, TestId = 1003, Text = "За последний месяц: вы чувствовали, что справляетесь с личными проблемами?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2424, TestId = 1003, Text = "За последний месяц: вы сталкивались с ситуациями, которые вызывали у вас ощущение перегруженности?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2425, TestId = 1003, Text = "За последний месяц: вы находили достаточно времени для отдыха и восстановления?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2426, TestId = 1003, Text = "За последний месяц: вы чувствовали, что требования со стороны работы/учебы/домашних слишком велики?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2427, TestId = 1003, Text = "За последний месяц: вы замечали, что негативные события занимают ваши мысли дольше, чем хотелось бы?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2428, TestId = 1003, Text = "За последний месяц: вы справлялись с неожиданными трудностями?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2429, TestId = 1003, Text = "За последний месяц: вы чувствовали себя уверенно в своих решениях?", Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2430, TestId = 1003, Text = "За последний месяц: вы чувствовали, что у вас есть ресурсы (временные, эмоциональные, социальные) для решения проблем?", Type = Models.AnswerType.SingleChoice}
        );

        // Answers for expanded PHQ (2401-2410) and GAD (2411-2420) — standard 4-point frequency scale
        modelBuilder.Entity<Answer>().HasData(
            // PHQ answers (0..3)
            new Answer { Id = 4001, QuestionId = 2401, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4002, QuestionId = 2401, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4003, QuestionId = 2401, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4004, QuestionId = 2401, Text = "Почти каждый день", Value = 3, Order = 3 },
            // reuse same 4-option set for other PHQ items
            new Answer { Id = 4005, QuestionId = 2402, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4006, QuestionId = 2402, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4007, QuestionId = 2402, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4008, QuestionId = 2402, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4009, QuestionId = 2403, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4010, QuestionId = 2403, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4011, QuestionId = 2403, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4012, QuestionId = 2403, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4013, QuestionId = 2404, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4014, QuestionId = 2404, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4015, QuestionId = 2404, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4016, QuestionId = 2404, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4017, QuestionId = 2405, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4018, QuestionId = 2405, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4019, QuestionId = 2405, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4020, QuestionId = 2405, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4021, QuestionId = 2406, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4022, QuestionId = 2406, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4023, QuestionId = 2406, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4024, QuestionId = 2406, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4025, QuestionId = 2407, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4026, QuestionId = 2407, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4027, QuestionId = 2407, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4028, QuestionId = 2407, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4029, QuestionId = 2408, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4030, QuestionId = 2408, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4031, QuestionId = 2408, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4032, QuestionId = 2408, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4033, QuestionId = 2409, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4034, QuestionId = 2409, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4035, QuestionId = 2409, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4036, QuestionId = 2409, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4037, QuestionId = 2410, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4038, QuestionId = 2410, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4039, QuestionId = 2410, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4040, QuestionId = 2410, Text = "Почти каждый день", Value = 3, Order = 3 },

            // GAD answers (2411-2420) same 4-point frequency
            new Answer { Id = 4041, QuestionId = 2411, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4042, QuestionId = 2411, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4043, QuestionId = 2411, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4044, QuestionId = 2411, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4045, QuestionId = 2412, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4046, QuestionId = 2412, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4047, QuestionId = 2412, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4048, QuestionId = 2412, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4049, QuestionId = 2413, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4050, QuestionId = 2413, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4051, QuestionId = 2413, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4052, QuestionId = 2413, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4053, QuestionId = 2414, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4054, QuestionId = 2414, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4055, QuestionId = 2414, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4056, QuestionId = 2414, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4057, QuestionId = 2415, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4058, QuestionId = 2415, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4059, QuestionId = 2415, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4060, QuestionId = 2415, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4061, QuestionId = 2416, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4062, QuestionId = 2416, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4063, QuestionId = 2416, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4064, QuestionId = 2416, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4065, QuestionId = 2417, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4066, QuestionId = 2417, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4067, QuestionId = 2417, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4068, QuestionId = 2417, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4069, QuestionId = 2418, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4070, QuestionId = 2418, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4071, QuestionId = 2418, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4072, QuestionId = 2418, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4073, QuestionId = 2419, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4074, QuestionId = 2419, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4075, QuestionId = 2419, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4076, QuestionId = 2419, Text = "Почти каждый день", Value = 3, Order = 3 },
            new Answer { Id = 4077, QuestionId = 2420, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 4078, QuestionId = 2420, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 4079, QuestionId = 2420, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 4080, QuestionId = 2420, Text = "Почти каждый день", Value = 3, Order = 3 }
        );

        // Answers for PSS (2421-2430) use 0..4 scale
        modelBuilder.Entity<Answer>().HasData(
            new Answer { Id = 5001, QuestionId = 2421, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5002, QuestionId = 2421, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5003, QuestionId = 2421, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5004, QuestionId = 2421, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5005, QuestionId = 2421, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5006, QuestionId = 2422, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5007, QuestionId = 2422, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5008, QuestionId = 2422, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5009, QuestionId = 2422, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5010, QuestionId = 2422, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5011, QuestionId = 2423, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5012, QuestionId = 2423, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5013, QuestionId = 2423, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5014, QuestionId = 2423, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5015, QuestionId = 2423, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5016, QuestionId = 2424, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5017, QuestionId = 2424, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5018, QuestionId = 2424, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5019, QuestionId = 2424, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5020, QuestionId = 2424, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5021, QuestionId = 2425, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5022, QuestionId = 2425, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5023, QuestionId = 2425, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5024, QuestionId = 2425, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5025, QuestionId = 2425, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5026, QuestionId = 2426, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5027, QuestionId = 2426, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5028, QuestionId = 2426, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5029, QuestionId = 2426, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5030, QuestionId = 2426, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5031, QuestionId = 2427, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5032, QuestionId = 2427, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5033, QuestionId = 2427, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5034, QuestionId = 2427, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5035, QuestionId = 2427, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5036, QuestionId = 2428, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5037, QuestionId = 2428, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5038, QuestionId = 2428, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5039, QuestionId = 2428, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5040, QuestionId = 2428, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5041, QuestionId = 2429, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5042, QuestionId = 2429, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5043, QuestionId = 2429, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5044, QuestionId = 2429, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5045, QuestionId = 2429, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 5046, QuestionId = 2430, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 5047, QuestionId = 2430, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 5048, QuestionId = 2430, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 5049, QuestionId = 2430, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 5050, QuestionId = 2430, Text = "Очень часто", Value = 4, Order = 4 }
        );

        // No additional test interpretations — they are already seeded in AddTestInterpretations migration

        // --- Additional built-in tests (each with 10 questions) ---
        modelBuilder.Entity<Test>().HasData(
            new Test { Id = 1004, Name = "Шкала устойчивости (RS-10)", Description = "Краткая шкала устойчивости к стрессу", Type = Models.TestType.BuiltIn },
            new Test { Id = 1005, Name = "Краткая шкала сна (Sleep-10)", Description = "Вопросы о качестве сна", Type = Models.TestType.BuiltIn },
            new Test { Id = 1006, Name = "Шкала эмоциональной регуляции", Description = "Оценка навыков регуляции эмоций", Type = Models.TestType.BuiltIn },
            new Test { Id = 1007, Name = "Шкала социальной поддержки", Description = "Оценка уровня социальной поддержки", Type = Models.TestType.BuiltIn }
        );

        // Questions for test 1004 (RS-10)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2301, TestId = 1004, Text = "Я легко приспосабливаюсь к изменениям.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2302, TestId = 1004, Text = "Я могу справляться с трудностями.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2303, TestId = 1004, Text = "У меня есть внутренняя сила, чтобы преодолевать проблемы.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2304, TestId = 1004, Text = "Я быстро восстанавливаюсь после неудач.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2305, TestId = 1004, Text = "Мне удаётся сохранять ясность мышления в стрессовых ситуациях.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2306, TestId = 1004, Text = "Я могу найти выход даже из сложной ситуации.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2307, TestId = 1004, Text = "Я уверен(а) в своей способности контролировать жизнь.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2308, TestId = 1004, Text = "Я нахожу новые способы решения проблем.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2309, TestId = 1004, Text = "Я сохраняю чувство юмора в трудные моменты.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2310, TestId = 1004, Text = "Я могу опираться на свои ресурсы при стрессе.", Type = Models.AnswerType.SingleChoice }
        );

        // Questions for test 1005 (Sleep-10)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2321, TestId = 1005, Text = "Вы засыпаете в течение 30 минут или меньше?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2322, TestId = 1005, Text = "Вы часто просыпаетесь ночью?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2323, TestId = 1005, Text = "Вы чувствуете, что сон восстанавливает силы?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2324, TestId = 1005, Text = "Вы просыпаетесь рано и не можете снова заснуть?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2325, TestId = 1005, Text = "Вы часто испытываете дневную сонливость?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2326, TestId = 1005, Text = "Вы часто используете снотворные или алкоголь, чтобы заснуть?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2327, TestId = 1005, Text = "Вы доволены качеством своего сна?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2328, TestId = 1005, Text = "Ваш сон влияет на выполнение рабочих/учебных задач?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2329, TestId = 1005, Text = "Вы просыпаетесь от громких звуков или движений?", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2330, TestId = 1005, Text = "Вы удовлетворены продолжительностью своего сна?", Type = Models.AnswerType.SingleChoice }
        );

        // Questions for test 1006 (Emotion regulation)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2341, TestId = 1006, Text = "Я контролирую свои эмоциональные реакции.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2342, TestId = 1006, Text = "Я использую техники для успокоения, когда расстроен(а).", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2343, TestId = 1006, Text = "Мне удаётся переосмыслить негативные события.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2344, TestId = 1006, Text = "Я замечаю ранние признаки эмоционального напряжения.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2345, TestId = 1006, Text = "Я умею выражать эмоции конструктивно.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2346, TestId = 1006, Text = "Я умею отвлекаться от навязчивых мыслей.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2347, TestId = 1006, Text = "Я использую дыхательные или релаксационные практики при стрессе.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2348, TestId = 1006, Text = "Я могу просить помощи у других при эмоциональной перегрузке.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2349, TestId = 1006, Text = "Я способен(на) восстанавливаться после эмоциональных срывов.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2350, TestId = 1006, Text = "Я чувствую, что управляю своими эмоциями в повседневной жизни.", Type = Models.AnswerType.SingleChoice }
        );

        // Questions for test 1007 (Social support)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2361, TestId = 1007, Text = "У меня есть люди, к которым можно обратиться за помощью.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2362, TestId = 1007, Text = "Я чувствую эмоциональную поддержку со стороны близких.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2363, TestId = 1007, Text = "Мне легко просить помощи, когда это нужно.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2364, TestId = 1007, Text = "У меня есть друзья/коллеги, с которыми приятно общаться.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2365, TestId = 1007, Text = "Я чувствую, что меня понимают и принимают.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2366, TestId = 1007, Text = "У меня есть человек, с кем можно обсудить важные дела.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2367, TestId = 1007, Text = "Я могу рассчитывать на помощь в критической ситуации.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2368, TestId = 1007, Text = "У меня есть доверенные люди, с которыми можно поделиться переживаниями.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2369, TestId = 1007, Text = "Я удовлетворён(на) уровнем своей социальной поддержки.", Type = Models.AnswerType.SingleChoice },
            new Question { Id = 2370, TestId = 1007, Text = "Я чувствую, что не остаюсь один(одна) с проблемами.", Type = Models.AnswerType.SingleChoice }
        );

        // Answers for new tests (use 0-3 scale)
        var answerId = 3301;
        var addAnswer = new Action<int,int[]>( (qId, unused) => { });

        // We'll add answers programmatically by constructing HasData entries inline
        modelBuilder.Entity<Answer>().HasData(
            // For test 1004 questions (2301-2310)
            // Each question: 4 answers 0..3
            new Answer { Id = 3301, QuestionId = 2301, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3302, QuestionId = 2301, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3303, QuestionId = 2301, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3304, QuestionId = 2301, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3305, QuestionId = 2302, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3306, QuestionId = 2302, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3307, QuestionId = 2302, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3308, QuestionId = 2302, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3309, QuestionId = 2303, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3310, QuestionId = 2303, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3311, QuestionId = 2303, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3312, QuestionId = 2303, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3313, QuestionId = 2304, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3314, QuestionId = 2304, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3315, QuestionId = 2304, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3316, QuestionId = 2304, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3317, QuestionId = 2305, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3318, QuestionId = 2305, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3319, QuestionId = 2305, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3320, QuestionId = 2305, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3321, QuestionId = 2306, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3322, QuestionId = 2306, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3323, QuestionId = 2306, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3324, QuestionId = 2306, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3325, QuestionId = 2307, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3326, QuestionId = 2307, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3327, QuestionId = 2307, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3328, QuestionId = 2307, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3329, QuestionId = 2308, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3330, QuestionId = 2308, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3331, QuestionId = 2308, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3332, QuestionId = 2308, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3333, QuestionId = 2309, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3334, QuestionId = 2309, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3335, QuestionId = 2309, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3336, QuestionId = 2309, Text = "Всегда", Value = 3, Order = 3 },

            new Answer { Id = 3337, QuestionId = 2310, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3338, QuestionId = 2310, Text = "Иногда", Value = 1, Order = 1 },
            new Answer { Id = 3339, QuestionId = 2310, Text = "Часто", Value = 2, Order = 2 },
            new Answer { Id = 3340, QuestionId = 2310, Text = "Всегда", Value = 3, Order = 3 }
        );

        // For brevity, add baseline answers for other new tests (2321-2330, 2341-2350, 2361-2370)
        var baseId = 3341;
        var qBlocks = new[] { 2321,2322,2323,2324,2325,2326,2327,2328,2329,2330,
                             2341,2342,2343,2344,2345,2346,2347,2348,2349,2350,
                             2361,2362,2363,2364,2365,2366,2367,2368,2369,2370 };
        var answerList = new List<Answer>();
        foreach (var qid in qBlocks)
        {
            answerList.Add(new Answer { Id = baseId++, QuestionId = qid, Text = "Никогда", Value = 0, Order = 0 });
            answerList.Add(new Answer { Id = baseId++, QuestionId = qid, Text = "Иногда", Value = 1, Order = 1 });
            answerList.Add(new Answer { Id = baseId++, QuestionId = qid, Text = "Часто", Value = 2, Order = 2 });
            answerList.Add(new Answer { Id = baseId++, QuestionId = qid, Text = "Всегда", Value = 3, Order = 3 });
        }
        modelBuilder.Entity<Answer>().HasData(answerList.ToArray());

        //  Seed: Reviews (фиксированные даты)
        modelBuilder.Entity<PsychologistReview>().HasData(
            new PsychologistReview
            {
                Id = 1,
                PsychologistId = 1,
                Rating = 5,
                Comment = "Отличный специалист!",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-10T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            },
            new PsychologistReview
            {
                Id = 2,
                PsychologistId = 1,
                Rating = 4,
                Comment = "Профессиональный подход",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-05T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            },
            new PsychologistReview
            {
                Id = 3,
                PsychologistId = 2,
                Rating = 5,
                Comment = "Помог решить семейные проблемы",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-15T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            },
            new PsychologistReview
            {
                Id = 4,
                PsychologistId = 3,
                Rating = 5,
                Comment = "EMDR терапия действительно работает",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-20T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            }
        );
    }
}
