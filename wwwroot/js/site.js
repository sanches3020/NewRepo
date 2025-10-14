// Enhanced theme system with smooth transitions
(function() {
  const THEME_KEY = 'sofia.theme';
  const root = document.documentElement;
  
  function applyTheme(theme) {
    // Add transition for smooth theme change
    root.style.transition = 'background-color 0.3s ease, color 0.3s ease';
    
    if (theme === 'dark') {
      root.classList.add('theme-dark');
    } else {
      root.classList.remove('theme-dark');
    }
    
    // Update theme toggle button text
    const toggleBtn = document.querySelector('[data-theme-toggle]');
    if (toggleBtn) {
      const icon = toggleBtn.querySelector('span');
      if (icon) {
        icon.textContent = theme === 'dark' ? '☀️' : '🌙';
      }
    }
    
    // Remove transition after animation
    setTimeout(() => {
      root.style.transition = '';
    }, 300);
  }
  
  // Load saved theme
  const saved = localStorage.getItem(THEME_KEY);
  if (saved) {
    applyTheme(saved);
  } else {
    // Default to system preference
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    applyTheme(prefersDark ? 'dark' : 'light');
  }
  
  // Theme toggle handler
  document.addEventListener('click', function(e) {
    const toggleBtn = e.target.closest('[data-theme-toggle]');
    if (!toggleBtn) return;
    
    const isDark = root.classList.contains('theme-dark');
    const next = isDark ? 'light' : 'dark';
    
    applyTheme(next);
    localStorage.setItem(THEME_KEY, next);
    
    // Add a subtle animation feedback
    toggleBtn.style.transform = 'scale(0.95)';
    setTimeout(() => {
      toggleBtn.style.transform = '';
    }, 150);
  });
  
  // Listen for system theme changes
  window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function(e) {
    // Only apply if user hasn't manually set a preference
    if (!localStorage.getItem(THEME_KEY)) {
      applyTheme(e.matches ? 'dark' : 'light');
    }
  });
})();

// Enhanced UI interactions
document.addEventListener('DOMContentLoaded', function() {
  // Add loading states to buttons
  const buttons = document.querySelectorAll('.btn');
  buttons.forEach(btn => {
    btn.addEventListener('click', function() {
      if (this.type === 'submit' || this.classList.contains('btn-primary')) {
        this.classList.add('loading');
        setTimeout(() => {
          this.classList.remove('loading');
        }, 1000);
      }
    });
  });
  
  // Add hover effects to cards
  const cards = document.querySelectorAll('.card');
  cards.forEach(card => {
    card.addEventListener('mouseenter', function() {
      this.style.transform = 'translateY(-4px)';
    });
    
    card.addEventListener('mouseleave', function() {
      this.style.transform = '';
    });
  });
  
  // Smooth scroll for anchor links
  const anchorLinks = document.querySelectorAll('a[href^="#"]');
  anchorLinks.forEach(link => {
    link.addEventListener('click', function(e) {
      e.preventDefault();
      const target = document.querySelector(this.getAttribute('href'));
      if (target) {
        target.scrollIntoView({
          behavior: 'smooth',
          block: 'start'
        });
      }
    });
  });
  
  // Add focus indicators for keyboard navigation
  const focusableElements = document.querySelectorAll('button, a, input, select, textarea');
  focusableElements.forEach(element => {
    element.addEventListener('focus', function() {
      this.style.outline = '2px solid var(--color-primary)';
      this.style.outlineOffset = '2px';
    });
    
    element.addEventListener('blur', function() {
      this.style.outline = '';
      this.style.outlineOffset = '';
    });
  });
});

