import { MenuItem} from "@mui/material";
import { CustomBox, StyledInputLabel, StyledSelect, StudentsHeading, StudentsFormControl } from '../../../StyledComponents';
import StudentsOfATeacherSubjectFetcher from "./StudentsOfATeacherSubjectFetcher.jsx";
import React, {useState} from 'react';
function StudentSelector({selectedStudentId, handleStudentChange, selectedSubjectId }) {
    
    const [students, setStudents] = useState([])
    
    return (<>
            <StudentsOfATeacherSubjectFetcher selectedSubjectId={selectedSubjectId}
            onData={(data)=>setStudents(data)}/>
            
        <CustomBox>
            <StudentsHeading>
                Diák kiválasztása
            </StudentsHeading>
            <StudentsFormControl>
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
            </StudentsFormControl>
        </CustomBox>
        </>
    );
}

export default StudentSelector;
