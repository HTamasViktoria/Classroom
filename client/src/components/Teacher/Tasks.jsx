import React, { useState } from 'react';
import TeacherNavbar from "./TeacherNavbar.jsx";
import SubjectSelector from "./SubjectSelector.jsx";
import GradeAddingForm from "./GradeAddingForm.jsx";
import TaskSelector from "./TaskSelector.jsx";
import NotificationMain from "./NotificationMain.jsx";

function Tasks(props) {
    const [chosenTask, setChosenTask] = useState("");

  const taskHandler = (chosenTask) =>{
     setChosenTask(chosenTask)
    }
 

    return (
        <>
            <TeacherNavbar />
            { chosenTask == "" &&(  <TaskSelector onChosenTask={taskHandler}/>)}
            {chosenTask == "addNotification" && <NotificationMain teacherId={props.teacherId}/>}
            {chosenTask == "addGrade" && <GradeAddingForm teacherId={props.teacherId} />}
            {chosenTask == "addMessage" && <div>Adding messages</div>}
                
           
            
            {chosenTask === "grade" && <GradeAddingForm />}
         
        </>
    );
}

export default Tasks;
