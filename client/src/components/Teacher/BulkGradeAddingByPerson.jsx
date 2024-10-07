import { useEffect, useState } from "react";
import { Box, Container, FormControl, InputLabel, MenuItem, Select, Typography } from "@mui/material";
import {StyledButton} from "../../../StyledComponents.js";

function BulkGradeAddingByPerson({selectedDate, selectedSubjectName, teacherId, selectedForWhat, student}) {
    const [grades, setGrades] = useState([]);
    const [selectedGrade, setSelectedGrade] = useState("");
    
    useEffect(() => {
        fetch('/api/grades/gradeValues')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Hiba történt az adatok betöltésekor.');
                }
                return response.json();
            })
            .then(data => {
                console.log(`selectedSubjectName a bulkgradeaddingbyperson-ban: ${selectedSubjectName}`)
                setGrades(data)
            })
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    const handleGradeChange = (e) => {
        setSelectedGrade(e.target.value);
    };



    const handleSubmit = (e) => {
        e.preventDefault();

        const formattedDate = new Date(selectedDate).toISOString();

        const gradeData = {
            teacherId: teacherId.toString(),
            studentId: student.id.toString(),
            subject: selectedSubjectName,
            forWhat: selectedForWhat,
            value: selectedGrade,
            date: formattedDate
        };
        console.log(gradeData)

        fetch('/api/grades/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(gradeData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('Grade added:', data);
            })
            .catch(error => console.error('Error adding grade:', error));
    };


    return (
        <Container>
            <Box my={4}>
                <Typography variant="h6" gutterBottom>
                    Jegy kiválasztása
                </Typography>
                <form onSubmit={handleSubmit}>
                    <FormControl fullWidth variant="outlined" sx={{ mb: 2 }}>
                        <InputLabel id="grade-select-label">Jegy kiválasztása</InputLabel>
                        <Select
                            labelId="grade-select-label"
                            value={selectedGrade}
                            onChange={handleGradeChange}
                            label="Jegy kiválasztása"
                            sx={{
                                backgroundColor: 'background.default',
                                color: 'text.primary',
                                '& .MuiOutlinedInput-notchedOutline': {
                                    borderColor: 'primary.main',
                                },
                                '&:hover .MuiOutlinedInput-notchedOutline': {
                                    borderColor: 'primary.dark',
                                },
                                '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                                    borderColor: 'primary.dark',
                                },
                            }}
                        >
                            {grades.length > 0 ? (
                                grades.map((grade) => (
                                    <MenuItem key={grade.id || grade} value={grade}>
                                        {grade}
                                    </MenuItem>
                                ))
                            ) : (
                                <MenuItem disabled>No grades available</MenuItem>
                            )}
                        </Select>
                        <StyledButton type='submit'>
                            Jegy hozzáadása
                        </StyledButton>
                    </FormControl>
                </form>
            </Box>
        </Container>
    );

}

export default BulkGradeAddingByPerson;
