import { useEffect, useState } from 'react';
import { Button, Stack } from "@mui/material";
import StudentSelector from "../Teacher/StudentSelector.jsx";


function StudentAddingToClass(props) {
    const [students, setStudents] = useState([]);
    const [selectedStudentId, setSelectedStudentId] = useState('');

    useEffect(() => {
        fetch('/api/students')
            .then(response => response.json())
            .then(data => setStudents(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    const handleStudentChange = (e) => {
        setSelectedStudentId(e.target.value);
    };

    const handleSubmit = (e) => {

        const addingStudentToClassData = {
            studentId: selectedStudentId, 
            classId: props.classId
        };
        e.preventDefault();
        fetch('/api/classes/addStudent', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(addingStudentToClassData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('Student added:', data);
                props.onSuccessfulAdding();
            })
            .catch(error => console.error('Error adding student:', error));
    
    };

    return (
        <>
            <h1>Diák hozzáadása az {props.className} osztályhoz</h1>
            <form noValidate onSubmit={handleSubmit}>
                <Stack spacing={2} width={400}>
                    <StudentSelector
                        selectedStudentId={selectedStudentId}
                        students={students}
                        handleStudentChange={handleStudentChange}
                    />
                    <Button
                        type='submit'
                        variant='contained'
                        sx={{backgroundColor: '#b5a58d', '&:hover': {backgroundColor: '#b8865a'}}}
                    >
                        Hozzáad
                    </Button>
                </Stack>
            </form>
        </>
    );
}

export default StudentAddingToClass;
