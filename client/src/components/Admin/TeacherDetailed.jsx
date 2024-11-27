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
import {
    AdminStyledTableHead,
    AdminStyledTableCell,
    AdminStyledTableRow,
    AdminStyledTableContainer,
    BButton,
    StyledHeading,
} from '../../../StyledComponents';
import { useNavigate } from 'react-router-dom';


function TeacherDetailed({ teacher, subjects }) {
    const navigate = useNavigate();



    return (
        <Box>
            <StyledHeading>
                {teacher.firstName} {teacher.familyName}
            </StyledHeading>
            <AdminStyledTableContainer>
                <Table aria-label="teachers table">
                    <AdminStyledTableHead>
                        <TableRow>
                            <AdminStyledTableCell>Tantárgyak</AdminStyledTableCell>
                            <AdminStyledTableCell>Osztályok</AdminStyledTableCell>
                        </TableRow>
                    </AdminStyledTableHead>
                    <TableBody>
                        {subjects.length > 0 ? (
                            subjects.map((subject) => (
                                <AdminStyledTableRow key={subject.id}>
                                    <TableCell>{subject.subject}</TableCell>
                                    <TableCell>{subject.className || "N/A"}</TableCell>
                                </AdminStyledTableRow>
                            ))
                        ) : (
                            <AdminStyledTableRow>
                                <TableCell>
                                    Nincsenek tantárgyai.
                                </TableCell>
                            </AdminStyledTableRow>
                        )}
                    </TableBody>
                </Table>
            </AdminStyledTableContainer>
            <BButton onClick={() => navigate("/admin/teachers")}>
                Vissza
            </BButton>
        </Box>
    );
}

export default TeacherDetailed;
