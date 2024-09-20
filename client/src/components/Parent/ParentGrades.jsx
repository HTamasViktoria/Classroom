import React, { useState, useEffect } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';

function ParentGrades(props) {
    const [grades, setGrades] = useState([]);

    useEffect(() => {
        fetch(`/api/grades/${props.student.id}`)
            .then(response => response.json())
            .then(data => {
                setGrades(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [props.student]);

    return (
        <TableContainer component={Paper}>
            <Typography variant="h6" component="div" style={{ padding: 16 }}>
                {props.student.FirstName} {props.student.FamilyName} Jegyei
            </Typography>
            {grades.length > 0 ? (
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Tantárgy</TableCell>
                            <TableCell>Dátum</TableCell>
                            <TableCell>Jegy</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {grades.map((grade) => (
                            <TableRow key={`tableRow${grade.id}`}>
                                <TableCell>{grade.subject}</TableCell>
                                <TableCell>
                                    {grade.date.substring(0, 10)}
                                </TableCell>
                                <TableCell>{grade.value}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            ) : (
                <Typography variant="body1" style={{ padding: 16 }}>
                    Nincs elérhető jegy
                </Typography>
            )}
        </TableContainer>
    );
}

export default ParentGrades;