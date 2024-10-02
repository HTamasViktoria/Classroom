import React, { useState } from 'react';
import { TextField, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { StyledButton } from '../../../StyledComponents';

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

    const goBackHandler = () => {
        navigate("/admin/teachers");
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
                    <StyledButton
                        type='submit'
                    >
                        Hozzáad
                    </StyledButton>
                    <StyledButton
                        type='button'
                        onClick={goBackHandler}
                        variant='outlined'
                    >
                        Vissza
                    </StyledButton>
                </Stack>
            </form>
        </>
    );
}

export default TeacherAddingForm;
