import React, {useEffect, useState} from 'react';
import TeacherNavbar from "./TeacherNavbar.jsx";
import TeacherGrades from "./TeacherGrades.jsx";
import GradeAddingForm from "./GradeAddingForm.jsx";
import TaskSelector from "./TaskSelector.jsx";
import NotificationMain from "./NotificationMain.jsx";
import BulkGradeAdding from "./BulkGradeAdding.jsx";
import TeacherViewingGrades from "./TeacherViewingGrades.jsx";
import {
    StyledButton 
} from "../../../StyledComponents.js";
function Tasks(props) {
    const [chosenTask, setChosenTask] = useState("");
    const [teacherSubjects, setTeacherSubjects] = useState([])

    useEffect(() => {
        fetch(`/api/teacherSubjects/getByTeacherId/${props.teacherId}`)
            .then(response => response.json())
            .then(data => {
                setTeacherSubjects(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [props.teacherId]);
  const taskHandler = (chosenTask) =>{
     setChosenTask(chosenTask)
    }
 
  
    
const chosenNullHandler =()=>{
      setChosenTask("")
}
 

    return (
        <>
            <TeacherNavbar />
            { chosenTask === "" &&(  <TaskSelector onChosenTask={taskHandler}/>)}
            {chosenTask === "addNotification" && <NotificationMain teacherSubjects={teacherSubjects} teacherId={props.teacherId} teacherName={props.teacherName} onGoBack={chosenNullHandler}/>}
            {chosenTask === "grades" && <TeacherGrades teacherSubjects={teacherSubjects} onGoBack={chosenNullHandler} teacherId={props.teacherId}  onChosenTask={taskHandler} />}
            {chosenTask === "addMessage" && <div>Adding messages</div>}
            {chosenTask === "addGrades" && <GradeAddingForm teacherSubjects={teacherSubjects} teacherId={props.teacherId} onGoBack={chosenNullHandler} />}
            {chosenTask === "addingBulkGrades" && <BulkGradeAdding teacherSubjects={teacherSubjects} teacherId={props.teacherId}/>}
            {chosenTask === "viewingGrades" && <TeacherViewingGrades  onGoBack={chosenNullHandler} teacherSubjects={teacherSubjects}/>}
       
        </>
    );
}

export default Tasks;
