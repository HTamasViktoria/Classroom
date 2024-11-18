import React, { useState } from 'react';
import {useParams} from "react-router-dom";
import TeacherNavbar from "./TeacherNavbar.jsx";
import NotificationSelector from "./NotificationSelector.jsx";
import NotificationAdder from "./NotificationAdder.jsx";
import { useProfile } from "../../contexts/ProfileContext.jsx";
import {useNavigate} from "react-router-dom";


function NotificationMain() {

    const {id} = useParams()
    const navigate = useNavigate()
    const {profile, logout} = useProfile();
       
    
    const [chosenNotificationType, setChosenNotificationType] = useState("")

  
 const typeHandler = (type) =>{setChosenNotificationType(type)}
    const successfulAddingHandler = () => {
        setChosenNotificationType("");
    };
    
  
    
    return (
        <>
            <TeacherNavbar id={id} />
            {chosenNotificationType === "" && (<NotificationSelector teacherId={id}  
                                                                     onChosenType={typeHandler} 
                                                                     onGoBack={()=>navigate(`/teacher/notifications/${id}`)}/> )}
           
            {chosenNotificationType !== "" && (<NotificationAdder teacherId={id} 
                                                                  teacherName={`${profile.familyName} ${profile.firstName}`}
                                                                  type={chosenNotificationType} 
                                                                  onSuccessfulAdding={successfulAddingHandler}/>)}
        </>
    );
}

export default NotificationMain;
