document.getElementById('loginForm').addEventListener('submit', function(event) {
    event.preventDefault(); // Предотвращаем перезагрузку страницы

    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;
    const deviceName = localStorage.getItem('deviceName');

    const errorElement = document.querySelector('.login__error');

    // Замените URL на ваш адрес API
    const apiUrl = 'https://localhost:44316/api/auth/login';

    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ login, password, deviceName }),
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(err => {
                if(err.errors)
                    errorElement.innerHTML = Object.entries(err.errors)[0][1][0]
                else errorElement.innerHTML = err.error;
                throw new Error(err.error);
            });
        }
        return response.json();
    })
    .then(data => {
        localStorage.setItem('accessToken', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
        localStorage.setItem('userId', data.userId);

        window.location.href = '/truck_v21.06.24/content.html'; // Замените на вашу страницу
    })
});