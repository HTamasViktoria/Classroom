import { TextField } from "@mui/material";
import { useState } from "react";

function DateSelector(props) {
    const [selectedDate, setSelectedDate] = useState("");

    const handleDateChange = (event) => {
        const newDate = event.target.value;
        setSelectedDate(newDate);
        props.onDateChange(newDate);
    };

    return (
        <TextField
            label="Dátum"
            type="date"
            value={selectedDate}
            onChange={handleDateChange}
            InputLabelProps={{ shrink: true }}
        />
    );
}

export default DateSelector;
