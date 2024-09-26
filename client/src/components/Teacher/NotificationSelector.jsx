import React, { useState } from 'react';
import { FormControl, InputLabel, Select, MenuItem, Button, Box } from "@mui/material";

function NotificationSelector(props) {
    const [selectedValue, setSelectedValue] = useState('');

    const handleChange = (event) => {
        const newValue = event.target.value;
        setSelectedValue(newValue);
    };

    const goBackHandler = () => {
        props.onGoBack();
    }

    return (
        <Box display="flex" alignItems="center" width="100%">
            <FormControl sx={{ minWidth: 200, mr: 2 }}>
                <InputLabel id="demo-simple-select-label">Értesítés típusa</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={selectedValue}
                    label="Értesítés típusa"
                    onChange={handleChange}
                >
                    <MenuItem value={"Homework"}>Házi feladat</MenuItem>
                    <MenuItem value={"Exam"}>Dolgozat</MenuItem>
                    <MenuItem value={"MissingEquipment"}>Felszereléshiány</MenuItem>
                    <MenuItem value={"OtherNotification"}>Egyéb</MenuItem>
                </Select>
            </FormControl>

            <Button
                variant="contained"
                sx={{
                    flexGrow: 1,
                    backgroundColor: '#82b2b8',
                    color: '#fff',
                    '&:hover': {
                        backgroundColor: '#6e9ea4',
                    },
                }}
                onClick={() => props.onChosenType(selectedValue)}
            >
                Típus kiválasztása
            </Button>

          
            <Box sx={{ mx: 1 }} />

            <Button
                variant="contained"
                sx={{
                    flexGrow: 1,
                    backgroundColor: '#a2c4c6',
                    color: '#fff',
                    '&:hover': {
                        backgroundColor: '#8ab2b5 ',
                    },
                }}
                onClick={goBackHandler}
            >
                Vissza
            </Button>
        </Box>
    );
}

export default NotificationSelector;
