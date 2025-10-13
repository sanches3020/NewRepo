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
