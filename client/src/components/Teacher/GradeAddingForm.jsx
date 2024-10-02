import { useEffect, useState } from "react";
import { Stack, Typography } from "@mui/material";
import { StyledButton, StyledFormControl, StyledSelect, StyledTextField } from "../../../StyledComponents";
import SubjectSelector from "./SubjectSelector.jsx";
import StudentSelector from "./StudentSelector.jsx";
import DateSelector from "./DateSelector.jsx";
import GradeValueSelector from "./GradeValueSelector.jsx";
import { useNavigate } from 'react-router-dom';

function GradeAddingForm({ teacherId, onGoBack }) {
    const [students, setStudents] = useState([]);
    const [teacherSubjects, setTeacherSubjects] = useState([]);
    const [selectedStudentId, setSelectedStudentId] = useState("");
    const [selectedSubjectId, setSelectedSubjectId] = useState("");
    const [selectedSubjectName, setSelectedSubjectName] = useState("");
    const [selectedGrade, setSelectedGrade] = useState("");
    const [selectedDate, setSelectedDate] = useState("");

    const navigate = useNavigate();

    useEffect(() => {
        fetch(`/api/teacherSubjects/getByTeacherId/${teacherId}`)
            .then(response => response.json())
            .then(data => {
                setTeacherSubjects(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [teacherId]);

    const studentChangeHandler = (e) => setSelectedStudentId(e.target.value);

    const subjectChangeHandler = (subjectId, subjectName) => {
        setSelectedSubjectId(subjectId);
        setSelectedSubjectName(subjectName);
        fetch(`/api/teacherSubjects/getStudentsByTeacherSubjectId/${subjectId}`)
            .then(response => response.json())
            .then(data => {
                setStudents(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    };

    const gradeChangeHandler = (e) => setSelectedGrade(e.target.value);
    const dateChangeHandler = (e) => setSelectedDate(e);

    const handleSubmit = (e) => {
        e.preventDefault();

        const formattedDate = new Date(selectedDate).toISOString();

        const gradeData = {
            teacherId: teacherId.toString(),
            studentId: selectedStudentId.toString(),
            subject: selectedSubjectName,
            value: selectedGrade,
            date: formattedDate
        };

        fetch('/api/grades/add', {
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
                onGoBack();
            })
            .catch(error => console.error('Error adding grade:', error));
    };

    return (
        <>
            <Typography variant="h4" gutterBottom>
                Jegy hozzáadása
            </Typography>
            <form noValidate onSubmit={handleSubmit}>
                <Stack spacing={2} width={400}>
                    <SubjectSelector
                        teacherSubjects={teacherSubjects}
                        selectedSubjectId={selectedSubjectId}
                        onSubjectChange={subjectChangeHandler}
                    />
                    <StudentSelector
                        selectedStudentId={selectedStudentId}
                        students={students}
                        handleStudentChange={studentChangeHandler}
                    />
                    <GradeValueSelector
                        selectedGrade={selectedGrade}
                        handleGradeChange={gradeChangeHandler}
                    />
                    <DateSelector
                        selectedDate={selectedDate}
                        onDateChange={dateChangeHandler}
                    />
                    <StyledButton type='submit'>
                        Jegy hozzáadása
                    </StyledButton>
                    <StyledButton
                        type='button'
                        onClick={onGoBack}
                        sx={{ backgroundColor: 'secondary.main' }}
                    >
                        Mégse
                    </StyledButton>
                </Stack>
            </form>
        </>
    );
}

export default GradeAddingForm;
