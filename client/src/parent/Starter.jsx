import { useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import ParentNavbar from "../components/Parent/ParentNavbar.jsx";
import ParentNotifications from "../components/Parent/ParentNotifications.jsx";
import ParentGrades from "../components/Parent/ParentGrades.jsx";

function Starter() {
    const { id } = useParams();
    const [student, setStudent] = useState(null);
    const [notifications, setNotifications] = useState([]);
    const [chosen, setChosen] = useState("");
    
    useEffect(() => {
        fetch(`/api/students/${id}`)
            .then(response => response.json())
            .then(data => 
                setStudent(data)
            )
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    useEffect(() => {
        fetch(`/api/notifications/byStudentId/${id}`)
            .then(response => response.json())
            .then(data => setNotifications(data))
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    const navbarHandler = (chosen) => {
        
        setChosen(chosen)
    };

    if (!student) {
        return <h1>Loading...</h1>;
    }

    return (
        <>
            <ParentNavbar student={student} onChosen={navbarHandler} notifications={notifications} />
            <h1>Hello, {student.firstName} szülője</h1>
            {chosen === "notifications" && <ParentNotifications student={student} notifications={notifications}/>}
            {chosen === "grades" && <ParentGrades student={student}/> }
        </>
    );
}

export default Starter;
