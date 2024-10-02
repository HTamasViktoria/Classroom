import React, { useState, useEffect } from 'react';
import {
    Table,
    TableCell,
    TableBody,
    TableContainer,
    Paper,
    Typography
} from '@mui/material';
import { useParams } from 'react-router-dom';
import { StyledTableHead, StyledTableCell } from '../../../StyledComponents';

function ParentGrades() {
    const { id } = useParams();
    const [grades, setGrades] = useState([]);

    useEffect(() => {
        fetch(`/api/grades/${id}`)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setGrades(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    return (
        <TableContainer component={Paper}>
            <Typography variant="h6" component="div" sx={{ margin: 2 }}>
                Osztályzatok
            </Typography>
            {grades.length > 0 ? (
                <Table>
                    <StyledTableHead>
                        <tr>
                            <StyledTableCell>Tantárgy</StyledTableCell>
                            <StyledTableCell>Dátum</StyledTableCell>
                            <StyledTableCell>Jegy</StyledTableCell>
                        </tr>
                    </StyledTableHead>
                    <TableBody>
                        {grades.map((grade) => (
                            <tr key={`tableRow${grade.id}`}>
                                <TableCell>{grade.subject}</TableCell>
                                <TableCell>{grade.date.substring(0, 10)}</TableCell>
                                <TableCell>{grade.value}</TableCell>
                            </tr>
                        ))}
                    </TableBody>
                </Table>
            ) : (
                <Typography variant="body1" sx={{ margin: 2 }}>
                    Nincs elérhető jegy
                </Typography>
            )}
        </TableContainer>
    );
}

export default ParentGrades;
