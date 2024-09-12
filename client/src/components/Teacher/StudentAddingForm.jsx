import {TextField, Button, Stack} from "@mui/material";
import React, { useState } from 'react';

function StudentAddingForm({ onSave, onCancel }){

    const [familyName, setFamilyName ] = useState("");
    const [firstName, setFirstName] = useState("");
    const [birthDate, setBirthDate] = useState("");
    const [birthPlace,setBirthPlace] = useState("");
    const [studentNo, setStudentNo] = useState("");
    


    const handleSubmit = (e) => {
        e.preventDefault();
        onSave({ familyName, firstName, birthDate, birthPlace, studentNo });
    };
 

    const handleFamilyNameChange = (e) => {
        setFamilyName(e.target.value);
    };

    const handleFirstNameChange = (e) => {
        setFirstName(e.target.value);
    };
    const handleBirthDateChange = (e)=>{
        setBirthDate(e.target.value);
    }
    
    const handleBirthPlaceChange = (e)=>{
        setBirthPlace(e.target.value);
    }
    
    const handleStudentNoChange =(e)=>{
        setStudentNo(e.target.value);
    }
    return (
        <>
            <h1>Diák hozzáadása</h1>
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
                        label="Születési idő" 
                               type="date"
                        value={birthDate}
                               InputLabelProps={{shrink: true}}
                    onChange = {handleBirthDateChange}/>
                    <TextField 
                        label="Születési hely" 
                        type='text'
                        value={birthPlace}
                    onChange={handleBirthPlaceChange}/>
                    <TextField 
                        label='OM azonosító' 
                        type='text'
                    value={studentNo}
                    onChange={handleStudentNoChange}/>
                    
                    <Button
                        type='submit'
                        variant='contained'
                        sx={{backgroundColor: '#b5a58d', '&:hover': {backgroundColor: '#b8865a'}}}
                    >
                        Hozzáad
                    </Button>
                </Stack>
            </form>
        </>
    );
}

export default StudentAddingForm
