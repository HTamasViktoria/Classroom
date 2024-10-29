import react, {useState, useEffect} from "react";
import {CustomBox, StyledButton} from "../../../StyledComponents.js";
import ParentGradeTable from "../Parent/ParentGradeTable.jsx";

function GradesByStudentMain({studentId, teacherId, teacherSubjects, nameOfClass, studentName, onGoBack}){
    
    const [grades, setGrades] = useState([])
    const [subjects, setSubjects] = useState([])
    const [refreshNeeded, setRefreshNeeded] = useState(false)
    
    
    useEffect(()=>{
        fetch(`/api/subjects`)
            .then(response=>response.json())
            .then(data=>setSubjects(data))
            .catch(error=>console.error(`Error:`,error))
    },[studentId])


    useEffect(() => {
       
        fetch(`/api/grades/${studentId}`)
            .then(response => response.json())
            .then(data => {
              console.log(grades)
                setGrades(data);
            })
            .catch(error => console.error(`Error:`, error));
    }, [refreshNeeded,studentId]);
    
    
    const goBackHandler=()=>{
        onGoBack()
    }
    
    const refreshHandler=()=>{
setGrades([])
        setRefreshNeeded((prevState)=> !prevState)
    }
       
    
    return(<>
    <ParentGradeTable onGoBack={goBackHandler} 
                      studentName={studentName} 
                      teacherId={teacherId} 
                      id={studentId} 
                      isEditable={true} 
                      subjects={subjects} 
                      grades={grades} 
                      nameOfClass={nameOfClass} 
                      teacherSubjects={teacherSubjects}
                      onRefresh={refreshHandler}/>
    </>)
}

export default GradesByStudentMain