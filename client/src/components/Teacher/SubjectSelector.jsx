import React from 'react';
import { FormControl, MenuItem } from "@mui/material";
import { CustomBox, StyledInputLabel, StyledSelect } from '../../../StyledComponents';

function SubjectSelector(props) {
    const handleSubjectChange = (event) => {
        const selectedId = event.target.value;
        const selectedSubject = props.teacherSubjects.find(subject => subject.id === selectedId);

        if (selectedSubject) {
            props.onSubjectChange(selectedSubject.id, selectedSubject.subject);
        }
    };

    return (
        <CustomBox sx={{ width: '48%' }}>
            <FormControl fullWidth variant="outlined">
                <StyledInputLabel id="subject-select-label">Tantárgy:</StyledInputLabel>
                <StyledSelect
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
                </StyledSelect>
            </FormControl>
        </CustomBox>
    );
}

export default SubjectSelector;
