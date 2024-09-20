import { Button, Stack } from "@mui/material";
import React, { useState, useEffect } from 'react';

function AddingTeacherSubjectForm(props) {
    const [subjects, setSubjects] = useState([]);
    const [selectedSubject, setSelectedSubject] = useState(null);
    const [classes, setClasses] = useState([]);
    const [selectedClass, setSelectedClass] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        Promise.all([
            fetch('/api/subjects'),
            fetch('/api/classes')
        ])
            .then(([subjectsResponse, classesResponse]) => {
                if (!subjectsResponse.ok || !classesResponse.ok) {
                    throw new Error('Network response was not ok');
                }
                return Promise.all([
                    subjectsResponse.json(),
                    classesResponse.json()
                ]);
            })
            .then(([subjectsData, classesData]) => {
                setSubjects(subjectsData);
                setClasses(classesData);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
                setError(error);
            });
    }, [props.teacher]);

    const handleSubjectClick = (subject) => {
        setSelectedSubject(subject);
    };

    const handleClassClick = (cls) => {
        setSelectedClass(cls);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        
        const teacherSubjectData = {
            subject: selectedSubject,
            teacherId: props.teacher.id,
            classOfStudentsId: selectedClass.id,
        };



        fetch('/api/teacherSubjects/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(teacherSubjectData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('TeacherSubject added:', data);
            })
            .catch(error => console.error('Error adding teachersubject:', error));
    };

    return (
        <Stack spacing={2}>
            {error && <div style={{ color: 'red' }}>Error: {error.message}</div>}
            <div>
                <h3>Subjects</h3>
                {subjects.length > 0 ? (
                    subjects.map(subject => (
                        <Button
                            key={subject}
                            variant={selectedSubject === subject ? "contained" : "outlined"}
                            onClick={() => handleSubjectClick(subject)}
                        >
                            {subject}
                        </Button>
                    ))
                ) : (
                    <div>No subjects available</div>
                )}
            </div>
            <div>
                <h3>Classes</h3>
                {classes.length > 0 ? (
                    classes.map(cls => (
                        <Button
                            key={cls.id}
                            variant={selectedClass === cls ? "contained" : "outlined"}
                            onClick={() => handleClassClick(cls)}
                        >
                            {cls.name}
                        </Button>
                    ))
                ) : (
                    <div>No classes available</div>
                )}
            </div>
            <Button variant="contained" onClick={handleSubmit}>Hozzáad</Button>
        </Stack>
    );
}

export default AddingTeacherSubjectForm;
