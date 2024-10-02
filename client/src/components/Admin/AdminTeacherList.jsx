import React from 'react';
import { CustomBox, StyledTableCell, StyledTableHead } from '../../../StyledComponents';
import { useNavigate } from "react-router-dom";
import { Box, Table, TableBody, TableContainer, TableRow, TableCell, Paper, Typography } from "@mui/material";


function AdminTeacherList({ teachers }) {
    const navigate = useNavigate();

    const handleTeacherChoosing = (id) => {
        navigate(`/admin/teachers/${id}`);
    };

    return (
        <CustomBox>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                Tanárok
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="teachers table">
                    <StyledTableHead>
                        <TableRow>
                            {['ID', 'Családnév', 'Keresztnév'].map((header) => (
                                <StyledTableCell key={header}>
                                    {header}
                                </StyledTableCell>
                            ))}
                        </TableRow>
                    </StyledTableHead>
                    <TableBody>
                        {teachers.map((teacher) => (
                            <TableRow
                                key={teacher.id}
                                onClick={() => handleTeacherChoosing(teacher.id)}
                                sx={{
                                    cursor: 'pointer',
                                    '&:hover': {
                                        backgroundColor: '#f5f5f5',
                                    }
                                }}
                            >
                                <TableCell>{teacher.id}</TableCell>
                                <TableCell>{teacher.familyName}</TableCell>
                                <TableCell>{teacher.firstName}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </CustomBox>
    );
}

export default AdminTeacherList;
