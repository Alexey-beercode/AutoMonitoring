const apiUrl = 'http://gtstream.net/api/auth';

const exitAndRedirect = () => {
    const userId = localStorage.getItem('userId');
    const accessToken = localStorage.getItem('accessToken');

    const clearLocalStorage = () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userId');
    }

    fetch(`${apiUrl}/logout/${userId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
    })
    .finally(() => {
        clearLocalStorage();
        window.location.href = '/login';
    });
}

const refreshToken = () => {
    const refreshToken = localStorage.getItem('refreshToken');
    const deviceName = localStorage.getItem("deviceName");

    return fetch(`${apiUrl}/refresh`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ refreshToken, deviceName }),
    })
    .then(response => {
        if (!response.ok || response.status === 404) {
            return response.json().then(err => {
                throw new Error(err.error);
            });
        }
        return response.json();
    })
    .then(data => {
        localStorage.setItem('accessToken', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
    })
    .catch(error => {
        exitAndRedirect();  // Выйти, если обновление токена не удалось
    });
}

const checkIsTokenValid = async () => {
    const token = localStorage.getItem('accessToken');

    const response = await fetch(`${apiUrl}/getTokenStatus`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    });

    if (response.status === 401) {
        console.log("refresh token");
        await refreshToken();  // Ожидаем обновления токена
        const newToken = localStorage.getItem('accessToken');  // Получаем новый токен после обновления
        return fetch(`${apiUrl}/getTokenStatus`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${newToken}`
            },
        });
    }

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.error);
    }

    return response.json();
}

checkIsTokenValid().catch(error => {
    exitAndRedirect();
});
