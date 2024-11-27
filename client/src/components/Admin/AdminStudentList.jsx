import React from 'react';
import { Box, Typography, Table, TableBody, TableContainer, TableHead, TableRow, Paper, TableCell } from "@mui/material";
import { CustomBox, Cell, TableHeading } from '../../../StyledComponents';
import {Navigate, useNavigate} from "react-router-dom";
function AdminStudentList({ students }) {

    const navigate = useNavigate()
    const parentViewHandler = (e) => {
        const studentId = e.target.getAttribute('data-id');
        navigate(`/admin/parentsOf/${studentId}`)
    }
    
    return (
        <CustomBox>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                Diákok
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="students table">
                    <TableHeading>
                        <TableRow>
                            {['Családnév', 'Keresztnév', 'Születési hely', 'Születési idő', 'OM azonosító', ''].map((header) => (
                                <Cell key={header}>
                                    {header}
                                </Cell>
                            ))}
                        </TableRow>
                    </TableHeading>
                    <TableBody>
                        {students.map((student) => (
                            <TableRow key={student.id}>
                                <TableCell>{student.familyName}</TableCell>
                                <TableCell>{student.firstName}</TableCell>
                                <TableCell>{student.birthPlace}</TableCell>
                                <TableCell>{student.birthDate.substring(0, 10)}</TableCell>
                                <TableCell>{student.studentNo}</TableCell>
                                <TableCell data-id={student.id} onClick={parentViewHandler}>Szülők és gondviselők</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </CustomBox>
    );
}

export default AdminStudentList;
