import React, {useEffect, useState} from 'react';
import TeacherNavbar from "./TeacherNavbar.jsx";
import GradeAddingForm from "./GradeAddingForm.jsx";
import TaskSelector from "./TaskSelector.jsx";
import NotificationMain from "./NotificationMain.jsx";

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
            { chosenTask == "" &&(  <TaskSelector onChosenTask={taskHandler}/>)}
            {chosenTask == "addNotification" && <NotificationMain teacherSubjects={teacherSubjects} teacherId={props.teacherId} teacherName={props.teacherName} onGoBack={goBackHandler}/>}
            {chosenTask == "addGrade" && <GradeAddingForm teacherSubjects={teacherSubjects} teacherId={props.teacherId} onGoBack={goBackHandler} />}
            {chosenTask == "addMessage" && <div>Adding messages</div>}
                
           
            
            {chosenTask === "grade" && <GradeAddingForm />}
         
        </>
    );
}

export default Tasks;
