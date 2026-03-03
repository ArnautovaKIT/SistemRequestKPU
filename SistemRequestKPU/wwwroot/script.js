let token = localStorage.getItem('token');
const apiUrl = 'https://localhost:7249/api'; // Ваш порт API, скорректируйте если нужно

if (!token) {
    window.location.href = 'login.html'; // Перенаправление на страницу авторизации, если токен отсутствует
}

let sortField = 'CreatedAt';
let isAscending = true;

async function loadSpictionaries() {
    // Загрузка оборудования
    const equipmentsResponse = await fetch(`${apiUrl}/equipments`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (equipmentsResponse.ok) {
        const equipments = await equipmentsResponse.json();
        const equipmentSelect = document.getElementById('equipmentId');
        equipmentSelect.innerHTML = '<option value="" disabled selected>Выберите оборудование</option>';
        equipments.forEach(eq => {
            equipmentSelect.innerHTML += `<option value="${eq.id}">${eq.name}</option>`;
        });
    } else {
        alert('Ошибка загрузки оборудования');
    }

    // Загрузка объектов
    const objectsResponse = await fetch(`${apiUrl}/objects`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (objectsResponse.ok) {
        const objects = await objectsResponse.json();
        const objectSelect = document.getElementById('objectId');
        objectSelect.innerHTML = '<option value="" disabled selected>Выберите объект</option>';
        objects.forEach(obj => {
            objectSelect.innerHTML += `<option value="${obj.id}">${obj.name}</option>`;
        });
    } else {
        alert('Ошибка загрузки объектов');
    }
}

function showCreateForm() {
    document.getElementById('createForm').style.display = 'block';
}

document.getElementById('createForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const request = {
        workType: document.getElementById('workType').value,
        priority: document.getElementById('priority').value,
        technicalSpecs: document.getElementById('technicalSpecs').value,
        requirements: document.getElementById('requirements').value,
        equipmentId: parseInt(document.getElementById('equipmentId').value),
        objectId: parseInt(document.getElementById('objectId').value)
    };
    const response = await fetch(`${apiUrl}/requests`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    });
    if (response.ok) {
        fetchRequests();
        document.getElementById('createForm').style.display = 'none';
        alert('Заявка создана!');
    } else {
        alert('Ошибка создания заявки');
    }
});

async function fetchRequests() {
    const search = document.getElementById('searchInput').value;
    const workType = document.getElementById('workTypeFilter').value;
    const status = document.getElementById('statusFilter').value;
    const url = `${apiUrl}/requests?search=${search}&workType=${workType}&status=${status}&sortBy=${sortField}&ascending=${isAscending}`;
    const response = await fetch(url, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (response.ok) {
        const data = await response.json();
        const tbody = document.querySelector('#requestsTable tbody');
        tbody.innerHTML = '';
        data.forEach(r => {
            const row = `<tr>
                <td>${r.uniqueNumber}</td>
                <td>${r.workType}</td>
                <td>${r.status}</td>
                <td>${new Date(r.createdAt).toLocaleDateString()}</td>
                <td>
                    <button onclick="updateStatus(${r.id}, 'InProgress')">В работу</button>
                    <button onclick="updateStatus(${r.id}, 'Completed')">Выполнено</button>
                    <button onclick="editRequest(${r.id})">Редактировать</button>
                </td>
            </tr>`;
            tbody.innerHTML += row;
        });
    } else {
        alert('Ошибка загрузки заявок');
    }
}

async function updateStatus(id, newStatus) {
    const response = await fetch(`${apiUrl}/requests/${id}/status`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newStatus)
    });
    if (response.ok) {
        fetchRequests();
    } else {
        alert('Ошибка обновления статуса');
    }
}

function editRequest(id) {
    // Для простоты: можно добавить модальную форму или переиспользовать createForm с загрузкой данных
    alert(`Редактирование заявки ${id} - реализуйте логику здесь`);
}

function sortBy(field) {
    sortField = field;
    isAscending = !isAscending;
    fetchRequests();
}

// Инициализация
loadSpictionaries();
fetchRequests();