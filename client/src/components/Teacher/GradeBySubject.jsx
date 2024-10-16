import React, { useState, useEffect } from "react";
import { Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import {
    CustomBox,
    StyledButton,
    StyledTableCell,
    StyledTableHead,
} from "../../../StyledComponents.js";
import GradeDetailsBySubjectByClass from "./GradeDetailsBySubjectByClass.jsx";

function GradeBySubject({ subject, onGoBack }) {
    const [classes, setClasses] = useState([]);
    const [averages, setAverages] = useState({})
    const [viewingDetails, setViewingDetails] = useState(false);
    const [classId, setClassId] = useState("")
    const [className, setClassName] = useState("")

    useEffect(() => {
        fetch(`/api/classes/getClassesBySubject/${subject}/`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log(data);
                setClasses(data);
            })
            .catch(error => console.error('Error fetching classes:', error));
    }, [subject]);

    
    useEffect(()=>{
        fetch(`/api/grades/class-average/${subject}`)
            .then(response=>response.json())
            .then(data=> {console.log(data)
                setAverages(data)
            })
            .catch(error=>console.error(`Error:`,error))

    },[])


    const viewDetailsHandler = (e) => {
        const id = parseInt(e.target.id*1);
        const selectedClass = classes.find(cls => cls.id === id);

        if (selectedClass) {
            setClassName(selectedClass.name);
        }

        setClassId(id);
        setViewingDetails(true);
    };
    
    
    const noDetailsHandler=()=>{
        setViewingDetails(false)
    }
    
    
    const goBackHandler=()=>{
        onGoBack()
    }
    
    return (
        <>
            {viewingDetails===true ? (<GradeDetailsBySubjectByClass onGoBack={noDetailsHandler} subject={subject} classId={classId} className={className}/>) : 
                (<> <TableContainer>
                <Table>
                    <StyledTableHead>
                        <TableRow>
                            <StyledTableCell>Osztály</StyledTableCell>
                            <StyledTableCell>Átlag</StyledTableCell>
                            <StyledTableCell>Műveletek</StyledTableCell>
                        </TableRow>
                    </StyledTableHead>
                    <TableBody>
                        {classes.map((classItem) => (
                            <TableRow key={classItem.id}>
                                <StyledTableCell>{classItem.name}</StyledTableCell>
                                <StyledTableCell>{averages[classItem.name]}</StyledTableCell>
                                <StyledTableCell>
                                    <StyledButton id={classItem.id} onClick={viewDetailsHandler} >Részletek</StyledButton>
                                </StyledTableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer><StyledButton onClick={goBackHandler}>Vissza</StyledButton>  </>)}
           
        </>
    );
}

export default GradeBySubject;
