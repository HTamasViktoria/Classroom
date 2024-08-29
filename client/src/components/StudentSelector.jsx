import { FormControl, InputLabel, Select, MenuItem, Box, Typography, Container } from "@mui/material";

function StudentSelector({ students, selectedStudentId, handleStudentChange }) {
    return (
        <Container>
            <Box my={4}>
                <Typography variant="h4" gutterBottom>
                    Diák kiválasztása
                </Typography>
                <FormControl fullWidth variant="outlined">
                    <InputLabel id="student-select-label">Select Student</InputLabel>
                    <Select
                        labelId="student-select-label"
                        value={selectedStudentId}
                        onChange={handleStudentChange}
                        label="Select Student"
                    >
                        {students.length > 0 ? (
                            students.map((student) => (
                                <MenuItem key={student.id} value={student.id}>
                                    {student.familyName} {student.firstName}
                                </MenuItem>
                            ))
                        ) : (
                            <MenuItem disabled>No students available</MenuItem>
                        )}
                    </Select>
                </FormControl>
            </Box>
        </Container>
    );
}

export default StudentSelector;
