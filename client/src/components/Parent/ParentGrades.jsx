import React, { useState, useEffect } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';
import { useParams } from 'react-router-dom';
function ParentGrades(props) {
    const { id } = useParams();
    
    const [grades, setGrades] = useState([]);

    useEffect(() => {
        fetch(`/api/grades/${id}`)
            .then(response => response.json())
            .then(data => {
                console.log(data)
                setGrades(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [props.student]);

    return (
        <TableContainer component={Paper}>
            <Typography variant="h6" component="div" style={{ padding: 16 }}>
               Osztályzatok
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