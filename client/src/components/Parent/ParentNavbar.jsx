import React, {useState, useEffect} from 'react';
import {useProfile} from "../../contexts/ProfileContext.jsx";
import GetNewGradesNum from "./GetNewGradesNum.jsx";
import GetNewNotifsNum from "./GetNewNotifsNum.jsx";
import ParentNavbarContainer from "./ParentNavbarContainer.jsx";


function ParentNavbar({studentId, refreshNeeded}) {

   
    const {profile, logout} = useProfile();
    const [newNotifsLength, setNewNotifsLength] = useState(null)
    const [newGradesLength, setNewGradesLength] = useState(null)


    return (<>
            
            <GetNewNotifsNum studentId={studentId}
                                    onLength={(length) => setNewNotifsLength(length)} refreshNeeded={refreshNeeded}
                                    profile={profile}/>
            
            
            <GetNewGradesNum studentId={studentId}
                             onLength={(length) => setNewGradesLength(length)} refreshNeeded={refreshNeeded}
                             profile={profile}/>
            
            
            <ParentNavbarContainer studentId={studentId} newGradesLength={newGradesLength}
                                   newNotificationsLength={newNotifsLength} profile={profile}/>
        </>

    );
}

export default ParentNavbar;