// TikTok-style Companion with Dialog System
class SofiaCompanion {
  constructor() {
    this.companion = null;
    this.messageElement = null;
    this.isVisible = true;
    this.messageQueue = [];
    this.currentMessageIndex = 0;
    this.lastInteraction = Date.now();
    
    // Companion stats
    this.stats = {
      level: 1,
      experience: 0,
      happiness: 80,
      energy: 100,
      coins: 0,
      totalInteractions: 0,
      notesCreated: 0,
      goalsCompleted: 0,
      daysActive: 1
    };
    
    // Achievements system
    this.achievements = {
      'first_note': { unlocked: false, name: 'Первая запись', icon: '📝', description: 'Создайте первую заметку' },
      'note_master': { unlocked: false, name: 'Мастер записей', icon: '📚', description: 'Создайте 10 заметок' },
      'goal_setter': { unlocked: false, name: 'Постановщик целей', icon: '🎯', description: 'Создайте первую цель' },
      'goal_achiever': { unlocked: false, name: 'Достигатель', icon: '🏆', description: 'Выполните 5 целей' },
      'daily_user': { unlocked: false, name: 'Ежедневный пользователь', icon: '📅', description: 'Используйте приложение 7 дней подряд' },
      'emotion_tracker': { unlocked: false, name: 'Трекер эмоций', icon: '😊', description: 'Запишите все типы эмоций' },
      'meditation_master': { unlocked: false, name: 'Мастер медитации', icon: '🧘‍♀️', description: 'Попробуйте все практики' },
      'social_butterfly': { unlocked: false, name: 'Социальная бабочка', icon: '👥', description: 'Поделитесь с психологом' }
    };
    
    // Messages based on level and mood
    this.messages = {
      level1: [
        "Привет! Я София! 🌟",
        "Как дела? Готов помочь!",
        "Не забывай о своих целях!",
        "Ты молодец! Продолжай!",
        "Нужна поддержка? Я здесь!"
      ],
      level2: [
        "Ура! Я расту! 🌱",
        "Ты помогаешь мне развиваться!",
        "Вместе мы сильнее! 💪",
        "Спасибо за заботу! ❤️",
        "Я чувствую себя лучше!"
      ],
      level3: [
        "Я стала мудрее! 🧠",
        "Могу дать больше советов!",
        "Твои успехи вдохновляют! ✨",
        "Мы отличная команда! 🤝",
        "Продолжай в том же духе!"
      ]
    };
    
    this.init();
  }
  
  init() {
    this.loadStats();
    this.createCompanion();
    this.createDialog();
    this.bindEvents();
    this.startRandomMessages();
    this.checkAchievements();
  }
  
  createCompanion() {
    const container = document.createElement('div');
    container.className = 'companion-container';
    container.innerHTML = `
      <div class="companion" id="sofia-companion">
        <div class="companion-body">
          <div class="companion-face">
            <div class="companion-eyes">
              <div class="companion-eye"></div>
              <div class="companion-eye"></div>
            </div>
            <div class="companion-mouth"></div>
          </div>
          <div class="companion-sparkles">
            <div class="sparkle"></div>
            <div class="sparkle"></div>
            <div class="sparkle"></div>
            <div class="sparkle"></div>
          </div>
        </div>
        <div class="companion-message" id="companion-message"></div>
      </div>
    `;
    
    document.body.appendChild(container);
    this.companion = document.getElementById('sofia-companion');
    this.messageElement = document.getElementById('companion-message');
  }
  
