import {Paper, Table, TableBody, TableCell, TableContainer, TableRow, Typography} from "@mui/material";
import {Cell, PopupContainer, TableHeading, StyledHeading} from "../../../StyledComponents.js";
import MonthSelector from "../Parent/MonthSelector.jsx";
import React, {useState} from "react";

function BySubjByClassTableCont({className, subject, studentsOfClass, grades}){

    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({ top: 0, left: 0 });
    const [chosenMonthIndex, setChosenMonthIndex] = useState(new Date().getMonth());
    
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


    return(<>
        <PopupContainer
        id="popUp"
        top={popupPosition.top}
        left={popupPosition.left}
        visible={!!hoverDate && !!hoverForWhat}>
        <span>{hoverForWhat}</span>
        <span>{hoverDate}</span>
    </PopupContainer>
        
        <TableContainer>
        <StyledHeading>
            {className} osztály jegyei {subject} tantárgyból
        </StyledHeading>

        <Table>
            <TableHeading>
                <TableRow>
                    <Cell>Név</Cell>
                    <Cell>Jegyek</Cell>
                    <Cell>
                        <MonthSelector onMonthChange={setChosenMonthIndex} />
                    </Cell>
                </TableRow>
            </TableHeading>
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
    </>)
}

export default BySubjByClassTableCont