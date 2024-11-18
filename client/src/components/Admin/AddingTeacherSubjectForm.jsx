import React, { useState } from "react";
import TeacherSubjectSelector from "./TeacherSubjectSelector.jsx";
import ClassSelector from "./ClassSelector.jsx";
import { AButton, StyledStack, StyledHeading } from "../../../StyledComponents";

function AddingTeacherSubjectForm({
                                      teacher,
                                      subjects,
                                      classes,
                                      selectedSubject,
                                      selectedClass,
                                      handleSubjectClick,
                                      handleClassClick,
                                      handleSubmit
                                  }) {
    const [localSubject, setLocalSubject] = useState(selectedSubject);

    const handleLocalSubjectClick = (subject) => {
        setLocalSubject(subject);
        handleSubjectClick(subject);
    };

    return (
        <div>
            <StyledHeading>
                {`Tantárgy hozzáadása ${teacher.familyName} ${teacher.firstName} tanárhoz`}
            </StyledHeading>

            <StyledStack>
                <TeacherSubjectSelector
                    subjects={subjects}
                    selectedSubject={localSubject}
                    handleSubjectClick={handleLocalSubjectClick}
                />

                <ClassSelector
                    classes={classes}
                    selectedClass={selectedClass}
                    handleClassClick={handleClassClick}
                />
            </StyledStack>

            <AButton onClick={handleSubmit}>
                Hozzáad
            </AButton>
        </div>
    );
}

export default AddingTeacherSubjectForm;
