import React, {useState} from "react";
import {Button, Stack, TextField} from "@mui/material";
import { useNavigate } from 'react-router-dom';

function ClassAdding(){

    const navigate = useNavigate();
    
    const [grade, setGrade] = useState("")
    const [section, setSection] = useState("")

    const handleSubmit = (e) => {
        e.preventDefault();

        const classData = {
            grade: grade,
            section: section
        };

        fetch('/api/classes/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(classData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('class added:', data);
                navigate("/classes")
            })
            .catch(error => console.error('Error adding class:', error));
    };


    const handleSectionChange=(e)=>{
        setSection(e.target.value)
    }

    const handleGradeChange=(e)=>{
        setGrade(e.target.value)
    }

    return(<> <h1>Osztály létrehozása</h1>
        <form noValidate onSubmit={handleSubmit}>
            <Stack spacing={2} width={400}>
                <TextField
                    label='Grade'
                    type='text'
                    value={grade}
                    onChange={handleGradeChange}
                />
                <TextField
                    label='Section'
                    type='text'
                    value={section}
                    onChange={handleSectionChange}
                />
                <Button
                    type='submit'
                    variant='contained'
                    sx={{backgroundColor: '#b5a58d', '&:hover': {backgroundColor: '#b8865a'}}}
                >
                    Hozzáad
                </Button>
            </Stack>
        </form>
    </>)
}

export default ClassAdding