import { FormControl, InputLabel, Select, MenuItem, Box } from "@mui/material";

function SubjectSelector(props) {
    const handleSubjectChange = (event) => {
        const selectedId = event.target.value;
        const selectedSubject = props.teacherSubjects.find(subject => subject.id === selectedId);

        if (selectedSubject) {
            props.onSubjectChange(selectedSubject.id, selectedSubject.subject);
        }
    };

    return (
        <Box sx={{ width: '48%' }}>
            <FormControl fullWidth variant="outlined">
                <InputLabel id="subject-select-label">Tantárgy:</InputLabel>
                <Select
                    labelId="subject-select-label"
                    value={props.selectedSubjectId || ""}
                    onChange={handleSubjectChange}
                    label="Tantárgy"
                >
                    {props.teacherSubjects.map((teacherSubject) => (
                        <MenuItem key={teacherSubject.id} value={teacherSubject.id}>
                            {teacherSubject.subject}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
        </Box>
    );
}

export default SubjectSelector;
