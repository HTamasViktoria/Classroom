import { useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import ParentNavbar from "../components/Parent/ParentNavbar.jsx";
import ParentNotifications from "../components/Parent/ParentNotifications.jsx";

function Starter() {
    const { id } = useParams();
    const [student, setStudent] = useState(null);
    const [chosen, setChosen] = useState("");

    useEffect(() => {
        fetch(`/api/students/${id}`)
            .then(response => response.json())
            .then(data => 
                setStudent(data)
            )
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    const navbarHandler = (chosen) => setChosen(chosen);

    if (!student) {
        return <h1>Loading...</h1>;
    }

    return (
        <>
            <ParentNavbar student={student} onChosen={navbarHandler} />
            <h1>Hello, {student.firstName} szülője</h1>
            {chosen == "notifications" && <ParentNotifications student={student}/>}
        </>
    );
}

export default Starter;
