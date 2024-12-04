import {useState, useEffect} from "react";
import {AButton} from "../../../StyledComponents.js";
import {ListItem} from "@mui/material";
import GradesByStudentMain from "./GradesByStudentMain.jsx";



function ViewGradesByStudent({teacherId, onGoBack}){
    
    
    const [students, setStudents] = useState([])
    const [chosenStudentId, setChosenStudentId] = useState("")
    const [chosenStudentsClassName, setChosenStudentsClassName] = useState("")
    const [chosenStudentName, setChosenStudentName] = useState("")
    
    useEffect(()=>{
        fetch(`/api/classes/students`)
            .then(response=>response.json())
            .then(data=>{
                const sortedData = data.sort((a,b)=>a.familyName.localeCompare(b.familyName));
                setStudents(sortedData)
            })
            .catch(error=>console.error(`Error:`, error))
    },[teacherId])
    
    
    const studentClickHandler=(e)=>{
        let chosenId = e.target.id
        let chosenStudent = students.find((student)=>student.id === chosenId)
        setChosenStudentsClassName(chosenStudent.nameOfClass)
        setChosenStudentName(`${chosenStudent.familyName} ${chosenStudent.firstName}`)
        setChosenStudentId(chosenId)
        
    }
     
    

    return (<>
        {chosenStudentId === "" ? (<>
            <ul>
                {students.map((student, index) => (
                    <ListItem onClick={studentClickHandler} 
                              id={student.id}
                              key={index}>{student.familyName} {student.firstName} - {student.nameOfClass}
                    </ListItem>
                ))}
            </ul>
            <AButton onClick={()=> onGoBack()}>Vissza</AButton>
        </>) : (<GradesByStudentMain onGoBack={()=> setChosenStudentId("")} 
                                     teacherId={teacherId}
                                     studentName={chosenStudentName} 
                                     studentId={chosenStudentId}
                                     nameOfClass={chosenStudentsClassName}/>)}

    </>)
}

export default ViewGradesByStudent