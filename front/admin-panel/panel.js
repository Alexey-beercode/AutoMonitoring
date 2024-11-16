const apiBaseUrl = 'http://gtstream.net/api/admin/user';

async function displayUsers() {
    const userList = document.getElementById('users');
    userList.innerHTML = ''; // Очищаем список перед добавлением новых пользователей

    const token = localStorage.getItem('accessToken');

    try {
        const response = await fetch(`${apiBaseUrl}/getAll`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.status} ${response.statusText}`);
        }

        const users = await response.json();

        users.forEach(user => {
            const userRow = document.createElement('tr');
            userRow.className = user.isBlocked ? 'blocked-user' : '';

            userRow.innerHTML = `
                <td>${user.id}</td>
                <td>${user.login}</td>
                <td>${user.password}</td>
                <td class="${user.isBlocked ? 'user-blocked' : (user.isActive ? 'status-active' : 'status-inactive')}">
                    ${user.isBlocked ? 'Заблокирован' : (user.isActive ? 'Активен' : 'Неактивен')}
                </td>
                <td>${user.deviceName ? user.deviceName : 'Неизвестно'}</td>
                <td>${new Date(user.lastActive).toLocaleString()}</td>
                <td>
                    <button class="${user.isBlocked ? '' : 'btn-blocked'}" onclick="blockUser('${user.id}', ${user.isBlocked})">
                        ${user.isBlocked ? 'Разблокировать' : 'Блокировать'}
                    </button>
                    <button onclick="deleteUser('${user.login}')">Удалить</button>
                </td>
            `;
            userList.appendChild(userRow); // Добавляем строку в таблицу
        });
    } catch (error) {
        console.error('Ошибка при получении пользователей:', error);
        alert('Ошибка при получении пользователей. Проверьте консоль для подробностей.');
    }
}

window.onload = function() {
    displayUsers();
};

document.getElementById('createUserForm').addEventListener('submit', async function(event) {
    event.preventDefault();

    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');
    const username = usernameInput.value;
    const password = passwordInput.value;

    const token = localStorage.getItem('accessToken');

    if (!username || !password) {
        alert('Пожалуйста, заполните все поля!');
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({
                login: username,
                password: password
            })
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.status} ${response.statusText}`);
        }

        usernameInput.value = '';
        passwordInput.value = '';

        await displayUsers();
    } catch (error) {
        console.error('Ошибка при создании пользователя:', error);
        alert('Не удалось создать пользователя. Проверьте консоль для получения подробной информации.');
    }
});

async function blockUser(userId, isBlocked) {
    const token = localStorage.getItem('accessToken');
    let url = `${apiBaseUrl}/block`;
    let method = 'PUT';

    if (isBlocked) {

        url = `${apiBaseUrl}/unblock/${userId}`;
    } else {

        const blockUntil = new Date();
        blockUntil.setHours(blockUntil.getHours() + 1);


        const bodyData = {
            blockUntil: blockUntil.toISOString(),
            userId: userId
        };

        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(bodyData)
            });

            if (!response.ok) {
                throw new Error(`Ошибка: ${response.status} ${response.statusText}`);
            }
        } catch (error) {
            console.error('Ошибка при блокировке пользователя:', error);
            alert('Не удалось заблокировать пользователя. Проверьте консоль для получения подробной информации.');
        }
    }

    if (isBlocked) {
        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error(`Ошибка: ${response.status} ${response.statusText}`);
            }
        } catch (error) {
            console.error('Ошибка при разблокировке пользователя:', error);
            alert('Не удалось разблокировать пользователя. Проверьте консоль для получения подробной информации.');
        }
    }

    await displayUsers();
}

async function deleteUser(login) {
    const token = localStorage.getItem('accessToken');

    try {
        const response = await fetch(`${apiBaseUrl}/delete/${login}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error(`Ошибка: ${response.status} ${response.statusText}`);
        }

        alert(`Пользователь ${login} успешно удалён.`);
        await displayUsers();
    } catch (error) {
        console.error('Ошибка при удалении пользователя:', error);
        alert('Не удалось удалить пользователя. Проверьте консоль для получения подробной информации.');
    }
}
