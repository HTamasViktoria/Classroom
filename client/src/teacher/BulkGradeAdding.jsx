import React, { useState } from "react";
import {BulkContainer,LeftInnerBox, RightInnerBox, BulkBox} from "../../StyledComponents.js";
import BulkGradeStudents from "../components/Teacher/BulkGradeStudents.jsx";
import DateSelector from "../components/Teacher/DateSelector.jsx";
import ForWhatSelector from "../components/Teacher/ForWhatSelector.jsx";
import { useParams } from "react-router-dom";
import SubjectSelector from "../components/Teacher/SubjectSelector.jsx";

function BulkGradeAdding() {
    const { id } = useParams();

    const [selectedSubjectId, setSelectedSubjectId] = useState("");
    const [selectedDate, setSelectedDate] = useState("");
    const [selectedForWhat, setSelectedForWhat] = useState("");
    const [selectedSubjectName, setSelectedSubjectName] = useState("");

    const forWhatChangeHandler = (e) => {
        setSelectedForWhat(e.target.value);
    };

    const handleSubjectChange = (subjectId, subjectName) => {
        setSelectedSubjectId(subjectId);
        setSelectedSubjectName(subjectName);
    };

    return (
        <BulkContainer>
            <BulkBox>               
                <LeftInnerBox>
                    <DateSelector
                        selectedDate={selectedDate}
                        onDateChange={(e) => setSelectedDate(e)}
                    />

                    <SubjectSelector
                        teacherId={id}
                        selectedSubjectId={selectedSubjectId}
                        onSubjectChange={handleSubjectChange}
                    />

                    <ForWhatSelector
                        selectedForWhat={selectedForWhat}
                        handleForWhatChange={forWhatChangeHandler}
                    />
                </LeftInnerBox>            
                <RightInnerBox>
                    <BulkGradeStudents
                        selectedSubjectId={selectedSubjectId}
                        selectedDate={selectedDate}
                        selectedForWhat={selectedForWhat}
                        teacherId={id}
                        selectedSubjectName={selectedSubjectName}
                    />
                </RightInnerBox>
            </BulkBox>
        </BulkContainer>
    );
}

export default BulkGradeAdding;
