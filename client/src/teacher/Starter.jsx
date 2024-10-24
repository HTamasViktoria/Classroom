import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";
import Tasks from "../components/Teacher/Tasks.jsx";


function Starter() {
    const { id } = useParams();
    const [selectedTeacher, setSelectedTeacher] = useState(null);
    

    useEffect(() => {
        fetch(`/api/teachers/${id}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log(data);
                setSelectedTeacher(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    return (
        <>
            {selectedTeacher &&  ( 
                <>
                    <TeacherNavbar />
                    <Tasks teacherId={id} teacherName={`${selectedTeacher.familyName} ${selectedTeacher.firstName}`} />
                </>
            )}
        </>
    );
}

export default Starter;
