import React, { useState } from 'react';
import { InputLabel, Select, MenuItem } from "@mui/material";
import { AButton, BButton, StyledFormControl, CustomBox } from '../../../StyledComponents';

function NotificationSelector(props) {
    const [selectedValue, setSelectedValue] = useState('');

    const handleChange = (event) => {
        const newValue = event.target.value;
        setSelectedValue(newValue);
    };

    const goBackHandler = () => {
        props.onGoBack();
    };

    return (
        <CustomBox display="flex" alignItems="center" width="100%">
            <StyledFormControl sx={{ minWidth: 200, mr: 2 }}>
                <InputLabel id="notification-type-label">Értesítés típusa</InputLabel>
                <Select
                    labelId="notification-type-label"
                    id="notification-type-select"
                    value={selectedValue}
                    label="Értesítés típusa"
                    onChange={handleChange}
                >
                    <MenuItem value={"Homework"}>Házi feladat</MenuItem>
                    <MenuItem value={"Exam"}>Dolgozat</MenuItem>
                    <MenuItem value={"MissingEquipment"}>Felszereléshiány</MenuItem>
                    <MenuItem value={"OtherNotification"}>Egyéb</MenuItem>
                </Select>
            </StyledFormControl>

            <AButton
                variant="contained"
                onClick={() => props.onChosenType(selectedValue)}
                sx={{
                    height: '40px',
                    marginRight: '16px',
                    padding: '30px 30px',
                }}
            >
                Típus kiválasztása
            </AButton>

            <BButton
                variant="contained"
                onClick={goBackHandler}
                sx={{
                    height: '40px',
                }}
            >
                Vissza
            </BButton>
        </CustomBox>
    );
}

export default NotificationSelector;
