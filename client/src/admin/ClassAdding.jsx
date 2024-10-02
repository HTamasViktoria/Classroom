import React, { useState } from "react";
import { Stack, TextField, Typography } from "@mui/material";
import { useNavigate } from 'react-router-dom';
import { StyledButton } from "../../StyledComponents";

function ClassAdding() {
    const navigate = useNavigate();

    const [grade, setGrade] = useState("");
    const [section, setSection] = useState("");

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
                navigate("/admin/classes");
            })
            .catch(error => console.error('Error adding class:', error));
    };

    const handleSectionChange = (e) => {
        setSection(e.target.value);
    };

    const handleGradeChange = (e) => {
        setGrade(e.target.value);
    };

    const goBackHandler = () => {
        navigate("/admin/classes");
    };

    return (
        <>
            <Typography variant="h4" gutterBottom>
                Osztály létrehozása
            </Typography>
            <form noValidate onSubmit={handleSubmit}>
                <Stack spacing={2} width={400}>
                    <TextField
                        label='Évfolyam'
                        type='text'
                        value={grade}
                        onChange={handleGradeChange}
                        sx={{ marginBottom: 2 }}
                    />
                    <TextField
                        label='Osztály'
                        type='text'
                        value={section}
                        onChange={handleSectionChange}
                        sx={{ marginBottom: 2 }}
                    />
                    <StyledButton type='submit'>
                        Hozzáad
                    </StyledButton>
                </Stack>
            </form>
            <StyledButton onClick={goBackHandler}>
                Vissza
            </StyledButton>
        </>
    );
}

export default ClassAdding;
