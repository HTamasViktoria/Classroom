// src/components/Starter.js
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import ParentNavbar from "../components/Parent/ParentNavbar";
import ParentNotifications from "../components/Parent/ParentNotifications";
import ParentGrades from "../components/Parent/ParentGrades";
import ParentMain from "../components/Parent/ParentMain";

function Starter() {
    const { id } = useParams();
    const [student, setStudent] = useState(null);
    const [notifications, setNotifications] = useState([]);
    const [refreshNeeded, setRefreshNeeded] = useState(false);
    const [lastNotifications, setLastNotifications] = useState([]);

    useEffect(() => {
        fetch(`/api/students/${id}`)
            .then(response => response.json())
            .then(data => {
                setStudent(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    useEffect(() => {
        fetch(`/api/notifications/byStudentId/${id}`)
            .then(response => response.json())
            .then(data => {
                setNotifications(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id, refreshNeeded]);

    useEffect(() => {
        fetch(`/api/notifications/lastsByStudentId/${id}`)
            .then(response => response.json())
            .then(data => {
                setLastNotifications(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id, refreshNeeded]);

    if (!student) {
        return <h1>Loading...</h1>;
    }

    const refreshHandler = () => {
        setRefreshNeeded((prevState) => !prevState);
    };

    return (
        <>
            <ParentNavbar studentId={student.id} notifications={notifications} />
            <ParentMain lastNotifications={lastNotifications} studentId={student.id} onRefreshNeeded={refreshHandler} />
        </>
    );
}

export default Starter;
