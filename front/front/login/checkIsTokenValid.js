const apiUrl = 'http://localhost:5007/api/auth';

const exitAndRedirect = () => {
    const userId = localStorage.getItem('userId');
    const accessToken = localStorage.getItem('accessToken');

    const clearLocalStorage = () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
    }

    fetch(`${apiUrl}/logout/${userId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        },
    })
    .then(response => {
        window.location.href = '/login';
        clearLocalStorage()
    })
}

const refreshToken = () => {
    const refreshToken = localStorage.getItem('refreshToken');
    const deviceName = localStorage.getItem("deviceName");

    fetch(`${apiUrl}/refresh`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${refreshToken}`
        },
        body: JSON.stringify({ refreshToken, deviceName }),
    })
    .then(response => {
        if (!response.ok || response.status == 404) {
            return response.json().then(err => {
                throw new Error(err.error);
            });
        }
    })
    .then(data => {
        localStorage.setItem('accessToken', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
    })
    .catch(error => {
        exitAndRedirect()
    }) 
}

const checkIsTokenValid = () => {
    const token = localStorage.getItem('accessToken');

    fetch(`${apiUrl}/getTokenStatus`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    })
    .then(response => {
        if(response.status == 401) {
            refreshToken();
        }
        else if(!response.ok) {
            return response.json().then(err => {
                throw new Error(err.error);
            });
        }
        return response.json();
    })
    .catch(error => {
        exitAndRedirect()
    });
}

checkIsTokenValid();