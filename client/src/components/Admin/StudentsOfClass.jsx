import React, { useEffect, useState } from 'react';
import { TableBody, TableCell,TableRow,Box } from '@mui/material';
import {
    AButton,
    TableHeading,
    Cell,
    StyledTableRow,
    StyledHeading,
    StyledTableContainer, StyledTable
} from '../../../StyledComponents';


function StudentsOfClass({classId, className, onGoBack}) {
    const [students, setStudents] = useState([]);
 

    useEffect(() => {
        fetch(`/api/classes/students-of-a-class/${classId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                if (Array.isArray(data)) {
                    setStudents(data);
                } else {
                    console.error('Unexpected data format:', data);
                    setStudents([]);
                }
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    }, [classId]);

    return (
        <>
            <Box sx={{ padding: 2 }}>
                <StyledHeading>
                    A(z) {className} osztály diákjai
                </StyledHeading>
                <StyledTableContainer>
                    <StyledTable>
                        <TableHeading>
                            <StyledTableRow>
                                {['Családnév', 'Keresztnév', 'Születési hely', 'Születési idő', 'OM azonosító'].map((header) => (
                                    <Cell key={header}>
                                        {header}
                                    </Cell>
                                ))}
                            </StyledTableRow>
                        </TableHeading>
                        <TableBody>
                            {students.map((student) => (
                                <TableRow key={student.id}>
                                   
                                    <TableCell>{student.familyName}</TableCell>
                                    <TableCell>{student.firstName}</TableCell>
                                    <TableCell>{student.birthPlace}</TableCell>
                                    <TableCell>{student.birthDate.substring(0, 10)}</TableCell>
                                    <TableCell>{student.studentNo}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </StyledTable>
                </StyledTableContainer>
            </Box>
            <AButton
                onClick={onGoBack}
            >
                Vissza
            </AButton>
        </>
    );
}

export default StudentsOfClass;
