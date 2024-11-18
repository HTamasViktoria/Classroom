import { useEffect, useState } from "react";
import { Box, Container, InputLabel, MenuItem, Select, Typography } from "@mui/material";
import {BulkFormControl, BulkSelect} from '../../../StyledComponents.js'

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
            <Box>
                <BulkFormControl>
                    <InputLabel id="grade-select-label">Jegy kiválasztása</InputLabel>
                    <BulkSelect
                        labelId="grade-select-label"
                        value={selectedGrade}
                        onChange={handleGradeChange}
                        label="Jegy kiválasztása"                 
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
                    </BulkSelect>
                </BulkFormControl>
            </Box>
        </Container>
    );
}

export default GradeValueSelector;