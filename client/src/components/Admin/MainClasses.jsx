import {Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import React from "react";
import { useNavigate } from 'react-router-dom';

function MainClasses(props){

    const navigate = useNavigate();
    const handleNavigate = () => {
        navigate("/add-class");
    }
    
    return (<>   <Button onClick={handleNavigate}>Adding new classes</Button>
        <TableContainer component={Paper} sx={{ marginTop: 2 }}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Grade</TableCell>
                        <TableCell>Section</TableCell>
                        <TableCell>Students</TableCell>
                        <TableCell>Actions</TableCell>
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
                                    color="primary"
                                    onClick={() => props.onViewStudents(classItem.id, classItem.name)}
                                >
                                    View Students
                                </Button>
                            </TableCell>
                            <TableCell>
                                <Button
                                    variant="contained"
                                    color="primary"
                                    onClick={() => props.onAddStudent(classItem.id, classItem.name)}
                                >
                                    Adding Student
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer></>)
}
export default MainClasses