import { FormControl, MenuItem, Typography } from "@mui/material";
import { CustomBox, StyledInputLabel, StyledSelect } from '../../../StyledComponents';

function StudentSelector({ students, selectedStudentId, handleStudentChange }) {
    return (
        <CustomBox sx={{ my: 4, width: '100%' }}>
            <Typography variant="h4" gutterBottom>
                Diák kiválasztása
            </Typography>
            <FormControl fullWidth variant="outlined">
                <StyledInputLabel id="student-select-label">Válassz Diákot</StyledInputLabel>
                <StyledSelect
                    labelId="student-select-label"
                    value={selectedStudentId}
                    onChange={handleStudentChange}
                    label="Válassz Diákot"
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
                </StyledSelect>
            </FormControl>
        </CustomBox>
    );
}

export default StudentSelector;
