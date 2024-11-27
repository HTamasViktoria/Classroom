import React, { useState} from "react";
import {AButton, SubjectChooseContainer } from "../../../StyledComponents.js";
import ClassOfStudentsSelector from "./ClassOfStudentsSelector.jsx";
import SubjectFetcher from "../Parent/SubjectFetcher.jsx";
function ViewGradesBySubjects({onGoBack}) {
    
    const [subjects, setSubjects] = useState([]);
    const [selectedSubject, setSelectedSubject] = useState("")

  
    
    return (<>
            <SubjectFetcher onData={(data)=>setSubjects(data)}/>

        {selectedSubject === "" ? 
            (<>
                <SubjectChooseContainer>
            {subjects.map((subject, index) => (
                <AButton key={index} id={subject} 
                         onClick={(e)=>setSelectedSubject(e.target.id)}>{subject}</AButton>
            ))}               
            </SubjectChooseContainer>
                
                <AButton onClick={()=>    onGoBack()}>Vissza</AButton> </>) : 
            
            
            (<ClassOfStudentsSelector onGoBack={()=> setSelectedSubject("")} subject={selectedSubject}/>)}      
        </>
    );
}

export default ViewGradesBySubjects;
