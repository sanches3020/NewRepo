<script>
    document.addEventListener('DOMContentLoaded', function() {
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirmPassword');
    const username = document.getElementById('username');
    const email = document.getElementById('email');
    const fullName = document.getElementById('fullName');
    const form = document.querySelector('form');
    const submitBtn = form.querySelector('button[type="submit"]');

    function validateUsername() {
        const value = username.value.trim();
    const usernameRegex = /^(?!.*__)[A-Za-z][A-Za-z0-9_]{1, 18}[A-Za-z0-9]$/;

    if (value.length < 3) {
        username.setCustomValidity('Имя пользователя должно содержать минимум 3 символа');
    showFieldError(username, 'Минимум 3 символа');
        } else if (value.length > 20) {
        username.setCustomValidity('Имя пользователя не должно превышать 20 символов');
    showFieldError(username, 'Не более 20 символов');
        } else if (!usernameRegex.test(value)) {
        username.setCustomValidity('Разрешены только латинские буквы, цифры и _ без двойных подчёркиваний');
    showFieldError(username, 'Только латиница, цифры и _');
        } else {
        username.setCustomValidity('');
    hideFieldError(username);
        }
    }

    function validateEmail() {
        const value = email.value.trim();
    const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    if (!emailRegex.test(value)) {
        email.setCustomValidity('Введите корректный email адрес');
    showFieldError(email, 'Некорректный email');
        } else {
        email.setCustomValidity('');
    hideFieldError(email);
        }
    }

    function validatePassword() {
        const value = password.value;
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{6, 32}$/;

    if (value.length < 6) {
        password.setCustomValidity('Пароль должен содержать минимум 6 символов');
    showFieldError(password, 'Минимум 6 символов');
        } else if (value.length > 32) {
        password.setCustomValidity('Пароль не должен превышать 32 символа');
    showFieldError(password, 'Максимум 32 символа');
        } else if (!passwordRegex.test(value)) {
        password.setCustomValidity('Пароль должен содержать заглавную, строчную буквы и цифру, без пробелов');
    showFieldError(password, 'Нужны буквы (верх/низ) + цифра, без пробелов');
        } else {
        password.setCustomValidity('');
    hideFieldError(password);
        }

    validateConfirmPassword();
    }

    function validateConfirmPassword() {
        if (password.value !== confirmPassword.value) {
        confirmPassword.setCustomValidity('Пароли не совпадают');
    showFieldError(confirmPassword, 'Пароли не совпадают');
        } else {
        confirmPassword.setCustomValidity('');
    hideFieldError(confirmPassword);
        }
    }

    function showFieldError(field, message) {
        let errorDiv = field.parentNode.querySelector('.field-error');
    if (!errorDiv) {
        errorDiv = document.createElement('div');
    errorDiv.className = 'field-error text-danger small mt-1';
    field.parentNode.appendChild(errorDiv);
        }
    errorDiv.textContent = message;
    field.classList.add('is-invalid');
    field.classList.remove('is-valid');
    }

    function hideFieldError(field) {
        const errorDiv = field.parentNode.querySelector('.field-error');
    if (errorDiv) {
        errorDiv.remove();
        }
    field.classList.remove('is-invalid');
    field.classList.add('is-valid');
    }

    username.addEventListener('input', validateUsername);
    username.addEventListener('blur', validateUsername);

    email.addEventListener('input', validateEmail);
    email.addEventListener('blur', validateEmail);

    password.addEventListener('input', validatePassword);
    password.addEventListener('blur', validatePassword);

    confirmPassword.addEventListener('input', validateConfirmPassword);
    confirmPassword.addEventListener('blur', validateConfirmPassword);

    fullName.addEventListener('input', function() {
        if (this.value.trim()) {
        this.classList.add('is-valid');
        } else {
        this.classList.remove('is-valid', 'is-invalid');
        }
    });

    form.addEventListener('submit', function(e) {
        if (!form.checkValidity()) {
        e.preventDefault();
            [username, email, password, confirmPassword].forEach(field => {
        field.dispatchEvent(new Event('blur'));
            });
    return;
        }

    submitBtn.disabled = true;
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Создание аккаунта...';

        setTimeout(() => {
        submitBtn.disabled = false;
    submitBtn.innerHTML = '🚀 Создать аккаунт';
        }, 10000);
    });

    password.addEventListener('input', function() {
        const strength = calculatePasswordStrength(this.value);
    updatePasswordStrengthIndicator(strength);
    });

    function calculatePasswordStrength(password) {
        let strength = 0;
        if (password.length >= 6) strength++;
        if (password.length >= 8) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/\d/.test(password)) strength++;
    if (/[^a-zA-Z\d]/.test(password)) strength++;
    return strength;
    }

    function updatePasswordStrengthIndicator(strength) {
        let indicator = password.parentNode.querySelector('.password-strength');
    if (!indicator) {
        indicator = document.createElement('div');
    indicator.className = 'password-strength mt-1';
    password.parentNode.appendChild(indicator);
        }

    const labels = ['Очень слабый', 'Слабый', 'Средний', 'Хороший', 'Отличный'];
    const colors = ['#dc3545', '#fd7e14', '#ffc107', '#20c997', '#28a745'];

    if (strength === 0) {
        indicator.style.display = 'none';
        } else {
        indicator.style.display = 'block';
    indicator.innerHTML = `
    <small class="text-muted">Надёжность пароля:
        <span style="color: ${colors[strength - 1]}; font-weight: bold;">
            ${labels[strength - 1]}
        </span>
    </small>
    `;
        }
    }
});
</script>
