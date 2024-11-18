
import {AButton
} from "../../../StyledComponents.js";
import React, {useState} from "react";

import EditGrades from "../../components/Teacher/EditGrades.jsx";
import GradeTable from "../../components/Parent/GradeTable.jsx";

function MainGradeTable({grades, studentId, isEditable, teacherId, teacherSubjects, nameOfClass, studentName, onGoBack, onRefresh }) {



    const [isEditing, setIsEditing] = useState(false);
    const [chosenSubject, setChosenSubject] = useState("")

const editHandler=(subject)=>{
        setChosenSubject(subject);
        setIsEditing(true);
}

    return (
        <>
            
            {isEditing ? (
                <EditGrades onGoBack={()=>setIsEditing(false)} 
                            teacherId={teacherId}
                            subject={chosenSubject}
                            studentId={studentId} 
                            studentName={studentName}
                            onRefresh={()=>  onRefresh()}/>
            ) : (
                <>
                    <GradeTable                    
                        studentId={studentId}
                        isEditable={isEditable}
                        teacherSubjects={teacherSubjects}
                        nameOfClass={nameOfClass}
                        onEditHandler={(subject)=>editHandler(subject)}
                        
                      
                       
                    />
                    <AButton onClick={()=>onGoBack()}>Vissza</AButton>
                </>
            )}
        </>
    );
}

export default MainGradeTable;
