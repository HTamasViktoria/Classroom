import { useState } from "react";
import { FormControl, InputLabel, Select, MenuItem, Checkbox, ListItemText, FormControlLabel } from "@mui/material";

function TeacherReceiverSelector({ teachers, onSelectionChange }) {
    const [selectedTeachers, setSelectedTeachers] = useState([]);
    const [selectAll, setSelectAll] = useState(false);

    const handleTeacherChange = (event) => {
        const { value } = event.target;
        setSelectedTeachers(value);
        onSelectionChange(value);
    };

    const handleSelectAllChange = (e) => {
        const isChecked = e.target.checked;
        setSelectAll(isChecked);
        const allIds = isChecked ? teachers.map((teacher) => teacher.id) : [];
        setSelectedTeachers(allIds);
        onSelectionChange(allIds);
    };

    return (
        <FormControl fullWidth>
            <InputLabel>Tanárok</InputLabel>
            <Select
                multiple
                value={selectedTeachers}
                onChange={handleTeacherChange}
                renderValue={(selected) => selected.map(id => teachers.find(teacher => teacher.id === id).name).join(", ")}
            >
                <MenuItem>
                    <FormControlLabel
                        control={
                            <Checkbox
                                checked={selectAll}
                                onChange={handleSelectAllChange}
                            />
                        }
                        label="Összes tanár kiválasztása"
                    />
                </MenuItem>
                {teachers.map((teacher) => (
                    <MenuItem key={teacher.id} value={teacher.id}>
                        <Checkbox checked={selectedTeachers.includes(teacher.id)} />
                        <ListItemText primary={teacher.name} />
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
}

export default TeacherReceiverSelector;
