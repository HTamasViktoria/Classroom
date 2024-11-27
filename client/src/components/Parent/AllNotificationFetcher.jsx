import React, { useEffect } from 'react';
import {useProfile} from "../../contexts/ProfileContext.jsx";
function AllNotificationFetcher({ studentId, onData, refreshNeeded }) {

    const {profile, logout} = useProfile();

let id;
    if (!id) {
        id = localStorage.getItem('id');
    }
    
    useEffect(() => {
        fetch(`/api/notifications/byStudent/byParent/${studentId}/${id}`)
            .then(response => response.json())
            .then(data => {
                console.log(data)
                onData(categorizeNotifications(data));
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id, refreshNeeded]);

    const categorizeNotifications = (notifications) => {
        const categorize = (type) => notifications.filter(n => n.type === type && n.read === false);

        return {
            exams: categorize("Exam"),
            homeworks: categorize("Homework"),
            missingEquipments: categorize("MissingEquipment"),
            others: categorize("OtherNotifications")
        };
    };

    return null;
}

export default AllNotificationFetcher;
