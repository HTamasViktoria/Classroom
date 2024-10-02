import React, { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, Box } from '@mui/material';
import { StyledButton, StyledTableHead, StyledTableCell } from '../../../StyledComponents';
import { useNavigate } from 'react-router-dom';

function StudentsOfClass(props) {
    const [students, setStudents] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`/api/classes/getStudents/${props.classId}`)
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
    }, [props.classId]);

    return (
        <>
            <Box sx={{ padding: 2 }}>
                <Typography variant="h6" sx={{ marginBottom: 2 }}>
                    Diákok
                </Typography>
                <TableContainer component={Paper}>
                    <Table sx={{ minWidth: 650 }} aria-label="students table">
                        <StyledTableHead>
                            <TableRow>
                                {['ID', 'Családnév', 'Keresztnév', 'Születési hely', 'Születési idő', 'OM azonosító'].map((header) => (
                                    <StyledTableCell key={header}>
                                        {header}
                                    </StyledTableCell>
                                ))}
                            </TableRow>
                        </StyledTableHead>
                        <TableBody>
                            {students.map((student) => (
                                <TableRow key={student.id}>
                                    <TableCell>{student.id}</TableCell>
                                    <TableCell>{student.familyName}</TableCell>
                                    <TableCell>{student.firstName}</TableCell>
                                    <TableCell>{student.birthPlace}</TableCell>
                                    <TableCell>{student.birthDate.substring(0, 10)}</TableCell>
                                    <TableCell>{student.studentNo}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
            <StyledButton
                variant="contained"
                onClick={props.onGoBack}
            >
                Vissza
            </StyledButton>
        </>
    );
}

export default StudentsOfClass;
