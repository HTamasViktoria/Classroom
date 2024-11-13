import React, { useEffect } from 'react';

function NewNotificationFetcher({onData,studentId, refreshNeeded}){




    useEffect(() => {
        fetch(`/api/notifications/newNotifsByStudentId/${studentId}`)
            .then(response => response.json())
            .then(data => onData(data))
            .catch(error => console.error(error));
    }, [studentId, refreshNeeded]);


    return null;
}

export default NewNotificationFetcher