import { useState } from "react";
import { FormControl, InputLabel, Select, MenuItem, Checkbox, ListItemText, FormControlLabel } from "@mui/material";

function ParentReceiverSelector({ parents, onSelectionChange }) {
    const [selectedParents, setSelectedParents] = useState([]);
    const [selectAll, setSelectAll] = useState(false);

    const handleParentChange = (event) => {
        const { value } = event.target;
        setSelectedParents(value);
        onSelectionChange(value);
    };

    const handleSelectAllChange = (e) => {
        const isChecked = e.target.checked;
        setSelectAll(isChecked);
        const allIds = isChecked ? parents.map((parent) => parent.id) : [];
        setSelectedParents(allIds);
        onSelectionChange(allIds);
    };

    return (
        <FormControl fullWidth>
            <InputLabel>Szülők:</InputLabel>
            <Select
                multiple
                value={selectedParents}
                onChange={handleParentChange}
                renderValue={(selected) => selected.map(id => parents.find(parent => parent.id === id).name).join(", ")}
            >
                <MenuItem>
                    <FormControlLabel
                        control={
                            <Checkbox
                                checked={selectAll}
                                onChange={handleSelectAllChange}
                            />
                        }
                        label="Összes szülő kiválasztása"
                    />
                </MenuItem>
                {parents.map((parent) => (
                    <MenuItem key={parent.id} value={parent.id}>
                        <Checkbox checked={selectedParents.includes(parent.id)} />
                        <ListItemText primary={parent.name} />
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
}

export default ParentReceiverSelector;
