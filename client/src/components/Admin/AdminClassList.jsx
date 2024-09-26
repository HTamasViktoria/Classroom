import {Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import React from "react";
import { useNavigate } from 'react-router-dom';

function AdminClassList(props){

    const navigate = useNavigate();
    const handleNavigate = () => {
        navigate("/add-class");
    }
    
    return (<>    <Button
        variant="contained"
        sx={{
            backgroundColor: '#b29a88',
            color: '#fff',
            '&:hover': {
                backgroundColor: '#a0887a',
            },
        }}
        onClick={() => navigate("/add-class")}
    >
        Új osztály létrehozása
    </Button>
        <TableContainer component={Paper} sx={{ marginTop: 2 }}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Évfolyam</TableCell>
                        <TableCell>Osztály</TableCell>
                        <TableCell>Tanulók</TableCell>
                        <TableCell>Műveletek</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.classes.map((classItem) => (
                        <TableRow key={classItem.id}>
                            <TableCell>{classItem.grade}</TableCell>
                            <TableCell>{classItem.section}</TableCell>
                            <TableCell>
                                <Button
                                    variant="contained"
                                    sx={{
                                        backgroundColor: '#bacfb0',
                                        color: '#fff',
                                        '&:hover': {
                                            backgroundColor: '#a8bfa1 ',
                                        },
                                    }}
                                    onClick={() => props.onViewStudents(classItem.id, classItem.name)}
                                >
                                   Diákok megtekintése
                                </Button>
                            </TableCell>
                            <TableCell>
                                <Button
                                    variant="contained"
                                    sx={{
                                        backgroundColor: '#bacfb0',
                                        color: '#fff',
                                        '&:hover': {
                                            backgroundColor: '#a8bfa1 ',
                                        },
                                    }}
                                    onClick={() => props.onAddStudent(classItem.id, classItem.name)}
                                >
                                    Diák hozzáadása az osztályhoz
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer></>)
}
export default AdminClassList