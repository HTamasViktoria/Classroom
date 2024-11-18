import React, { useState } from "react";
import { Paper, Table, TableBody, TableCell, TableContainer, Typography } from "@mui/material";
import { Cell, TableHeading, StyledTableContainer, StyledHeading } from "../../../StyledComponents";
import MonthSelector from "./MonthSelector.jsx";
import RenderGrades from "./RenderGrades.jsx";
import SubjectFetcher from "./SubjectFetcher.jsx";



function GradeTable({studentId, isEditable, teacherSubjects, nameOfClass, onEditHandler}) {
    
    const [subjects, setSubjects] = useState([]);
    const [chosenMonthIndex, setChosenMonthIndex] = useState(new Date().getMonth());
    
    
    return (
        <>
            <SubjectFetcher studentId={studentId} onData={(data) => setSubjects(data)} />

            <StyledTableContainer>
                <StyledHeading>
                    Osztályzatok
                </StyledHeading>
                <Table>
                    <TableHeading>
                        <tr>
                            <Cell>Tantárgy</Cell>
                            <Cell>
                                <MonthSelector onMonthChange={setChosenMonthIndex} />
                            </Cell>
                            <Cell>Statisztika</Cell>
                            {isEditable && <Cell>Műveletek</Cell>}
                        </tr>
                    </TableHeading>
                    <TableBody>
                        {subjects.map((subject, index) => (
                            <RenderGrades
                                studentId={studentId}
                                key={subject}
                                subject={subject}
                                index={index}
                                chosenMonthIndex={chosenMonthIndex}
                                isEditable={isEditable}
                                teacherSubjects={teacherSubjects}
                                nameOfClass={nameOfClass}
                                onEditHandler={onEditHandler}
                            />
                        ))}
                    </TableBody>
                </Table>
            </StyledTableContainer>
        </>
    );
}

export default GradeTable;
