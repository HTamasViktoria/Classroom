import { FormControl, InputLabel, Select, MenuItem, Box, Typography, Container } from "@mui/material";

function SubjectSelector({ selectedSubject, handleSubjectChange }) {
    return (
        <Container>
            <Box my={4}>
                <Typography variant="h4" gutterBottom>
                    Tantárgy kiválasztása
                </Typography>
                <FormControl fullWidth variant="outlined">
                    <InputLabel id="subject-select-label">Tantárgy:</InputLabel>
                    <Select
                        labelId="subject-select-label"
                        value={selectedSubject}
                        onChange={handleSubjectChange}
                        label="Select Subject"
                    >
                        {["Nyelvtan", "Irodalom", "Matematika", "Környezetismeret"].map((subject) => (
                            <MenuItem key={subject} value={subject}>
                                {subject}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </Box>
        </Container>
    );
}

export default SubjectSelector;
