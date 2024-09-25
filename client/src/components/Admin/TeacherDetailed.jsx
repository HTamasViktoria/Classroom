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

function TeacherDetailed({ teacher, subjects }) {
    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                {teacher.firstName} {teacher.familyName}
            </Typography>
            <TableContainer component={Paper} sx={{ overflow: 'hidden' }}>
                <Table sx={{ minWidth: 650 }} aria-label="teachers table">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>
                                Tantárgyak
                            </TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>
                                Osztályok
                            </TableCell>
                        </TableRow>
                    </TableHead>
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
        </Box>
    );
}

export default TeacherDetailed;
