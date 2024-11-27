import React, {useState} from 'react';
import { FormControl, MenuItem } from "@mui/material";
import {StyledInputLabel, StyledSelect, SubjectChooseContainer } from '../../../StyledComponents';
import TeacherSubjectsFetcher from "./TeacherSubjectsFetcher.jsx";
function SubjectSelector(props) {
    
    const [teacherSubjects, setTeacherSubjects] = useState([])
    const handleSubjectChange = (event) => {
        const selectedId = event.target.value;
        const selectedSubjectObj = teacherSubjects.find(subject => subject.id === selectedId);

        if (selectedSubjectObj) {
            props.onSubjectChange(selectedSubjectObj.id, selectedSubjectObj.subject);
        }
    };

    return (<>
            <TeacherSubjectsFetcher teacherId={props.teacherId} onData={(data) => setTeacherSubjects(data)}/>
            <SubjectChooseContainer>
                <FormControl>
                    <StyledInputLabel id="subject-select-label">Tantárgy:</StyledInputLabel>
                    <StyledSelect
                        labelId="subject-select-label"
                        value={props.selectedSubjectId || ""}
                        onChange={handleSubjectChange}
                        label="Tantárgy"
                    >
                        {teacherSubjects.map((teacherSubject) => (
                            <MenuItem key={teacherSubject.id} value={teacherSubject.id}>
                                {teacherSubject.subject}
                            </MenuItem>
                        ))}
                    </StyledSelect>
                </FormControl>
            </SubjectChooseContainer>
        </>
    );
}

export default SubjectSelector;
