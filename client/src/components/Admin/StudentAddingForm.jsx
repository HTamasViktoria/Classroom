import React, { useState } from 'react';
import { CustomBox } from '../../../StyledComponents';
import { AButton, StyledHeading, StyledTextField, StyledGridItem, StyledGrid } from '../../../StyledComponents';

function StudentAddingForm({ onSave, onCancel }) {
    const [familyName, setFamilyName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [birthDate, setBirthDate] = useState("");
    const [birthPlace, setBirthPlace] = useState("");
    const [studentNo, setStudentNo] = useState("");
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [secondaryPassword, setSecondaryPassword] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();

        if (password !== secondaryPassword) {
            alert("A megadott jelszavak nem egyeznek!");
            return;
        }

        if (!familyName || !firstName || !birthDate || !birthPlace || !studentNo || !email || !username || !password || !secondaryPassword) {
            alert("Kérjük, töltse ki az összes mezőt.");
            return;
        }

        onSave({ familyName, firstName, birthDate, birthPlace, studentNo, email, username, password });
    };

    return (
        <CustomBox>
            <StyledHeading>
                Diák hozzáadása
            </StyledHeading>
            <form noValidate onSubmit={handleSubmit}>
                <StyledGrid container spacing={3}>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Vezetéknév"
                            type="text"
                            value={familyName}
                            onChange={(e) => setFamilyName(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Keresztnév"
                            type="text"
                            value={firstName}
                            onChange={(e) => setFirstName(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Születési idő"
                            type="date"
                            value={birthDate}
                            InputLabelProps={{ shrink: true }}
                            onChange={(e) => setBirthDate(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Születési hely"
                            type="text"
                            value={birthPlace}
                            onChange={(e) => setBirthPlace(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="OM azonosító"
                            type="text"
                            value={studentNo}
                            onChange={(e) => setStudentNo(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Email"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Felhasználónév"
                            type="text"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Jelszó"
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item>
                        <StyledTextField
                            label="Jelszó megerősítése"
                            type="password"
                            value={secondaryPassword}
                            onChange={(e) => setSecondaryPassword(e.target.value)}
                            fullWidth
                            required
                        />
                    </StyledGridItem>
                    <StyledGridItem item xs={12}>
                        <AButton type="submit">
                            Hozzáad
                        </AButton>
                        <AButton type="button" onClick={onCancel}>
                            Mégse
                        </AButton>
                    </StyledGridItem>
                </StyledGrid>
            </form>
        </CustomBox>
    );
}

export default StudentAddingForm;
