import { Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import React from "react";
import { useNavigate } from 'react-router-dom';
import { useTheme } from '@mui/material/styles';
import { AButton, TableHeading, Cell } from "../../../StyledComponents";

function AdminClassList(props) {
    const navigate = useNavigate();
    const theme = useTheme();

    return (
        <>
            <AButton
                sx={{
                    marginBottom: 2
                }}
                onClick={() => navigate("/add-class")}
            >
                Új osztály létrehozása
            </AButton>
            <TableContainer component={Paper} sx={{ marginTop: 2 }}>
                <Table>
                    <TableHeading>
                        <TableRow>
                            <Cell>Évfolyam</Cell>
                            <Cell>Osztály</Cell>
                            <Cell>Tanulók</Cell>
                            <Cell>Műveletek</Cell>
                        </TableRow>
                    </TableHeading>
                    <TableBody>
                        {props.classes.map((classItem) => (
                            <TableRow key={classItem.id}>
                                <TableCell>{classItem.grade}</TableCell>
                                <TableCell>{classItem.section}</TableCell>
                                <TableCell>
                                    <AButton
                                        onClick={() => props.onViewStudents(classItem.id, classItem.name)}
                                    >
                                        Diákok megtekintése
                                    </AButton>
                                </TableCell>
                                <TableCell>
                                    <AButton
                                        onClick={() => props.onAddStudent(classItem.id, classItem.name)}
                                    >
                                        Diák hozzáadása az osztályhoz
                                    </AButton>
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
