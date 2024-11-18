import { TextField } from "@mui/material";


function DateSelector({selectedDate, onDateChange}) {

    const handleDateChange = (event) => {
        const newDate = event.target.value;
        onDateChange(newDate);
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
