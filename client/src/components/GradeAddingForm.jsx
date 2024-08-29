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

    const handleStudentChange = (e) => setSelectedStudentId(e.target.value);
    const handleSubjectChange = (e) => setSelectedSubject(e.target.value);
    const handleGradeChange = (e) => setSelectedGrade(e.target.value);
    const handleDateChange = (e) => setSelectedDate(e.target.value);

    const handleSubmit = (e) => {
        e.preventDefault();

        // ISO 8601 formátumban biztosítjuk a dátumot
        const formattedDate = new Date(selectedDate).toISOString();

        const gradeData = {
            teacherId: teacherId.toString(), // Győződj meg róla, hogy string típusú
            studentId: selectedStudentId.toString(), // Győződj meg róla, hogy string típusú
            subject: selectedSubject,
            value: selectedGrade,
            date: formattedDate
        };

        console.log('Submitting data:', gradeData);

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
                    <SubjectSelector selectedSubject={selectedSubject} handleSubjectChange={handleSubjectChange} />
                    <StudentSelector students={students} selectedStudentId={selectedStudentId} handleStudentChange={handleStudentChange} />
                    <GradeValueSelector selectedGrade={selectedGrade} handleGradeChange={handleGradeChange} />
                    <DateSelector selectedDate={selectedDate} handleDateChange={handleDateChange} />
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
