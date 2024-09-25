import React, { useEffect, useState } from 'react';
import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, Box} from '@mui/material';
import Button from "@mui/material/Button";

function StudentsOfClass(props) {
    const [students, setStudents] = useState([]);

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


    const goBackHandler = () => {
        navigate("/admin")
    }
    
    
    return (
        <>
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                Diákok
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="students table">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>Családnév</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>Keresztnév</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>Születési hely</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>Születési idő</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>OM azonosító</TableCell>
                        </TableRow>
                    </TableHead>
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
    <Button variant="contained"
            sx={{
                backgroundColor: '#bacfb0',
                color: '#fff',
                '&:hover': {
                    backgroundColor: '#a8bfa1',
                },
            }}
            onClick={props.onGoBack}>
        Vissza
    </Button>
    </>
    );
}

export default StudentsOfClass;
