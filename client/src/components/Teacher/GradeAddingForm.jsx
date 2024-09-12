import { useEffect, useState } from "react";
import { Button, Stack } from "@mui/material";
import SubjectSelector from "./SubjectSelector.jsx";
import StudentSelector from "./StudentSelector.jsx";
import DateSelector from "./DateSelector.jsx";
import GradeValueSelector from "./GradeValueSelector.jsx";
import { useNavigate } from 'react-router-dom';

function GradeAddingForm({ teacherId }) {
    const [students, setStudents] = useState([]);
    const [selectedStudentId, setSelectedStudentId] = useState("");
    const [selectedSubject, setSelectedSubject] = useState("");
    const [selectedGrade, setSelectedGrade] = useState("");
    const [selectedDate, setSelectedDate] = useState("");

    const navigate = useNavigate();

    useEffect(() => {
        fetch('/api/students')
            .then(response => response.json())
            .then(data => setStudents(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    const studentChangeHandler = (e) => setSelectedStudentId(e.target.value);
    const subjectChangeHandler = (e) => setSelectedSubject(e);
    const gradeChangeHandler = (e) => setSelectedGrade(e.target.value);
    const dateChangeHandler = (e) => setSelectedDate(e);

    const handleSubmit = (e) => {
        e.preventDefault();
        
        const formattedDate = new Date(selectedDate).toISOString();

        const gradeData = {
            teacherId: teacherId.toString(),
            studentId: selectedStudentId.toString(),
            subject: selectedSubject,
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
                navigate("/teacher");
            })
            .catch(error => console.error('Error adding grade:', error));
    };

    return (
        <>
            <h1>Jegy hozzáadása</h1>
            <form noValidate onSubmit={handleSubmit}>
                <Stack spacing={2} width={400}>
                    <SubjectSelector selectedSubject={selectedSubject} onSubjectChange={subjectChangeHandler} />
                    <StudentSelector selectedStudentId={selectedStudentId} students={students} handleStudentChange={studentChangeHandler} />
                    <GradeValueSelector selectedGrade={selectedGrade} handleGradeChange={gradeChangeHandler} />
                    <DateSelector selectedDate={selectedDate} onDateChange={dateChangeHandler} />
                    <Button
                        type='submit'
                        variant='contained'
                        sx={{ backgroundColor: '#b5a58d', '&:hover': { backgroundColor: '#b8865a' } }}
                    >
                        Hozzáad
                    </Button>
                </Stack>
            </form>
        </>
    );
}

export default GradeAddingForm;
