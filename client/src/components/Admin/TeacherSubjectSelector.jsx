import { StyledFormControl, StyledInputLabel, StyledMenuItem, StyledSelect } from "../../../StyledComponents.js";
import React from "react";

function TeacherSubjectSelector({ subjects, selectedSubject, handleSubjectClick }) {
    return (
        <StyledFormControl>
            <StyledInputLabel id="select-subject-label">Tantárgy:</StyledInputLabel>
            <StyledSelect
                labelId="select-subject-label"
                value={selectedSubject || ""}
                onChange={(e) => handleSubjectClick(e.target.value)}
            >
                {subjects.length > 0 ? (
                    subjects.map((subject, index) => (
                        <StyledMenuItem key={index} value={subject}>
                            {subject}
                        </StyledMenuItem>
                    ))
                ) : (
                    <StyledMenuItem disabled>No subjects available</StyledMenuItem>
                )}
            </StyledSelect>
        </StyledFormControl>
    );
}

export default TeacherSubjectSelector;
