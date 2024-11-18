import {Stack} from "@mui/material";
import { CustomBox } from '../../../StyledComponents';
import React, { useState } from 'react';
import { useTheme } from '@mui/material/styles';
import { AButton, StyledTextField, StyledHeading } from '../../../StyledComponents';

function ParentAddingForm({ onSave, onCancel, studentId }){

    const theme = useTheme();
    const [familyName, setFamilyName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [secondaryPassword, setSecondaryPassword] = useState("");
    const [childName, setChildName] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();

        if(password !== secondaryPassword){
            alert("A megadott jelszavak nem egyeznek!");
            return;
        }


        if (!familyName || !firstName || !email || !username || !password || !secondaryPassword || !childName) {
            alert("Kérjük, töltse ki az összes mezőt.");
            return;
        }
        onSave({ familyName, firstName, email, username, password, studentId, childName });
    };

    return (<>
        <CustomBox>
            <StyledHeading>
                Diák hozzáadása
            </StyledHeading>
            <form noValidate onSubmit={handleSubmit}>
                <Stack spacing={2} width={400}>
                    <StyledTextField
                        label='Vezetéknév'
                        type='text'
                        value={familyName}
                        onChange={(e) => setFamilyName(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label='Keresztnév'
                        type='text'
                        value={firstName}
                        onChange={(e) => setFirstName(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label='Gyermek neve'
                        type='text'
                        value={childName}
                        onChange={(e) => setChildName(e.target.value)}
                        required
                    />
                
                    <StyledTextField
                        label='Email'
                        type='email'
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label='Felhasználónév'
                        type='text'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label='Jelszó'
                        type='password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label='Jelszó újra'
                        type='password'
                        value={secondaryPassword}
                        onChange={(e) => setSecondaryPassword(e.target.value)}
                        required
                    />
                    <AButton
                        type='submit'
                        variant='contained'
                    >
                        Hozzáad
                    </AButton>
                    <AButton
                        type='button'
                        variant='outlined'
                        onClick={onCancel}
                    >
                        Mégse
                    </AButton>
                </Stack>
            </form>
        </CustomBox></>)
}

export default ParentAddingForm