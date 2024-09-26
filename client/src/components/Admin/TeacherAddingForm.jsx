import React, { useState } from 'react';
import { TextField, Button, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";

function TeacherAddingForm({ onSave, onCancel }) {

    const navigate = useNavigate();
    
    const [familyName, setFamilyName] = useState("");
    const [firstName, setFirstName] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave({ familyName, firstName });
    };

    const handleFamilyNameChange = (e) => {
        setFamilyName(e.target.value);
    };

    const handleFirstNameChange = (e) => {
        setFirstName(e.target.value);
    };
    
    const goBackHandler=()=>{
        navigate("/admin/teachers");
    }

    return (
        <>
            
            <h1>Tanár hozzáadása</h1>
            <form noValidate onSubmit={handleSubmit}>
                <Stack spacing={2} width={400}>
                    <TextField
                        label='Vezetéknév'
                        type='text'
                        value={familyName}
                        onChange={handleFamilyNameChange}
                    />
                    <TextField
                        label='Keresztnév'
                        type='text'
                        value={firstName}
                        onChange={handleFirstNameChange}
                    />
                    <Button
                        type='submit'
                        variant='contained'
                        sx={{ backgroundColor: '#82b2b8', '&:hover': { backgroundColor: '#6e9ea4' } }}
                    >
                        Hozzáad
                    </Button>
                    <Button
                        type='submit'
                        variant='contained'
                        sx={{ backgroundColor: '#d9c2bd', '&:hover': { backgroundColor: '#c2a6a0' } }}
                        onClick={goBackHandler}
                    >
                        Vissza
                    </Button>
                </Stack>
            </form>
        </>
    );
}

export default TeacherAddingForm;
