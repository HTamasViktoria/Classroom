import {Paper, Table, TableBody, TableCell, TableContainer, Typography} from "@mui/material";
import {PopupContainer, StatisticsContainer, StyledTableCell, StyledTableHead} from "../../../StyledComponents.js";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos.js";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos.js";
import React, {useEffect, useState} from "react";
import {getAverageBySubjectFullYear, getDifference} from "../../../GradeCalculations.js";
import ParentStatistics from "./ParentStatistics.jsx";

function ParentGradeTable({grades, subjects, id}){

    const [chosenMonth, setChosenMonth] = useState("");
    const [chosenMonthIndex, setChosenMonthIndex] = useState(0);
    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({ top: 0, left: 0 });

    useEffect(() => {
        const currentMonthIndex = new Date().getMonth();
        setChosenMonth(monthNames[currentMonthIndex]);
        setChosenMonthIndex(currentMonthIndex);
    }, []);


    const monthNames = [
        "Január", "Február", "Március", "Április", "Május", "Június",
        "Július", "Augusztus", "Szeptember", "Október", "November", "December"
    ];

    const schoolMonths = [
        "Szeptember", "Október", "November", "December", "Január",
        "Február", "Március", "Április", "Május", "Június"
    ];

    const startHover = (grade, element) => {
        const date = new Date(grade.date).toLocaleDateString()
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


    const monthBackHandler = () => {
        const actualsIndexInSchoolMonth = schoolMonths.indexOf(chosenMonth);

        if (actualsIndexInSchoolMonth - 1 < 0) {
            console.log("Nem lehet visszafelé lapozni")
        } else {
            const nextMonthIndex = (chosenMonthIndex - 1) % monthNames.length;
            setChosenMonth(monthNames[nextMonthIndex]);
            setChosenMonthIndex(nextMonthIndex);
        }
    }

    const monthForwardHandler = () => {
        const actualsIndexInSchoolMonth = schoolMonths.indexOf(chosenMonth);

        if (actualsIndexInSchoolMonth + 1 > 9) {
            console.log("Nem lehet előre lapozni");
        } else {
            const nextMonthIndex = (chosenMonthIndex + 1) % monthNames.length;
            setChosenMonth(monthNames[nextMonthIndex]);
            setChosenMonthIndex(nextMonthIndex);
        }
    };

    const filterGradesByMonth = (subject) => {
        return grades.filter((grade) => {
            const gradeDate = new Date(grade.date);
            const gradeMonthIndex = gradeDate.getMonth();
            return grade.subject === subject && gradeMonthIndex === chosenMonthIndex;
        });
    };



    const renderGrades = (subject, index) => {
        const filteredGrades = filterGradesByMonth(subject);
        return (
            <tr key={`${subject.subject}-${index}`}>
                <TableCell>{subject}</TableCell>
                <TableCell>
                    {filteredGrades.map((grade) => (
                        <span
                            onMouseEnter={(e) => startHover(grade, e.currentTarget)}
                            onMouseLeave={finishHover}
                            key={grade.id}
                            className="grade"
                            style={{ margin: '0 10px' }}
                        >
                            {grade.value}
                        </span>
                    ))}
                </TableCell>
           <ParentStatistics id={id} grades={grades} subject={subject} chosenMonthIndex={chosenMonthIndex}/>
            </tr>
        );
    };


 
    
    return(<>

        <TableContainer component={Paper}>
            <Typography variant="h6" component="div" sx={{ margin: 2 }}>
                Osztályzatok
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
                    <tr>
                        <StyledTableCell>Tantárgy</StyledTableCell>
                        <StyledTableCell>
                            <ArrowBackIosIcon onClick={monthBackHandler} sx={{ margin: '0 10px' }} />
                            <span>{chosenMonth}</span>
                            <ArrowForwardIosIcon onClick={monthForwardHandler} sx={{ margin: '0 10px' }} />
                        </StyledTableCell>
                        <StyledTableCell>Statisztika</StyledTableCell>
                    </tr>
                </StyledTableHead>
                <TableBody>
                    {subjects.map((subject, index) => renderGrades(subject, index))}
                </TableBody>
            </Table>
        </TableContainer></>)
}

export default ParentGradeTable