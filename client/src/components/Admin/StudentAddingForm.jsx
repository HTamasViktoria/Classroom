import { TextField, Stack, Typography } from "@mui/material";
import { CustomBox } from '../../../StyledComponents';
import React, { useState } from 'react';
import { useTheme } from '@mui/material/styles';
import {StyledButton, StyledTextField} from '../../../StyledComponents';

function StudentAddingForm({ onSave, onCancel }) {
    
    const theme = useTheme();
    const [familyName, setFamilyName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [birthDate, setBirthDate] = useState("");
    const [birthPlace, setBirthPlace] = useState("");
    const [studentNo, setStudentNo] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
      
        if (!familyName || !firstName || !birthDate || !birthPlace || !studentNo) {
            alert("Kérjük, töltse ki az összes mezőt.");
            return;
        }
        onSave({ familyName, firstName, birthDate, birthPlace, studentNo });
    };

    return (
        <CustomBox>
            <Typography variant="h6" sx={{ marginBottom: 2, color: theme.palette.tertiary.main }}>
                Diák hozzáadása
            </Typography>
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
                        label="Születési idő"
                        type="date"
                        value={birthDate}
                        InputLabelProps={{ shrink: true }}
                        onChange={(e) => setBirthDate(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label="Születési hely"
                        type='text'
                        value={birthPlace}
                        onChange={(e) => setBirthPlace(e.target.value)}
                        required
                    />
                    <StyledTextField
                        label='OM azonosító'
                        type='text'
                        value={studentNo}
                        onChange={(e) => setStudentNo(e.target.value)}
                        required
                    />
                    <StyledButton
                        type='submit'
                        variant='contained'
                    >
                        Hozzáad
                    </StyledButton>
                    <StyledButton
                        type='button'
                        variant='outlined'
                        onClick={onCancel}
                    >
                        Mégse
                    </StyledButton>
                </Stack>
            </form>
        </CustomBox>
    );
}

export default StudentAddingForm;
