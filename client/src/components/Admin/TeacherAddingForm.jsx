import React, { useState } from 'react';
import { TextField, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { StyledButton } from '../../../StyledComponents';

function TeacherAddingForm({ onSave, onCancel }) {
    const navigate = useNavigate();

    const [familyName, setFamilyName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave({ familyName, firstName, username, email, password });
    };

    const handleFamilyNameChange = (e) => {
        setFamilyName(e.target.value);
    };

    const handleFirstNameChange = (e) => {
        setFirstName(e.target.value);
    };

    const handleUsernameChange = (e) => {
        setUsername(e.target.value);
    };

    const handleEmailChange = (e) => {
        setEmail(e.target.value);
    };

    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
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
                    <TextField
                        label='Felhasználónév'
                        type='text'
                        value={username}
                        onChange={handleUsernameChange}
                    />
                    <TextField
                        label='Email'
                        type='email'
                        value={email}
                        onChange={handleEmailChange}
                    />
                    <TextField
                        label='Jelszó'
                        type='password'
                        value={password}
                        onChange={handlePasswordChange}
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
