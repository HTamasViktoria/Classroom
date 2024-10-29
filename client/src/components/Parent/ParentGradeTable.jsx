import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    Typography,
} from "@mui/material";
import {
    PopupContainer,
    StyledButton,
    StyledTableCell,
    StyledTableHead,
} from "../../../StyledComponents.js";
import React, {useEffect, useState} from "react";
import ParentStatistics from "./ParentStatistics.jsx";
import MonthSelector from "./MonthSelector.jsx";
import EditGrades from "../Teacher/EditGrades.jsx";
import GradeTable from "./GradeTable.jsx";

function ParentGradeTable({ grades, subjects, id, isEditable, teacherId, teacherSubjects, nameOfClass, studentName, onGoBack, onRefresh }) {
    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({ top: 0, left: 0 });
    const [chosenMonthIndex, setChosenMonthIndex] = useState(new Date().getMonth());
    const [isEditing, setIsEditing] = useState(false);
    const [chosenSubject, setChosenSubject] = useState("")
    //const [gradesToEdit, setGradesToEdit] = useState([]);
    const [classAverages, setClassAverages] = useState([]);
    

    useEffect(() => {fetch(`/api/grades/class-averages/${id}`)
            .then(response => response.json())
            .then(data => {
                setClassAverages(data);
            })
            .catch(error => console.error(`Error fetching data:`, error));
    }, [id]);

    const startHover = (grade, element) => {
        const date = new Date(grade.date).toLocaleDateString();
        setHoverDate(date);
        setHoverForWhat(grade.forWhat);
        const rect = element.getBoundingClientRect();
        setPopupPosition({
            top: rect.top + window.scrollY - 40,
            left: rect.left + window.scrollX + rect.width + 10,
        });
    };

    const finishHover = () => {
        setHoverDate("");
        setHoverForWhat("");
    };
    
    

    const filterGradesByMonth = (subject) => {
        return grades.filter((grade) => {
            const gradeDate = new Date(grade.date);
            const gradeMonthIndex = gradeDate.getMonth();
            return grade.subject === subject && gradeMonthIndex === chosenMonthIndex;
        });
    };

    const editHandler = (e) => {
        let chosenEditSubject = e.target.id;
        setChosenSubject(chosenEditSubject);
      /*  let gradesEditing = grades.filter((grade) => grade.subject === chosenSubject);

        gradesEditing.sort((a, b) => new Date(b.date) - new Date(a.date));

        setGradesToEdit(gradesEditing);*/
        setIsEditing(true);
    };

    const notEditingHandler = () => {
        setIsEditing(false);
    };

    const goBackHandler = () => {
        onGoBack();
    };
    
    const refreshHandler=()=>{
    onRefresh()
    }

    return (
        <>
            {isEditing ? (
                <EditGrades onGoBack={notEditingHandler} 
                            teacherId={teacherId} 
                            //grades={gradesToEdit} 
                    subject={chosenSubject}
                            studentId={id} 
                            studentName={studentName}
                            onRefresh={refreshHandler}/>
            ) : (
                <>
                    <GradeTable
                        subjects={subjects}
                        filterGradesByMonth={filterGradesByMonth}
                        startHover={startHover}
                        finishHover={finishHover}
                        popupPosition={popupPosition}
                        hoverDate={hoverDate}
                        hoverForWhat={hoverForWhat}
                        id={id}
                        grades={grades}
                        chosenMonthIndex={chosenMonthIndex}
                        classAverages={classAverages}
                        isEditable={isEditable}
                        teacherSubjects={teacherSubjects}
                        nameOfClass={nameOfClass}
                        editHandler={editHandler}
                        setChosenMonthIndex={setChosenMonthIndex}
                    />
                    <StyledButton onClick={goBackHandler}>Vissza</StyledButton>
                </>
            )}
        </>
    );
}

export default ParentGradeTable;
