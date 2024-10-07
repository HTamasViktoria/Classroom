import React, {useEffect, useState} from 'react';
import TeacherNavbar from "./TeacherNavbar.jsx";
import TeacherGrades from "./TeacherGrades.jsx";
import GradeAddingForm from "./GradeAddingForm.jsx";
import TaskSelector from "./TaskSelector.jsx";
import NotificationMain from "./NotificationMain.jsx";
import BulkGradeAdding from "./BulkGradeAdding.jsx";

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
 
    const goBackHandler=()=>{
      setChosenTask("");
    }

    return (
        <>
            <TeacherNavbar />
            { chosenTask === "" &&(  <TaskSelector onChosenTask={taskHandler}/>)}
            {chosenTask === "addNotification" && <NotificationMain teacherSubjects={teacherSubjects} teacherId={props.teacherId} teacherName={props.teacherName} onGoBack={goBackHandler}/>}
            {chosenTask === "grades" && <TeacherGrades teacherSubjects={teacherSubjects} teacherId={props.teacherId} onGoBack={goBackHandler} onChosenTask={taskHandler} />}
            {chosenTask === "addMessage" && <div>Adding messages</div>}
            {chosenTask === "addGrades" && <GradeAddingForm teacherSubjects={teacherSubjects} teacherId={props.teacherId} onGoBack={goBackHandler}/>}
            {chosenTask === "addingBulkGrades" && <BulkGradeAdding teacherSubjects={teacherSubjects} teacherId={props.teacherId} onGoBack={goBackHandler}/>}   
            
         
        </>
    );
}

export default Tasks;
