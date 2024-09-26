import React, {useEffect, useState} from 'react';
import ParentNotificationsMain from "./ParentNotificationsMain.jsx";
import ExamNotifications from "./ExamNotifications.jsx";
import HomeworkNotifications from "./HomeworkNotifications.jsx";
import MissingEquipNotifications from "./MissingEquipNotifications.jsx";
import OtherNotifications from "./OtherNotifications.jsx";


function ParentNotifications(props) {  
   
 const [chosenType, setChosenType] = useState("")
  
    const resfresHandler=()=>{
     props.onRefreshing();
    }

    const choosingHandler = (type) =>{
     setChosenType(type)         
    }

    return (
       <>
           {chosenType === "exams" && <ExamNotifications exams={props.notifications.filter(n=>n.type=== "Exam")} />}
           {chosenType === "missingEquipments" && <MissingEquipNotifications missingEquipments={props.notifications.filter(n=>n.type=== "MissingEquipments")} />}
           {chosenType === "homeworks" && <HomeworkNotifications onRefreshing={resfresHandler} homeworks={props.notifications.filter(n=>n.type=== "Homework")} />}
           {chosenType === "others" && <OtherNotifications others={props.notifications.filter(n=>n.type=== "Other")} />}
           {chosenType === "" &&<ParentNotificationsMain studentId={props.studentId} notifications={props.notifications} onChoosing={choosingHandler}/>}
       </>
    );
}

export default ParentNotifications;
