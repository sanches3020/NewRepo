<script>
document.addEventListener('DOMContentLoaded', function () {
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirmPassword');
    const username = document.getElementById('username');
    const email = document.getElementById('email');
    const fullName = document.getElementById('fullName');
    const form = document.querySelector('form');
    const submitBtn = form.querySelector('button[type="submit"]');

    function validateUsername() {
        const value = username.value.trim();
        const usernameRegex = /^(?!.*__)[A-Za-z][A-Za-z0-9_]{1,18}[A-Za-z0-9]$/;

        if (value.length < 3) {
            showFieldError(username, 'Минимум 3 символа');
            username.setCustomValidity('Имя пользователя должно содержать минимум 3 символа');
        } else if (value.length > 20) {
            showFieldError(username, 'Не более 20 символов');
            username.setCustomValidity('Имя пользователя не должно превышать 20 символов');
        } else if (!usernameRegex.test(value)) {
            showFieldError(username, 'Только латиница, цифры и _');
            username.setCustomValidity('Разрешены только латинские буквы, цифры и _ без двойных подчёркиваний');
        } else {
            hideFieldError(username);
            username.setCustomValidity('');
        }
    }

    function validateEmail() {
        const value = email.value.trim();
        const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

        if (!emailRegex.test(value)) {
            showFieldError(email, 'Некорректный email');
            email.setCustomValidity('Введите корректный email адрес');
        } else {
            hideFieldError(email);
            email.setCustomValidity('');
        }
    }

    function validatePassword() {
        const value = password.value;

        const passwordRegex =
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d!@#$%^&*()_+=<>?{}

\[\]

~]{6,32}$/;

        if (/\s/.test(value)) {
            showFieldError(password, 'Без пробелов');
            password.setCustomValidity('Пароль не должен содержать пробелов');
        } else if (value.length < 6) {
            showFieldError(password, 'Минимум 6 символов');
            password.setCustomValidity('Пароль должен содержать минимум 6 символов');
        } else if (value.length > 32) {
            showFieldError(password, 'Максимум 32 символа');
            password.setCustomValidity('Пароль не должен превышать 32 символа');
        } else if (!passwordRegex.test(value)) {
            showFieldError(password, 'Нужны буквы (верх/низ) + цифра');
            password.setCustomValidity('Пароль должен содержать заглавную, строчную буквы и цифру');
        } else {
            hideFieldError(password);
            password.setCustomValidity('');
        }

        validateConfirmPassword();
    }

    function validateConfirmPassword() {
        if (password.value !== confirmPassword.value) {
            showFieldError(confirmPassword, 'Пароли не совпадают');
            confirmPassword.setCustomValidity('Пароли не совпадают');
        } else {
            hideFieldError(confirmPassword);
            confirmPassword.setCustomValidity('');
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
        if (errorDiv) errorDiv.remove();
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

    fullName.addEventListener('input', function () {
        if (this.value.trim()) {
            this.classList.add('is-valid');
        } else {
            this.classList.remove('is-valid', 'is-invalid');
        }
    });

    form.addEventListener('submit', function (e) {
        if (!form.checkValidity()) {
            e.preventDefault();
            [username, email, password, confirmPassword].forEach(f =>
                f.dispatchEvent(new Event('blur'))
            );
            return;
        }

        submitBtn.disabled = true;
        submitBtn.innerHTML =
            '<span class="spinner-border spinner-border-sm me-2"></span>Создание аккаунта...';

        setTimeout(() => {
            submitBtn.disabled = false;
            submitBtn.innerHTML = '🚀 Создать аккаунт';
        }, 10000);
    });

    password.addEventListener('input', function () {
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
