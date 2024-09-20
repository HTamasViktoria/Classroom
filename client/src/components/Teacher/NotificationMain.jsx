import React, { useState } from 'react';
import TeacherNavbar from "./TeacherNavbar.jsx";
import { FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import NotificationSelector from "./NotificationSelector.jsx";
import NotificationAdder from "./NotificationAdder.jsx";



function NotificationMain({teacherId, teacherSubjects}) {

    const [chosenNotificationType, setChosenNotificationType] = useState("")

 const typeHandler = (type) =>{setChosenNotificationType(type)}
    
    return (
        <>
            <TeacherNavbar />
            {chosenNotificationType === "" && (<NotificationSelector teacherId={teacherId}  onChosenType={typeHandler}/>)}
            {chosenNotificationType != "" && (<NotificationAdder teacherId={teacherId} teacherSubjects={teacherSubjects} type={chosenNotificationType}/>)}
        </>
    );
}

export default NotificationMain;
