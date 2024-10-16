import React, { useState, useEffect } from "react";
import {
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
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos.js";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos.js";
import MonthSelector from "../Parent/MonthSelector.jsx";

function GradeDetailsBySubjectByClass({ subject, classId, className, onGoBack}) {
    const [studentsOfClass, setStudentsOfClass] = useState([]);
    const [grades, setGrades] = useState([]);
    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({ top: 0, left: 0 });
    const [chosenMonthIndex, setChosenMonthIndex] = useState(new Date().getMonth());
    




    useEffect(() => {
        fetch(`/api/classes/getStudents/${classId}`)
            .then(response => response.json())
            .then(data => {
                setStudentsOfClass(data);
            })
            .catch(error => console.error(`Error:`, error));
    }, [classId]);

    useEffect(() => {
        fetch(`/api/grades/${classId}/${subject}`)
            .then(response => response.json())
            .then(data => {
                setGrades(data);
            })
            .catch(error => console.error(`Error:`, error));
    }, [classId, subject]);


   
    const filterGradesByMonth = (studentId) => {
        return grades.filter((grade) => {
            const gradeDate = new Date(grade.date);
            const gradeMonthIndex = gradeDate.getMonth();
            return grade.studentId === studentId && gradeMonthIndex === chosenMonthIndex;
        });
    };

    const startHover = (grade, element) => {
        const date = new Date(grade.date).toLocaleDateString();
        setHoverDate(date);
        setHoverForWhat(grade.forWhat);
        const rect = element.getBoundingClientRect();
        setPopupPosition({
            top: rect.top + window.scrollY - 40,
            left: rect.left + window.scrollX + rect.width + 10
        });
    };

    const finishHover = () => {
        setHoverDate("");
        setHoverForWhat("");
    };
    
    const goBackHandler=()=>{
        onGoBack()
    }
 

    return (
        <>
        <TableContainer component={Paper}>
            <Typography variant="h6" component="div" sx={{ margin: 2 }}>
                {className} osztály jegyei {subject} tantárgyból
            </Typography>
            <PopupContainer
                id="popUp"
                top={popupPosition.top}
                left={popupPosition.left}
                visible={!!hoverDate && !!hoverForWhat}
            >
                <span>{hoverForWhat}</span>
                <span>{hoverDate}</span>
            </PopupContainer>
            <Table>
                <StyledTableHead>
                    <TableRow>
                        <StyledTableCell>Név</StyledTableCell>
                        <StyledTableCell>Jegyek</StyledTableCell>
                        <StyledTableCell>
                            <MonthSelector onMonthChange={setChosenMonthIndex} />
                        </StyledTableCell>
                    </TableRow>
                </StyledTableHead>
                <TableBody>
                    {studentsOfClass.map((student, index) => (
                        <TableRow key={student.id}>
                            <TableCell>{student.familyName} {student.firstName}</TableCell>
                            <TableCell>
                                {filterGradesByMonth(student.id).map((grade) => (
                                    <span
                                        key={grade.id}
                                        onMouseEnter={(e) => startHover(grade, e.currentTarget)}
                                        onMouseLeave={finishHover}
                                        className="grade"
                                        style={{ margin: '0 10px' }}
                                    >
                                        {grade.value}
                                    </span>
                                ))}
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
        <StyledButton onClick={goBackHandler}>Vissza</StyledButton>
        </>
    );
}

export default GradeDetailsBySubjectByClass;
