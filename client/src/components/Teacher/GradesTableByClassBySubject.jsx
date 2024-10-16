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
import MonthSelector from "../Parent/MonthSelector.jsx";


function GradesTableByClassBySubject({grades, students}) {


    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({ top: 0, left: 0 });
    const [chosenMonthIndex, setChosenMonthIndex] = useState(new Date().getMonth());

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



    const filterGradesByMonth = (studentId) => {
        return grades.filter((grade) => {
            const gradeDate = new Date(grade.date);
            const gradeMonthIndex = gradeDate.getMonth();
            return grade.studentId === studentId && gradeMonthIndex === chosenMonthIndex;
        });
    };
    
    const renderGrades = (id) => {
        const filteredGrades = filterGradesByMonth(id)
        return (
            <>
                {filteredGrades.map((grade) => (
                    <span
                        onMouseEnter={(e) => startHover(grade, e.currentTarget)}
                        onMouseLeave={finishHover}
                        key={grade.id}
                        className="grade"
                        style={{ margin: "0 10px" }}
                    >
                    {grade.value}
                </span>
                ))}
            </>
        );
    };


    return (<>
        <TableContainer>
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
                <StyledTableCell>Osztályzat</StyledTableCell>
                <StyledTableCell>
                    <MonthSelector onMonthChange={setChosenMonthIndex}/>
                </StyledTableCell>
            </TableRow>
            </StyledTableHead>
                <TableBody>
                    {students.map((student) => <TableRow key={student.id}>
                    <TableCell>{student.familyName} {student.firstName}</TableCell>
                    <TableCell>{renderGrades(student.id)}</TableCell>
                </TableRow>)}

                </TableBody>
            </Table>
        </TableContainer>

    </>)
}
export default GradesTableByClassBySubject