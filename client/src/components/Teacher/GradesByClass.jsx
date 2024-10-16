import React, { useState, useEffect } from "react";
import {
    ListItem,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
    Typography
} from "@mui/material";
import {
    PopupContainer, StyledButton,
    StyledTableCell,
    StyledTableHead,
} from "../../../StyledComponents.js";
import GradesTableByClassBySubject from "./GradesTableByClassBySubject.jsx";

function GradesByClass({ classId, onGoBack }) {
    const [grades, setGrades] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [students, setStudents] = useState([])

    
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
        fetch(`/api/classes/getStudents/${classId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then(data => {
                setStudents(data);
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

    const goBackHandler=()=>{
        onGoBack()
    }
 
    return (
        <div>
            {subjects.map((subject, index) => (
                <div key={index}>
                    {subject}
                    <GradesTableByClassBySubject students={students}
                                                 grades={grades.filter(grade => grade.subject === subject)}/>
                </div>
            ))}
       <StyledButton onClick={goBackHandler}>Vissza</StyledButton>
        </div>
    );
}

export default GradesByClass;