  createDialog() {
    const modal = document.createElement('div');
    modal.className = 'companion-modal';
    modal.id = 'companion-modal';
    modal.innerHTML = `
      <div class="companion-dialog">
        <div class="companion-dialog-header">
          <h2 class="companion-dialog-title">
            <span>🌟</span> София
          </h2>
          <button class="companion-dialog-close" onclick="window.sofiaCompanion.closeDialog()">×</button>
        </div>
        <div class="companion-dialog-body">
          <div class="companion-status">
            <div class="companion-avatar">
              <div class="companion-body">
                <div class="companion-face">
                  <div class="companion-eyes">
                    <div class="companion-eye"></div>
                    <div class="companion-eye"></div>
                  </div>
                  <div class="companion-mouth"></div>
                </div>
                <div class="companion-sparkles">
                  <div class="sparkle"></div>
                  <div class="sparkle"></div>
                  <div class="sparkle"></div>
                  <div class="sparkle"></div>
                </div>
              </div>
            </div>
            <div class="companion-name">София</div>
            <div class="companion-level">Уровень ${this.stats.level}</div>
            <div class="companion-happiness">
              <span>😊</span>
              <div class="happiness-bar">
                <div class="happiness-fill" style="width: ${this.stats.happiness}%"></div>
              </div>
              <span>${this.stats.happiness}%</span>
            </div>
          </div>
          
          <div class="companion-stats">
            <div class="stat-item">
              <div class="stat-value">${this.stats.experience}</div>
              <div class="stat-label">Опыт</div>
            </div>
            <div class="stat-item">
              <div class="stat-value">${this.stats.coins}</div>
              <div class="stat-label">Монеты</div>
            </div>
            <div class="stat-item">
              <div class="stat-value">${this.stats.notesCreated}</div>
              <div class="stat-label">Заметки</div>
            </div>
            <div class="stat-item">
              <div class="stat-value">${this.stats.goalsCompleted}</div>
              <div class="stat-label">Цели</div>
            </div>
          </div>
          
          <div class="companion-actions">
            <button class="action-btn" onclick="window.sofiaCompanion.feedCompanion()">
              🍎 Покормить
            </button>
            <button class="action-btn" onclick="window.sofiaCompanion.playWithCompanion()">
              🎮 Поиграть
            </button>
            <button class="action-btn" onclick="window.sofiaCompanion.petCompanion()">
              🥰 Погладить
            </button>
            <button class="action-btn" onclick="window.sofiaCompanion.meditateWithCompanion()">
              🧘‍♀️ Медитация
            </button>
          </div>
          
          <div class="companion-messages" id="companion-messages">
            <div class="message-item">Привет! Я рада тебя видеть! 👋</div>
          </div>
          
          <div class="companion-achievements">
            <div class="achievements-title">🏆 Достижения</div>
            <div class="achievements-grid" id="achievements-grid">
              ${this.renderAchievements()}
            </div>
          </div>
        </div>
        <div class="companion-dialog-footer">
          <button class="btn-companion" onclick="window.sofiaCompanion.closeDialog()">Закрыть</button>
          <button class="btn-companion primary" onclick="window.sofiaCompanion.resetCompanion()">Сбросить</button>
        </div>
      </div>
    `;
    
    document.body.appendChild(modal);
    this.modal = modal;
  }

  renderAchievements() {
    return Object.entries(this.achievements).map(([key, achievement]) => `
      <div class="achievement-item ${achievement.unlocked ? 'unlocked' : ''}" 
           title="${achievement.description}">
        <div class="achievement-icon">${achievement.icon}</div>
        <div>${achievement.name}</div>
      </div>
    `).join('');
  }

  bindEvents() {
    // Click interaction - open dialog
    this.companion.addEventListener('click', () => {
      this.openDialog();
      this.addExperience(5);
      this.lastInteraction = Date.now();
    });
    
    // Hover effects
    this.companion.addEventListener('mouseenter', () => {
      this.companion.style.transform = 'scale(1.1)';
    });
    
    this.companion.addEventListener('mouseleave', () => {
      this.companion.style.transform = 'scale(1)';
    });
    
    // Page visibility change
    document.addEventListener('visibilitychange', () => {
      if (document.hidden) {
        this.hideCompanion();
      } else {
        setTimeout(() => this.showCompanion(), 1000);
      }
    });
    
    // Scroll interaction
    let scrollTimeout;
    window.addEventListener('scroll', () => {
      if (scrollTimeout) clearTimeout(scrollTimeout);
      
      scrollTimeout = setTimeout(() => {
        if (Math.random() < 0.1) { // 10% chance
          this.showMessage("Не торопись, читай внимательно! 📖");
        }
      }, 2000);
    });
  }
  
  showMessage(message) {
    this.messageElement.textContent = message;
    this.messageElement.classList.add('show');
    
    setTimeout(() => {
      this.messageElement.classList.remove('show');
    }, 3000);
  }
  
  getRandomMessage() {
    const levelMessages = this.messages[`level${this.stats.level}`] || this.messages.level1;
    const availableMessages = levelMessages.filter((_, index) => index !== this.currentMessageIndex);
    const randomIndex = Math.floor(Math.random() * availableMessages.length);
    this.currentMessageIndex = availableMessages[randomIndex];
    return availableMessages[randomIndex];
  }

  // Dialog management
  openDialog() {
    this.modal.classList.add('show');
    this.updateDialogStats();
    this.addMessage("Привет! Как дела? 😊");
  }

  closeDialog() {
    this.modal.classList.remove('show');
  }

