import React, { useState } from 'react';
import { TextField, Button, Stack } from "@mui/material";


function TeacherAddingForm({ onSave, onCancel }) {
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
                        sx={{ backgroundColor: '#b5a58d', '&:hover': { backgroundColor: '#b8865a' } }}
                    >
                        Hozzáad
                    </Button>
                </Stack>
            </form>
        </>
    );
}

export default TeacherAddingForm;
