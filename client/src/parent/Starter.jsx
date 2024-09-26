import { useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import ParentNavbar from "../components/Parent/ParentNavbar.jsx";
import ParentNotifications from "../components/Parent/ParentNotifications.jsx";
import ParentGrades from "../components/Parent/ParentGrades.jsx";
import ParentMain from "../components/Parent/ParentMain.jsx";

function Starter() {
    const { id } = useParams();
    const [student, setStudent] = useState(null);
    const [notifications, setNotifications] = useState([]);
    const [resfreshNeeded, setRefreshNeeded] = useState(false);
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
    }, [id, resfreshNeeded]);

    
    useEffect(() => {
        fetch(`/api/notifications/lastsByStudentId/${id}`)
            .then(response => response.json())
            .then(data => {
                setLastNotifications(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id, resfreshNeeded]);

    if (!student) {
        return <h1>Loading...</h1>;
    }

    const refreshHandler = () => {
        setRefreshNeeded((prevState) => !prevState);
    };

    return (
        <>
            <ParentNavbar studentId={student.id} notifications={notifications} />
            {/* 
            {chosen === "notifications" && <ParentNotifications onRefreshing={refreshHandler} student={student} notifications={notifications} />}
            {chosen === "grades" && <ParentGrades student={student} />}
            */}
            <ParentMain lastNotifications={lastNotifications} studentId={student.id} onRefreshNeeded={refreshHandler} />
        </>
    );
}

export default Starter;
