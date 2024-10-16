import {CustomBox, StyledButton, StyledTableCell, StyledTableHead} from "../../../StyledComponents.js";
import {ListItem, Table, TableContainer, TableRow, TableBody, TableCell} from "@mui/material";
import {useEffect, useState} from "react";
import EditingMain from "./EditingMain.jsx";
import GradeEditForm from "./GradeEditForm.jsx";


function EditGrades({teacherId, grades, studentId, studentName, onGoBack, onRefresh}){

    const [isEditing, setIsEditing] = useState(false)
    const [editingGrade, setEditingGrade] = useState("")


    const editHandler=(id)=>{
        const gradeToEdit = grades.find((grade)=>grade.id == id)
        setEditingGrade(gradeToEdit)
        setIsEditing(true)
    }


    const goBackHandler=()=>{
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
            (<GradeEditForm grade={editingGrade} onGoBack={backFromEditFormHandler}/>) : 
            (<EditingMain teacherId={teacherId} 
                          grades={grades} 
                          studentId={studentId} 
                          studentName={studentName} 
                          onEditChosen={editHandler} 
                          onGoingBack={goBackHandler}  
                          onRefreshing={refreshHandler}/>) }
    </>)
}

export default EditGrades