  updateDialogStats() {
    if (!this.modal) return;
    
    // Update level
    const levelElement = this.modal.querySelector('.companion-level');
    if (levelElement) levelElement.textContent = `Уровень ${this.stats.level}`;
    
    // Update happiness bar
    const happinessFill = this.modal.querySelector('.happiness-fill');
    const happinessText = this.modal.querySelector('.companion-happiness span:last-child');
    if (happinessFill) happinessFill.style.width = `${this.stats.happiness}%`;
    if (happinessText) happinessText.textContent = `${this.stats.happiness}%`;
    
    // Update stats
    const statValues = this.modal.querySelectorAll('.stat-value');
    if (statValues.length >= 4) {
      statValues[0].textContent = this.stats.experience;
      statValues[1].textContent = this.stats.coins;
      statValues[2].textContent = this.stats.notesCreated;
      statValues[3].textContent = this.stats.goalsCompleted;
    }
    
    // Update achievements
    const achievementsGrid = this.modal.querySelector('#achievements-grid');
    if (achievementsGrid) {
      achievementsGrid.innerHTML = this.renderAchievements();
    }
  }

  addMessage(message) {
    if (!this.modal) return;
    
    const messagesContainer = this.modal.querySelector('#companion-messages');
    if (messagesContainer) {
      const messageElement = document.createElement('div');
      messageElement.className = 'message-item';
      messageElement.textContent = message;
      messagesContainer.appendChild(messageElement);
      messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }
  }

  // Companion actions
  feedCompanion() {
    if (this.stats.energy < 20) {
      this.addMessage("Я не голодна! 🥺");
      return;
    }
    
    this.stats.energy -= 20;
    this.stats.happiness = Math.min(100, this.stats.happiness + 15);
    this.addExperience(10);
    this.addCoins(5);
    this.addMessage("Спасибо за еду! Вкусно! 😋");
    this.updateDialogStats();
  }

  playWithCompanion() {
    if (this.stats.energy < 15) {
      this.addMessage("Я устала, давай отдохнем! 😴");
      return;
    }
    
    this.stats.energy -= 15;
    this.stats.happiness = Math.min(100, this.stats.happiness + 20);
    this.addExperience(15);
    this.addCoins(8);
    this.addMessage("Ура! Было весело! 🎉");
    this.updateDialogStats();
  }

  petCompanion() {
    this.stats.happiness = Math.min(100, this.stats.happiness + 10);
    this.addExperience(5);
    this.addCoins(3);
    this.addMessage("Ммм, приятно! 🥰");
    this.updateDialogStats();
  }

  meditateWithCompanion() {
    this.stats.happiness = Math.min(100, this.stats.happiness + 25);
    this.stats.energy = Math.min(100, this.stats.energy + 20);
    this.addExperience(20);
    this.addCoins(10);
    this.addMessage("Как спокойно и мирно! 🧘‍♀️✨");
    this.updateDialogStats();
  }

  // Stats management
  addExperience(amount) {
    this.stats.experience += amount;
    this.stats.totalInteractions++;
    
    // Level up check
    const expNeeded = this.stats.level * 100;
    if (this.stats.experience >= expNeeded) {
      this.levelUp();
    }
    
    this.saveStats();
  }

  addCoins(amount) {
    this.stats.coins += amount;
    this.saveStats();
  }

  levelUp() {
    this.stats.level++;
    this.stats.happiness = 100;
    this.stats.energy = 100;
    this.addMessage(`🎉 Уровень ${this.stats.level}! Я расту! 🌱`);
    this.addCoins(this.stats.level * 20);
    
    // Unlock new features based on level
    if (this.stats.level >= 2) {
      this.addMessage("Теперь я могу давать больше советов! 💡");
    }
    if (this.stats.level >= 3) {
      this.addMessage("Я стала мудрее! Могу помочь с медитацией! 🧘‍♀️");
    }
  }

  // Achievement system
  checkAchievements() {
    // Check note achievements
    if (this.stats.notesCreated >= 1 && !this.achievements.first_note.unlocked) {
      this.unlockAchievement('first_note');
    }
    if (this.stats.notesCreated >= 10 && !this.achievements.note_master.unlocked) {
      this.unlockAchievement('note_master');
    }
    
    // Check goal achievements
    if (this.stats.goalsCompleted >= 1 && !this.achievements.goal_setter.unlocked) {
      this.unlockAchievement('goal_setter');
    }
    if (this.stats.goalsCompleted >= 5 && !this.achievements.goal_achiever.unlocked) {
      this.unlockAchievement('goal_achiever');
    }
    
    // Check daily usage
    if (this.stats.daysActive >= 7 && !this.achievements.daily_user.unlocked) {
      this.unlockAchievement('daily_user');
    }
  }

