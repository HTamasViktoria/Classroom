import React from 'react';
import { Box, Typography, Table, TableBody, TableContainer, TableHead, TableRow, Paper, TableCell } from "@mui/material";
import { CustomBox, StyledTableCell, StyledTableHead } from '../../../StyledComponents';

function AdminStudentList({ students }) {
    return (
        <CustomBox>
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
        </CustomBox>
    );
}

export default AdminStudentList;
