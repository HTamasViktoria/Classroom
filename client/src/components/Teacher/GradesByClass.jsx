import React, { useState, useEffect } from "react";
import {AButton} from "../../../StyledComponents.js";
import GradesTableByClassBySubject from "./GradesTableByClassBySubject.jsx";

function GradesByClass({ classId, onGoBack }) {
    
    
    const [grades, setGrades] = useState([]);
    const [subjects, setSubjects] = useState([]);
  

    
    useEffect(() => {
        fetch(`/api/grades/getGradesByClass/${classId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then(data => {
                setGrades(data);
            })
            .catch(error => console.error(`Error:`, error));
    }, [classId]);



    
    useEffect(() => {
        if (grades.length > 0) {
            const subjectsArray = grades.map(grade => grade.subject);
            const uniqueSubjectsArray = [...new Set(subjectsArray)];
            setSubjects(uniqueSubjectsArray);
        }
    }, [grades]);

 
 
    return (
        <div>
            {subjects.map((subject, index) => (
                <div key={index}>
                    {subject}
                    <GradesTableByClassBySubject
                        grades={grades.filter(grade => grade.subject === subject)}
                    classId={classId}
                    />
                </div>
            ))}
       <AButton onClick={()=>onGoBack()}>Vissza</AButton>
        </div>
    );
}

export default GradesByClass;