  unlockAchievement(achievementKey) {
    this.achievements[achievementKey].unlocked = true;
    this.addCoins(50);
    this.addMessage(`🏆 Достижение разблокировано: ${this.achievements[achievementKey].name}!`);
    this.updateDialogStats();
  }

  // External actions (called from other parts of the app)
  onNoteCreated() {
    this.stats.notesCreated++;
    this.addExperience(10);
    this.addCoins(5);
    this.addMessage("Отлично! Записывай свои мысли! 📝");
    this.checkAchievements();
    this.saveStats();
  }

  onGoalCompleted() {
    this.stats.goalsCompleted++;
    this.addExperience(25);
    this.addCoins(15);
    this.addMessage("Поздравляю с достижением цели! 🎯🏆");
    this.checkAchievements();
    this.saveStats();
  }

  onGoalCreated() {
    this.addExperience(5);
    this.addCoins(3);
    this.addMessage("Отличная цель! Ты справишься! 💪");
    this.checkAchievements();
    this.saveStats();
  }

  // Data persistence
  saveStats() {
    localStorage.setItem('sofiaCompanionStats', JSON.stringify(this.stats));
    localStorage.setItem('sofiaCompanionAchievements', JSON.stringify(this.achievements));
  }

  loadStats() {
    const savedStats = localStorage.getItem('sofiaCompanionStats');
    if (savedStats) {
      this.stats = { ...this.stats, ...JSON.parse(savedStats) };
    }
    
    const savedAchievements = localStorage.getItem('sofiaCompanionAchievements');
    if (savedAchievements) {
      this.achievements = { ...this.achievements, ...JSON.parse(savedAchievements) };
    }
  }

  resetCompanion() {
    if (confirm('Вы уверены, что хотите сбросить прогресс Софии?')) {
      this.stats = {
        level: 1,
        experience: 0,
        happiness: 80,
        energy: 100,
        coins: 0,
        totalInteractions: 0,
        notesCreated: 0,
        goalsCompleted: 0,
        daysActive: 1
      };
      
      Object.keys(this.achievements).forEach(key => {
        this.achievements[key].unlocked = false;
      });
      
      this.saveStats();
      this.updateDialogStats();
      this.addMessage("Начинаем заново! 🌟");
    }
  }
  
  startRandomMessages() {
    setInterval(() => {
      const timeSinceLastInteraction = Date.now() - this.lastInteraction;
      
      // Show random message if no interaction for 30 seconds
      if (timeSinceLastInteraction > 30000 && this.isVisible) {
        this.showMessage(this.getRandomMessage());
        this.lastInteraction = Date.now();
      }
    }, 10000); // Check every 10 seconds
  }
  
  hideCompanion() {
    this.companion.style.opacity = '0';
    this.isVisible = false;
  }
  
  showCompanion() {
    this.companion.style.opacity = '1';
    this.isVisible = true;
  }
  
  // Public methods for external interaction
  celebrate() {
    this.showMessage("Ура! Отлично! 🎉🎊");
    this.companion.classList.add('bounce');
    setTimeout(() => {
      this.companion.classList.remove('bounce');
    }, 1000);
  }
  
  encourage() {
    this.showMessage("Ты справишься! Я верю в тебя! 💪");
  }
  
  remind() {
    this.showMessage("Не забывай о своих целях! ⭐");
  }
}

// Initialize companion when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
  window.sofiaCompanion = new SofiaCompanion();
});

// Global functions for external use
function celebrateCompanion() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.celebrate();
  }
}

function encourageCompanion() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.encourage();
  }
}

function remindCompanion() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.remind();
  }
}

function openCompanionDialog() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.openDialog();
  }
}

// New functions for companion actions
function onNoteCreated() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.onNoteCreated();
  }
}

function onGoalCreated() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.onGoalCreated();
  }
}

function onGoalCompleted() {
  if (window.sofiaCompanion) {
    window.sofiaCompanion.onGoalCompleted();
  }
}
