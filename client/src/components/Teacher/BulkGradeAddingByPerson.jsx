import {useState } from "react";
import { BulkContainer, StyledBox } from "../../../StyledComponents.js";
import GradeValueSelector from "./GradeValueSelector.jsx";
import { AButton } from "../../../StyledComponents.js";

function BulkGradeAddingByPerson({ selectedDate, selectedSubjectName, teacherId, selectedForWhat, student }) {
    const [selectedGrade, setSelectedGrade] = useState("");
    const [successfullyAdded, setSuccessfullyAdded] = useState(false);

    const handleSubmit = (e) => {
        e.preventDefault();

        const formattedDate = new Date(selectedDate).toISOString();

        const gradeData = {
            teacherId: teacherId,
            studentId: student.id,
            subject: selectedSubjectName,
            forWhat: selectedForWhat,
            value: selectedGrade,
            date: formattedDate
        };

        fetch('/api/grades', {
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
                setSuccessfullyAdded(true);
            })
            .catch(error => console.error('Error adding grade:', error));
    };

    const formatGrade = (grade) => {
        const [stringGrade, numberGrade] = grade.split(' = ');
        return `${stringGrade}(${numberGrade})`;
    };

    const gradeChangeHandler = (e) => setSelectedGrade(e.target.value);

    return (
        <>
            {successfullyAdded ? (
                <div>{`Jegy sikeresen hozzáadva: ${formatGrade(selectedGrade)}`}</div>
            ) : (
                <BulkContainer>
                    <StyledBox>
                        <form onSubmit={handleSubmit}>
                            <GradeValueSelector selectedGrade={selectedGrade} handleGradeChange={gradeChangeHandler} />
                            <AButton type="submit">Jegy hozzáadása</AButton>
                        </form>
                    </StyledBox>
                </BulkContainer>
            )}
        </>
    );
}

export default BulkGradeAddingByPerson;
