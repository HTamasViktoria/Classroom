import React, { useEffect, useState } from 'react';
import NotificationByTypeFetcher from "./NotificationByTypeFetcher.jsx";
import NotificationsByTypeTable from "./NotificationsByTypeTable.jsx";

function GeneralNotifications({ chosen, onRefreshing, onGoBack, id, refreshNeeded }) {
    
 
    const [notifications, setNotifications] = useState([]);

    useEffect(() => {
        fetch(`/api/notifications/setToOfficiallyRead/${id}`, { method: 'POST' })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('Response data:', data);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }, [id]);

    return (
        <>
            <NotificationByTypeFetcher onData={(data) => setNotifications(data)} chosen={chosen} onRefreshNeeded={refreshNeeded} />
            <NotificationsByTypeTable notifications={notifications} onRefresh={onRefreshing} onGoBack={onGoBack}/>
        </>
    );
}

export default GeneralNotifications;
