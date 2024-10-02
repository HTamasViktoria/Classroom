import { Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import React from "react";
import { useNavigate } from 'react-router-dom';
import { useTheme } from '@mui/material/styles';
import { StyledButton, StyledTableHead, StyledTableCell } from "../../../StyledComponents";

function AdminClassList(props) {
    const navigate = useNavigate();
    const theme = useTheme();

    return (
        <>
            <StyledButton
                sx={{
                    marginBottom: 2
                }}
                onClick={() => navigate("/add-class")}
            >
                Új osztály létrehozása
            </StyledButton>
            <TableContainer component={Paper} sx={{ marginTop: 2 }}>
                <Table>
                    <StyledTableHead>
                        <TableRow>
                            <StyledTableCell>Évfolyam</StyledTableCell>
                            <StyledTableCell>Osztály</StyledTableCell>
                            <StyledTableCell>Tanulók</StyledTableCell>
                            <StyledTableCell>Műveletek</StyledTableCell>
                        </TableRow>
                    </StyledTableHead>
                    <TableBody>
                        {props.classes.map((classItem) => (
                            <TableRow key={classItem.id}>
                                <TableCell>{classItem.grade}</TableCell>
                                <TableCell>{classItem.section}</TableCell>
                                <TableCell>
                                    <StyledButton
                                        onClick={() => props.onViewStudents(classItem.id, classItem.name)}
                                    >
                                        Diákok megtekintése
                                    </StyledButton>
                                </TableCell>
                                <TableCell>
                                    <StyledButton
                                        onClick={() => props.onAddStudent(classItem.id, classItem.name)}
                                    >
                                        Diák hozzáadása az osztályhoz
                                    </StyledButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}

export default AdminClassList;
