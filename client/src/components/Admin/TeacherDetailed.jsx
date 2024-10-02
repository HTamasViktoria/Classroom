import {
    Box,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography
} from "@mui/material";
import React from "react";
import { StyledButton, StyledTableHead, StyledTableCell } from '../../../StyledComponents';
import { useNavigate } from 'react-router-dom';

function TeacherDetailed({ teacher, subjects }) {
    const navigate = useNavigate();

    const goBackHandler = () => {
        navigate("/admin/teachers");
    };

    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                {teacher.firstName} {teacher.familyName}
            </Typography>
            <TableContainer component={Paper} sx={{ overflow: 'hidden' }}>
                <Table sx={{ minWidth: 650 }} aria-label="teachers table">
                    <StyledTableHead>
                        <TableRow>
                            <StyledTableCell>Tantárgyak</StyledTableCell>
                            <StyledTableCell>Osztályok</StyledTableCell>
                        </TableRow>
                    </StyledTableHead>
                    <TableBody>
                        {subjects.length > 0 ? (
                            subjects.map((subject) => (
                                <TableRow key={subject.id}>
                                    <TableCell>{subject.subject}</TableCell>
                                    <TableCell>{subject.className || "N/A"}</TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={2} align="center">
                                    Nincsenek tantárgyai.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            <StyledButton
                onClick={goBackHandler}
                sx={{ marginTop: 2 }}
            >
                Vissza
            </StyledButton>
        </Box>
    );
}

export default TeacherDetailed;
