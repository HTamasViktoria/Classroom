import react, {useState, useEffect} from "react";
import {CustomBox, StyledButton} from "../../../StyledComponents.js";
import ViewGradesBySubjects from "./ViewGradesBySubjects.jsx";
import ViewGradesByClass from "./ViewGradesByClass.jsx";
import ViewGradesByStudent from "./ViewGradesByStudent.jsx";

function TeacherViewingGrades({teacherSubjects, onGoBack}){
    
    const[chosen, setChosen] = useState("")
    
    const chosenHandler =(e)=>{
        setChosen(e.target.id)
    }
    
const goBackHandler=()=>{
        onGoBack()
}

const chosenNoneHandler=()=>{
        setChosen("")
}
    
    return(<>

        {chosen === "bySubject" ? (<ViewGradesBySubjects onGoBack={chosenNoneHandler} teacherid={teacherSubjects[0].teacherId}/>) : 
            
            chosen === "byClass" ? (<ViewGradesByClass onGoBack={chosenNoneHandler} teacherid={teacherSubjects[0].teacherId}/>):
                
                chosen === "byStudent" ? (<ViewGradesByStudent onGoBack={chosenNoneHandler} teacherSubjects={teacherSubjects} teacherId = {teacherSubjects[0].teacherId}/>) :
                    
            (<CustomBox sx={{ padding: 2, display: 'flex', justifyContent: 'space-between', gap: 2 }}>
            <StyledButton id={"bySubject"} onClick={chosenHandler}>Tantárgy alapján</StyledButton>
            <StyledButton id={"byClass"} onClick={chosenHandler}>Osztály alapján</StyledButton>
            <StyledButton id={"byStudent"} onClick={chosenHandler}>Tanuló alapján</StyledButton>
              <StyledButton onClick={goBackHandler}>Vissza</StyledButton>
        </CustomBox>)}
        </>
        )
        
        
        
}
export default TeacherViewingGrades