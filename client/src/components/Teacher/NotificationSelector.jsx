import React, { useState } from 'react';
import { FormControl, InputLabel, Select, MenuItem, Button } from "@mui/material";

function NotificationSelector(props) {
    const [selectedValue, setSelectedValue] = useState('');

   
    const handleChange = (event) => {
        const newValue = event.target.value;
        setSelectedValue(newValue);
       
    };

    return (
        <FormControl fullWidth>
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
            <Button onClick={()=>props.onChosenType(selectedValue)}>Típus kiválasztása</Button>
        </FormControl>
    );
}

export default NotificationSelector;
