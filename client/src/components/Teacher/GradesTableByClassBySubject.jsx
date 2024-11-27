import React, { useState} from "react";
import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
} from "@mui/material";
import {
    Cell,
    TableHeading,
} from "../../../StyledComponents.js";
import MonthSelector from "../Parent/MonthSelector.jsx";
import StudentFetcherByClassId from "./StudentFetcherByClassId.jsx";
import GradesByStudentByMonth from "./GradesByStudentByMonth.jsx";

function GradesTableByClassBySubject({grades, classId}) {



    const [chosenMonthIndex, setChosenMonthIndex] = useState(new Date().getMonth());
    const [students, setStudents] = useState([])
    
    
 

    const filterGradesByMonth = (studentId) => {
        return grades.filter((grade) => {
            const gradeDate = new Date(grade.date);
            const gradeMonthIndex = gradeDate.getMonth();
            return grade.studentId === studentId && gradeMonthIndex === chosenMonthIndex;
        });
    };
    
 

    return (<>
        <StudentFetcherByClassId classId={classId} onData={(data)=>setStudents(data)}/>
        
        <TableContainer>          
            <Table>
                <TableHeading>
                    <TableRow>
                <Cell>Név</Cell>
                <Cell>Osztályzat</Cell>
                <Cell>
                    <MonthSelector onMonthChange={setChosenMonthIndex}/>
                </Cell>
            </TableRow>
            </TableHeading>
                <TableBody>
                    {students.map((student) => <TableRow key={student.id}>
                    <TableCell>{student.familyName} {student.firstName}</TableCell>
                        
                        <GradesByStudentByMonth studentId={student.id} 
                                                studentGradesByMonth={filterGradesByMonth(student.id)}/>
                </TableRow>)}

                </TableBody>
            </Table>
        </TableContainer>

    </>)
}
export default GradesTableByClassBySubject