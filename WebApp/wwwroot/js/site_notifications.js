document.addEventListener('DOMContentLoaded', function () {
    const bellButton = document.querySelector('[data-bs-target="#notificationsPanel"]');

    fetch(`/api/notifications`)
        .then(response => response.json())
        .then(data => {
            const hasUnread = data.some(n => n.isRead === false);
            const bellIcon = document.getElementById('navbarBellIcon');
            if (hasUnread && bellIcon) {
                bellIcon.classList.remove('bi-bell');
                bellIcon.classList.add('bi-bell-fill', 'text-success');
            }
        })
        .catch(error => {
            console.error('Error checking unread notifications:', error);
        });


    if (bellButton) {
        bellButton.addEventListener('click', () => {
            fetch(`/api/notifications`)
                .then(response => response.json())
                .then(data => {
                    const container = document.getElementById('notificationList');
                    container.innerHTML = ''; // clear old items

                    if (data.length === 0) {
                        container.innerHTML = '<div class="text-muted">No notifications found.</div>';
                        return;
                    }

                    data.forEach(notification => {
                        const div = document.createElement('div');
                        div.classList.add('notification-item', 'mb-4');
                        div.setAttribute('data-id', notification.id);

                        const textClass = notification.isRead === false ? 'text-success fw-bold' : 'text-muted';
                        const iconClass = notification.isRead === false ? 'bi-bell-fill text-success' : 'bi-bell text-muted';

                        div.innerHTML = `
                            <small class="text-muted">${new Date(notification.createdAt).toLocaleString()}</small>
                            <p class="${textClass}" style="font-size: 1.25rem;">
                                <i class="bi ${iconClass}" style="font-size: 1.5rem; margin-right: 6px;"></i>
                                ${notification.notificationType.typeName || 'General'}
                            </p>
                        `;

                        // When a notification is clicked
                        // When a notification is clicked
                        div.addEventListener('click', () => {
                            showNotificationDetails(notification);

                            // Mark as read
                            fetch(`/api/notifications/markread/${notification.id}`, { method: 'POST' })
                                .then(() => {
                                    div.querySelector('i').className = 'bi bi-bell text-muted';
                                    const p = div.querySelector('p');
                                    p.className = 'text-muted';
                                    p.style.fontWeight = 'normal';

                                    // ✅ Check again if there are any unread notifications
                                    return fetch(`/api/notifications`);
                                })
                                .then(response => response.json())
                                .then(allNotifications => {
                                    const bellIcon = document.getElementById('navbarBellIcon');
                                    const hasUnread = allNotifications.some(n => n.isRead === false);

                                    if (bellIcon) {
                                        if (hasUnread) {
                                            bellIcon.classList.remove('bi-bell');
                                            bellIcon.classList.add('bi-bell-fill', 'text-success');
                                        } else {
                                            bellIcon.classList.remove('bi-bell-fill', 'text-success');
                                            bellIcon.classList.add('bi-bell');
                                        }
                                    }
                                });
                        });

                        container.appendChild(div);
                    });
                })
                .catch(error => {
                    document.getElementById('notificationList').innerHTML = '<div class="text-danger">Failed to load notifications.</div>';
                    console.error('Error fetching notifications:', error);
                });
        });
    }

    // Back button in details
    document.getElementById('backToList').addEventListener('click', function () {
        document.getElementById('notificationDetails').style.display = 'none';
        document.getElementById('notificationList').style.display = 'block';
    });
});

// Function to show notification details
function showNotificationDetails(notification) {
    document.getElementById('notificationTitle').innerText = "Notification #" + notification.id;
    document.getElementById('notificationMessage').innerText = notification.messageContent;
    document.getElementById('notificationList').style.display = 'none';
    document.getElementById('notificationDetails').style.display = 'block';
}



///Login show password

const passwordInput = document.getElementById("passwordInput");
const toggleIcon = document.getElementById("toggleIcon");

toggleIcon.addEventListener("click", function () {
    const isPassword = passwordInput.type === "password";
    passwordInput.type = isPassword ? "text" : "password";
    toggleIcon.classList.toggle("bi-eye");
    toggleIcon.classList.toggle("bi-eye-slash");
});

