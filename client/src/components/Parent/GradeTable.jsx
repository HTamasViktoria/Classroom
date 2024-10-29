import React from "react";
import { Paper, Table, TableBody, TableCell, TableContainer, Typography } from "@mui/material";
import { PopupContainer, StyledButton, StyledTableCell, StyledTableHead } from "../../../StyledComponents.js";
import MonthSelector from "./MonthSelector.jsx";
import RenderGrades from "./RenderGrades.jsx";

function GradeTable({
                        subjects,
                        filterGradesByMonth,
                        startHover,
                        finishHover,
                        popupPosition,
                        hoverDate,
                        hoverForWhat,
                        id,
                        grades,
                        chosenMonthIndex,
                        classAverages,
                        isEditable,
                        teacherSubjects,
                        nameOfClass,
                        editHandler,
                        setChosenMonthIndex
                    }) {
    
    
 
    
    return (
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
                            <MonthSelector onMonthChange={setChosenMonthIndex} />
                        </StyledTableCell>
                        <StyledTableCell>Statisztika</StyledTableCell>
                        {isEditable && <StyledTableCell>Műveletek</StyledTableCell>}
                    </tr>
                </StyledTableHead>
                <TableBody>
                    {subjects.map((subject, index) => (
                        <RenderGrades
                            key={subject}
                            subject={subject}
                            index={index}
                            filterGradesByMonth={filterGradesByMonth}
                            startHover={startHover}
                            finishHover={finishHover}
                            id={id}
                            grades={grades}
                            chosenMonthIndex={chosenMonthIndex}
                            classAverages={classAverages}
                            isEditable={isEditable}
                            teacherSubjects={teacherSubjects}
                            nameOfClass={nameOfClass}
                            editHandler={editHandler}
                        />
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}

export default GradeTable;
