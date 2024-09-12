import { useEffect, useState } from "react";
import { Box, Container, FormControl, InputLabel, MenuItem, Select, Typography } from "@mui/material";

function GradeValueSelector({ selectedGrade, handleGradeChange }) {
    const [grades, setGrades] = useState([]);

    useEffect(() => {
        fetch('/api/grades/gradeValues')
            .then(response => response.json())
            .then(data => setGrades(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    return (
        <Container>
            <Box my={4}>
                <Typography variant="h4" gutterBottom>
                    Jegy kiválasztása
                </Typography>
                <FormControl fullWidth variant="outlined">
                    <InputLabel id="grade-select-label">Jegy kiválasztása</InputLabel>
                    <Select
                        labelId="grade-select-label"
                        value={selectedGrade}
                        onChange={handleGradeChange}
                        label="Select Grade"
                    >
                        {grades.length > 0 ? (
                            grades.map((grade, index) => (
                                <MenuItem key={index} value={grade}>
                                    {grade}
                                </MenuItem>
                            ))
                        ) : (
                            <MenuItem disabled>No grades available</MenuItem>
                        )}
                    </Select>
                </FormControl>
            </Box>
        </Container>
    );
}

export default GradeValueSelector;
