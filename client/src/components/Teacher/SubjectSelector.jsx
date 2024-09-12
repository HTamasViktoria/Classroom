import { FormControl, InputLabel, Select, MenuItem, Box } from "@mui/material";

function SubjectSelector(props) {
    const handleSubjectChange = (event) => {
        props.onSubjectChange(event.target.value);
    };

    return (
        <Box sx={{ width: '48%' }}>
            <FormControl fullWidth variant="outlined">
                <InputLabel id="subject-select-label">Tantárgy:</InputLabel>
                <Select
                    labelId="subject-select-label"
                    value={props.selectedSubject}
                    onChange={handleSubjectChange}
                    label="Tantárgy"
                >
                    {["Nyelvtan", "Irodalom", "Matematika", "Környezetismeret"].map((subject) => (
                        <MenuItem key={subject} value={subject}>
                            {subject}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </Box>
    );
}

export default SubjectSelector;
