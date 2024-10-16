import React, { useState, useEffect } from "react";
import { CustomBox, StyledButton } from "../../../StyledComponents.js";
import GradeBySubject from "./GradeBySubject.jsx";
function ViewGradesBySubjects({onGoBack}) {
    const [subjects, setSubjects] = useState([]);
    const [selectedSubject, setSelectedSubject] = useState("")

    useEffect(() => {
        fetch(`/api/subjects`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                
                const sortedData = data.sort((a,b)=>a.localeCompare(b));
                setSubjects(sortedData);
            })
            .catch(error => console.error(`Error:`, error));
    }, []);

    
    const subjectChooseHandler=(e)=>{
        setSelectedSubject(e.target.id)
    }
    
  
    const goBackHandler=()=>{
        onGoBack()
    }
    
    const selectedNoneHandler=()=>{
        setSelectedSubject("")
    }
    
    return (<>

        {selectedSubject === "" ? (  <>   <CustomBox sx={{ padding: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
            {subjects.map((subject, index) => (
                <StyledButton key={index} id={subject} onClick={subjectChooseHandler}>{subject}</StyledButton>
            ))}
        </CustomBox><StyledButton onClick={goBackHandler}>Vissza</StyledButton> </>) : 
            (<GradeBySubject onGoBack={selectedNoneHandler} subject={selectedSubject}/>)}      
        </>
    );
}

export default ViewGradesBySubjects;
