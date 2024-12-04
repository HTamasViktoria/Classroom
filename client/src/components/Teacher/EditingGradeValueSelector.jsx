import { Box, Container, FormControl, InputLabel, MenuItem, Select, Typography } from "@mui/material";

function EditingGradeValueSelector({ selectedGrade, handleGradeChange, grades }){
    return(<> <Container>
        <Box my={4}>
            <Typography variant="h6" gutterBottom>
                Jegy kiválasztása
            </Typography>
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
    </Container></>)
}

export default EditingGradeValueSelector