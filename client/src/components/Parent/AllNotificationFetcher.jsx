import React, { useEffect } from 'react';

function AllNotificationFetcher({ id, onData, refreshNeeded }) {
    
    useEffect(() => {
        fetch(`/api/notifications/byStudentId/${id}`)
            .then(response => response.json())
            .then(data => {
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
