import React, { useState } from 'react';
import TeacherNavbar from "./TeacherNavbar.jsx";
import NotificationSelector from "./NotificationSelector.jsx";
import NotificationAdder from "./NotificationAdder.jsx";



function NotificationMain({teacherId, teacherSubjects, teacherName, onGoBack}) {

    const [chosenNotificationType, setChosenNotificationType] = useState("")

 const typeHandler = (type) =>{setChosenNotificationType(type)}
    const successfulAddingHandler = () => {
        setChosenNotificationType("");
    };
    
    const goBackHandler=()=>{
        onGoBack();
    }
    
    return (
        <>
            <TeacherNavbar />
            {chosenNotificationType === "" && (<NotificationSelector teacherId={teacherId}  onChosenType={typeHandler} onGoBack={goBackHandler}/> )}
            {chosenNotificationType !== "" && (<NotificationAdder teacherId={teacherId} teacherName={teacherName} teacherSubjects={teacherSubjects} type={chosenNotificationType} onSuccessfulAdding={successfulAddingHandler}/>)}
        </>
    );
}

export default NotificationMain;
