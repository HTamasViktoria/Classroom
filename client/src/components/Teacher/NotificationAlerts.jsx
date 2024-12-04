import React, { useEffect, useState } from 'react';

function NotificationAlerts({ teacher }) {
    
    const [notifications, setNotifications] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

  
    useEffect(() => {
        const fetchNotifications = async () => {
            try {
             
                const response = await fetch(`/api/notifications/teacher/${teacher.id}`);

                if (!response.ok) {
                    throw new Error('Hiba történt az értesítések lekérése során');
                }

                const data = await response.json();
                setNotifications(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchNotifications();
    }, [teacher.id]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div>
            <h2>Tanárhoz tartozó értesítések</h2>
            {notifications.length === 0 ? (
                <p>Nincs új értesítés.</p>
            ) : (
                <ul>
                    {notifications.map((notification, index) => (
                        <li key={index}>
                            <div><strong>Tantárgy:</strong> {notification.subject}</div>
                            <div><strong>Dátum:</strong> {notification.date}</div>
                            <div><strong>Gyermek neve:</strong> {notification.studentName}</div>
                            <div><strong>Állapot:</strong> {notification.officiallyRead ? 'Megnézve' : 'Nem nézték meg'}</div>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default NotificationAlerts;
