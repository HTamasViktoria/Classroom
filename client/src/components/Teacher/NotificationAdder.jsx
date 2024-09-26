import React, { useState } from 'react';
import { Button, Stack, Alert, Box } from "@mui/material";
import SubjectSelector from "./SubjectSelector.jsx";
import DateSelector from "./DateSelector.jsx";
import ChooseFromStudentsSelector from "./ChooseFromStudentsSelector.jsx";
import DescriptionSelector from "./DescriptionSelector.jsx";
import OptionalDescriptionSelector from "./OptionalDescriptionSelector.jsx";
import ButtonText from "./buttonText.js";
import validateForm from "./validateForm.js";

function NotificationAdder(props) {
    const [state, setState] = useState({
        selectedDate: "",
        selectedSubjectId: "",
        selectedSubjectName: "",
        description: "",
        optionalDescription: "",
        allStudents: [],
        students: [],
        alertMessage: "",
    });

    const handleChange = (key, value) => {
        setState(prevState => ({ ...prevState, [key]: value }));
    };

    const dateChangeHandler = (date) => handleChange('selectedDate', date);
    const textChangeHandler = (text) => handleChange('description', text);
    const selectedStudentsHandler = (students) => handleChange('students', students);
    const descriptionHandler = (description) => handleChange('optionalDescription', description);

    const subjectChangeHandler = (subjectId, subjectName) => {
        handleChange('selectedSubjectId', subjectId);
        handleChange('selectedSubjectName', subjectName);

        fetch(`/api/teacherSubjects/getStudentsByTeacherSubjectId/${subjectId}`)
            .then(response => response.json())
            .then(data => handleChange('allStudents', data))
            .catch(error => console.error('Error fetching data:', error));
    };

    const addingHandler = (e) => {
        e.preventDefault();
        const validationMessage = validateForm(state, props.type);
        if (validationMessage) {
            handleChange('alertMessage', validationMessage);
            return;
        }

        const formattedDate = new Date(state.selectedDate).toISOString();
        const notificationData = {
            type: props.type,
            teacherId: props.teacherId,
            teacherName: props.teacherName,
            date: new Date().toISOString(),
            dueDate: formattedDate,
            subject: state.selectedSubjectName,
            description: state.description,
            studentIds: state.students,
            read: false,
            optionalDescription: state.optionalDescription,
        };

        fetch('/api/notifications/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(notificationData),
        })
            .then(response => {
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                return response.json();
            })
            .then(data => {
                console.log('Grade added:', data);
                props.onSuccessfulAdding();
            })
            .catch(error => console.error('Error adding grade:', error));
    };

    return (
        <Box sx={{ padding: 2, width: '100%', maxWidth: '900px', margin: '0 auto' }}>
            <Stack spacing={2} direction="column">
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <DateSelector
                        selectedDate={state.selectedDate}
                        onDateChange={dateChangeHandler}
                    />
                    <SubjectSelector
                        teacherSubjects={props.teacherSubjects}
                        selectedSubjectId={state.selectedSubjectId}
                        onSubjectChange={subjectChangeHandler}
                    />
                </Box>

                <DescriptionSelector
                    type={props.type}
                    onDescriptionChange={textChangeHandler}
                />

                <ChooseFromStudentsSelector
                    students={state.allStudents}
                    onStudentChange={selectedStudentsHandler}
                />

                {props.type !== "Other" && (
                    <OptionalDescriptionSelector
                        onOptionalDescriptionChange={descriptionHandler}
                    />
                )}

                <Button
                    variant="contained"
                    sx={{
                        backgroundColor: '#82b2b8',
                        color: '#fff',
                        '&:hover': {
                            backgroundColor: '#6e9ea4',
                        },
                    }}
                    onClick={addingHandler}
                >
                    <ButtonText type={props.type} />
                </Button>

                {state.alertMessage && (
                    <Alert severity="error" sx={{ marginTop: 2 }}>
                        {state.alertMessage}
                    </Alert>
                )}
            </Stack>
        </Box>
    );
}

export default NotificationAdder;
