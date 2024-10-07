
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import ParentNavbar from "../components/Parent/ParentNavbar";
import ParentMain from "../components/Parent/ParentMain";

function Starter() {
    const { id } = useParams();
    const [student, setStudent] = useState(null);
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
            <ParentNavbar studentId={student.id} refreshNeeded={refreshNeeded} />
            <ParentMain lastNotifications={lastNotifications} studentId={student.id} onRefreshNeeded={refreshHandler} />
        </>
    );
}

export default Starter;
