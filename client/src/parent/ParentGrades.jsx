import React, { useState, useEffect } from 'react';
import {
    Table,
    TableCell,
    TableBody,
    TableContainer,
    Paper,
    Typography
} from '@mui/material';
import { useParams } from 'react-router-dom';
import ParentGradeTable from "../components/Parent/ParentGradeTable.jsx";
import { StyledTableHead, StyledTableCell, PopupContainer, StatisticsContainer } from '../../StyledComponents';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import { getAverageBySubjectFullYear, getAverageBySubjectByMonth, getDifference } from '../../gradeCalculations';
import ParentNavbar from "../components/Parent/ParentNavbar.jsx";

function ParentGrades() {
    const { id } = useParams();
    
    const [grades, setGrades] = useState([]);
    const [subjects, setSubjects] = useState([]);



    useEffect(() => {
        fetch(`/api/grades/${id}`)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setGrades(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [id]);

    useEffect(() => {
        fetch(`/api/subjects`)
            .then(response => response.json())
            .then(data => {
                setSubjects(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, []);


  

    return (<>
        <ParentNavbar studentId={id}/>
       <ParentGradeTable id={id} grades={grades} subjects={subjects}/>
        </>
    );
}

export default ParentGrades;
