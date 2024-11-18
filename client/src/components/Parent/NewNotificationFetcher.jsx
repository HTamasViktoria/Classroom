import React, { useEffect } from 'react';
import {useProfile} from "../../contexts/ProfileContext.jsx";
function NewNotificationFetcher({onData,studentId, refreshNeeded}){


    const {profile, logout} = useProfile();


    let id = profile?.id;
    if (!id) {
        id = localStorage.getItem('id');
    }
    
    useEffect(() => {
        fetch(`/api/notifications/newNotifsByStudentId/${studentId}/${id}`)
            .then(response => response.json())
            .then(data => onData(data))
            .catch(error => console.error(error));
    }, [studentId, refreshNeeded]);


    return null;
}

export default NewNotificationFetcher