import { AButton } from "../../../StyledComponents.js";
import React from "react";

function NotifDuoButton({ notification, onRefresh, onGoBack }) {
    const setToReadHandler = (e) => {
        const id = e.target.id;

        fetch(`/api/notifications/setToRead/${id}`, { method: 'POST' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to set notification to read');
                }
                return response.json();
            })
            .then(() => {
                onRefresh();
            })
            .catch(error => console.error('Error setting notification to read:', error));
    };

    return (
        <>
            {notification.read ? (
                <AButton
                    onClick={setToReadHandler}
                    id={notification.id}
                    variant="contained"
                    color="secondary"
                    size="small">
                    Olvasatlannak jelöl
                </AButton>
            ) : (
                <AButton
                    onClick={onGoBack}                    
                    id={notification.id}
                    variant="contained"
                    color="secondary"
                    size="small">
                    Erre még visszatérek
                </AButton>
            )}
        </>
    );
}

export default NotifDuoButton;
