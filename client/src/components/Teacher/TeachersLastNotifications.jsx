import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import TeacherNavbar from "./TeacherNavbar.jsx";
import {AButton} from '../../../StyledComponents.js'

function TeachersLastNotifications() {
    const { id } = useParams();
    const navigate = useNavigate();
    
    const [lastNotifications, setLastNotifications] = useState([]);

    useEffect(() => {
        fetch(`/api/notifications/teacherslasts/${id}`)
            .then(response => response.json())
            .then(data => {
                setLastNotifications(data);
            })
            .catch(error => console.error("Error fetching data:", error));
    }, [id]);

    return (
        <>
            <TeacherNavbar id={id}/>
        <div>
            <h1>Tájékoztató az elküldött értesítések megtekintéseiről</h1>
            <table border="1">
                <thead>
                <tr>
                    <th>Type</th>
                    <th>Date</th>
                    <th>Due Date</th>
                    <th>Student Name</th>
                    <th>Parent Name</th>
                    <th>Description</th>
                    <th>Optional Description</th>
                    <th>Subject Name</th>
                    <th>Read</th>
                    <th>Officially Read</th>
                </tr>
                </thead>
                <tbody>
                {lastNotifications.map((notification, index) => (
                    <tr key={index}>
                        <td>{notification.type}</td>
                        <td>{new Date(notification.date).toLocaleString()}</td>
                        <td>{new Date(notification.dueDate).toLocaleString()}</td>
                        <td>{notification.studentName}</td>
                        <td>{notification.parentName}</td>
                        <td>{notification.description}</td>
                        <td>{notification.optionalDescription || "N/A"}</td>
                        <td>{notification.subjectName}</td>
                        <td>{notification.read ? "Yes" : "No"}</td>
                        <td>{notification.officiallyRead ? "Yes" : "No"}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
            <AButton onClick={()=>navigate(`/teacher/notifications/${id}`)}>Vissza</AButton>
        </>
    );
}

export default TeachersLastNotifications;
