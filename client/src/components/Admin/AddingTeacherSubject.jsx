import React, { useState, useEffect } from 'react';
import AddingTeacherSubjectForm from "./AddingTeacherSubjectForm.jsx";

function AddingTeacherSubject(props) {
    const [subjects, setSubjects] = useState([]);
    const [selectedSubject, setSelectedSubject] = useState(null);
    const [classes, setClasses] = useState([]);
    const [selectedClass, setSelectedClass] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const subjectsData = await fetchSubjects();
                const classesData = await fetchClasses();
                console.log(subjectsData);
                setSubjects(subjectsData);
                setClasses(classesData);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };
        fetchData();
    }, [props.teacher]);

    const fetchSubjects = async () => {
        try {
            const response = await fetch('/api/subjects');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        } catch (error) {
            console.error('Error fetching subjects:', error);
            throw error;
        }
    };

    const fetchClasses = async () => {
        try {
            const response = await fetch('/api/classes');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        } catch (error) {
            console.error('Error fetching classes:', error);
            throw error;
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!selectedSubject || !selectedClass) {
            console.error('Subject or class not selected');
            return;
        }

        const teacherSubjectData = {
            subject: selectedSubject,
            teacherId: props.teacher.id,
            classOfStudentsId: selectedClass.id,
            className: `${selectedClass.grade} ${selectedClass.section}`
        };

        try {
            const response = await fetch('/api/teachersubjects', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(teacherSubjectData)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            console.log('TeacherSubject added:', data);
        } catch (error) {
            console.error('Error adding teachersubject:', error);
        } finally {
            props.onSuccessfulAdding();
        }
    };

    const handleSubjectClick = (subjectName) => {
        setSelectedSubject(subjectName);
    };

    const handleClassClick = (cls) => {
        setSelectedClass(cls);
    };

    return (
        <>
            <AddingTeacherSubjectForm
                teacher={props.teacher}
                subjects={subjects}
                classes={classes}
                selectedSubject={selectedSubject}
                selectedClass={selectedClass}
                handleSubjectClick={handleSubjectClick}
                handleClassClick={handleClassClick}
                handleSubmit={handleSubmit}
            />
        </>
    );
}

export default AddingTeacherSubject;
