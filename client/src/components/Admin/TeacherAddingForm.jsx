import React, { useState } from 'react';
import { TextField, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { AButton, CenteredStack, BButton } from '../../../StyledComponents';

function TeacherAddingForm({ onSave, onCancel }) {
    const navigate = useNavigate();

    const [familyName, setFamilyName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [secondPassword, setSecondPassword] = useState("")

    const handleSubmit = (e) => {
        e.preventDefault();
        if(secondPassword !== password){
            alert("A két jelszó nem egyezik!");
            return;
        }
        
       
        onSave({ familyName, firstName, username, email, password });
    };




    return (
        <>
            <h1>Tanár hozzáadása</h1>
            <form noValidate onSubmit={handleSubmit}>
                <CenteredStack>
                    <TextField
                        label='Vezetéknév'
                        type='text'
                        value={familyName}
                        onChange={(e)=>setFamilyName(e.target.value)}
                    />
                    <TextField
                        label='Keresztnév'
                        type='text'
                        value={firstName}
                        onChange={(e)=>setFirstName(e.target.value)}
                    />
                    <TextField
                        label='Felhasználónév'
                        type='text'
                        value={username}
                        onChange={(e)=>setUsername(e.target.value)}
                    />
                    <TextField
                        label='Email'
                        type='email'
                        value={email}
                        onChange={(e)=>setEmail(e.target.value)}
                    />
                    <TextField
                        label='Jelszó'
                        type='password'
                        value={password}
                        onChange={(e)=> setPassword(e.target.value)}
                    />
                    <TextField
                        label='Jelszó újra'
                        type='password'
                        value={secondPassword}
                        onChange={(e)=>setSecondPassword(e.target.value)}
                    />
                    <AButton
                        type='submit'
                    >
                        Hozzáad
                    </AButton>
                    <BButton
                        type='button'
                        onClick={()=>navigate("/admin/teachers")}
                        variant='outlined'
                    >
                        Vissza
                    </BButton>
                </CenteredStack>
            </form>
        </>
    );
}

export default TeacherAddingForm;
