import React, { useState } from 'react';
import { Button, Stack, Alert, Box } from "@mui/material";
import SubjectSelector from "./SubjectSelector.jsx";
import DateSelector from "./DateSelector.jsx";
import ChooseFromStudentsSelector from "./ChooseFromStudentsSelector.jsx";
import DescriptionSelector from "./DescriptionSelector.jsx";
import OptionalDescriptionSelector from "./OptionalDescriptionSelector.jsx";

function NotificationAdder(props) {
    const [selectedDate, setSelectedDate] = useState("");
    const [selectedSubjectId, setSelectedSubjectId] = useState("")
    const [selectedSubjectName, setSelectedSubjectName] = useState("")
    const [description, setDescription] = useState("");
    const [optionalDescription, setOptionalDescription] = useState("");
    const [allStudents, setAllStudents] = useState([])
    const [students, setStudents] = useState([]);
    const [alertMessage, setAlertMessage] = useState("");

    const dateChangeHandler = (date) => setSelectedDate(date);
    const textChangeHandler = (text) => setDescription(text);
    const selectedStudentsHandler = (students) => setStudents(students);
    const descriptionHandler = (description) => setOptionalDescription(description);

    const addingHandler = (e) => {
        e.preventDefault();

        let isValid = true;
        let newAlertMessage = "";

        if (selectedDate === "") {
            newAlertMessage = "Dátum megadása kötelező!";
            isValid = false;
        } else if (description === "") {
            newAlertMessage = "Leírás megadása kötelező!";
            isValid = false;
        } else if (students.length === 0) {
            newAlertMessage = "Legalább egy diák megadása kötelező!";
            isValid = false;
        } else if (["Exam", "MissingEquipment", "Homework"].includes(props.type) && selectedSubjectName === "") {
            newAlertMessage = "Tantárgy megadása kötelező!";
            isValid = false;
        }

        if (!isValid) {
            setAlertMessage(newAlertMessage);
            return;
        }

        const formattedDate = new Date(selectedDate).toISOString();

        const notificationData = {
            type: props.type,
            teacherId: props.teacherId,
            date: formattedDate,
            subject: selectedSubjectName,
            description: description,
            studentIds: students,
            optionalDescription: optionalDescription,
        };

        fetch('/api/notifications/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(notificationData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('Grade added:', data);
            })
            .catch(error => console.error('Error adding grade:', error));
    };

    const subjectChangeHandler = (subjectId, subjectName) => {
        setSelectedSubjectId(subjectId);
        setSelectedSubjectName(subjectName);
        fetch(`/api/teacherSubjects/getStudentsByTeacherSubjectId/${subjectId}`)
            .then(response => response.json())
            .then(data => {
                setAllStudents(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    };

    return (
        <Box sx={{ padding: 2 }}>
            <Stack spacing={2} direction="column">
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <DateSelector selectedDate={selectedDate} onDateChange={dateChangeHandler} />
                    <SubjectSelector teacherSubjects={props.teacherSubjects} selectedSubjectId ={selectedSubjectId} onSubjectChange={subjectChangeHandler} />
                </Box>
                <DescriptionSelector type={props.type} onDescriptionChange={textChangeHandler} />
                <ChooseFromStudentsSelector students={allStudents} onStudentChange={selectedStudentsHandler} />
                {props.type !== "Other" && <OptionalDescriptionSelector onOptionalDescriptionChange={descriptionHandler} />}
                <Button
                    variant="contained"
                    color="primary"
                    sx={{ marginTop: 2 }}
                    onClick={addingHandler}
                >
                    {props.type === "Homework" ? "Házi feladat hozzáadása" :
                        props.type === "Other" ? "Egyéb értesítés hozzáadása" :
                            props.type === "Exam" ? "Dolgozat-értesítés hozzáadása" :
                                props.type === "MissingEquipment" ? "Felszerelés-hiány hozzáadása" : "Hozzáadás"}
                </Button>
                {alertMessage && (
                    <Alert severity="error" sx={{ marginTop: 2 }}>
                        {alertMessage}
                    </Alert>
                )}
            </Stack>
        </Box>
    );
}

export default NotificationAdder;
