import {CustomBox, StyledButton, StyledTableCell, StyledTableHead} from "../../../StyledComponents.js";
import {ListItem, Table, TableContainer, TableRow, TableBody, TableCell} from "@mui/material";
import {useEffect, useState} from "react";
import EditingMain from "./EditingMain.jsx";
import GradeEditForm from "./GradeEditForm.jsx";


function EditGrades({teacherId, subject, studentId, studentName, onGoBack, onRefresh}) {

    const [isEditing, setIsEditing] = useState(false)
    const [editingGrade, setEditingGrade] = useState("")
    const [gradesOfThisSubject, setGradesOfThisSubject] = useState([])

    
    
    useEffect(()=>{
        fetch(`/api/grades/getGradesBySubjectByStudent/${subject}/${studentId}`)
            .then(response=>response.json())
            .then(data=>{
                console.log(data)
                setGradesOfThisSubject(data)
            })
            .catch(error=>console.error(error))
    })
    
    const editHandler = (id) => {
        const gradeToEdit = gradesOfThisSubject.find((grade) => grade.id === id)
        setEditingGrade(gradeToEdit)
        setIsEditing(true)
    }


    const goBackHandler = () => {
        onGoBack()
    }
    
    const backFromEditFormHandler=()=>{
        setIsEditing(false)
    }
    
    const refreshHandler=()=>{
     
        onRefresh()
    }
    
    return(<>
        {isEditing ? 
            (<GradeEditForm grade={editingGrade} onGoBack={backFromEditFormHandler} onRefreshing={refreshHandler}/>) : 
            (<EditingMain teacherId={teacherId} 
                          grades={gradesOfThisSubject} 
                          studentId={studentId} 
                          studentName={studentName} 
                          subject={subject}
                          onEditChosen={editHandler} 
                          onGoingBack={goBackHandler}  
                          onRefreshing={refreshHandler}/>) }
    </>)
}

export default EditGrades