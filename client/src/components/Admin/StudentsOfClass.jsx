import { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';

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
    }, [props.classId]); // Add props.classId as a dependency

    return (
        <div style={{ padding: '16px' }}>
            <Typography variant="h4" gutterBottom>
                Students of Class {props.className}
            </Typography>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>First Name</TableCell>
                            <TableCell>Family Name</TableCell>
                            <TableCell>Birth Date</TableCell>
                            <TableCell>Birth Place</TableCell>
                            <TableCell>Student No</TableCell>
                            <TableCell>Class</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {students.length > 0 ? (
                            students.map(student => (
                                <TableRow key={student.id}>
                                    <TableCell>{student.id}</TableCell>
                                    <TableCell>{student.firstName}</TableCell>
                                    <TableCell>{student.familyName}</TableCell>
                                    <TableCell>{new Date(student.birthDate).toLocaleDateString()}</TableCell>
                                    <TableCell>{student.birthPlace}</TableCell>
                                    <TableCell>{student.studentNo}</TableCell>
                                    <TableCell>{props.className}</TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={7} align="center">
                                    No students found
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
    );
}

export default StudentsOfClass;
