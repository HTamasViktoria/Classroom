import React, { useEffect } from 'react';
import { useProfile } from "../../contexts/ProfileContext.jsx";

function GetNewNotifsNum({ studentId, onLength, refreshNeeded }) {
    const { profile, logout } = useProfile();

  
    let id = profile?.id;
    if (!id) {
        id = localStorage.getItem('id');
    }

    useEffect(() => {
        if (id) {
            fetch(`/api/notifications/newnotifsnum/${studentId}/${id}`)
                .then(response => response.json())
                .then(data => onLength(data))
                .catch(error => console.error(error));
        }
    }, [studentId, id, refreshNeeded]);

    return null;
}

export default GetNewNotifsNum;
