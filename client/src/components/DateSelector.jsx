import { TextField } from "@mui/material";

function DateSelector({ selectedDate, handleDateChange }) {
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
