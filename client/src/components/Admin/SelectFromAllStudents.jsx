import React, { useState, useEffect } from "react";
import { CustomBox, StudentsFormControl, StudentsHeading, StyledInputLabel, StyledSelect } from "../../../StyledComponents.js";
import { MenuItem } from "@mui/material";

function SelectFromAllStudents({ selectedStudentId, handleStudentChange }) {

    const [students, setStudents] = useState([]);

    useEffect(() => {
        fetch(`/api/students/`)
            .then(response => response.json())
            .then(data => setStudents(data))
            .catch(error => console.error(error));
    }, []);

    return (
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
    );
}

export default SelectFromAllStudents;
