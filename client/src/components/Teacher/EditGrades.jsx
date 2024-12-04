import {useEffect, useState} from "react";
import EditingMain from "./EditingMain.jsx";
import GradeEditForm from "./GradeEditForm.jsx";

function EditGrades({teacherId, subject, studentId, studentName, onGoBack, onRefresh}) {

    const [isEditing, setIsEditing] = useState(false);
    const [editingGrade, setEditingGrade] = useState("");
    const [gradesOfThisSubject, setGradesOfThisSubject] = useState([]);
    const [refreshNeeded, setRefreshNeeded] = useState(false)

    useEffect(() => {
        fetch(`/api/grades/bysubject/${subject}/bystudent/${studentId}`)
            .then(response => response.json())
            .then(data => {
                setGradesOfThisSubject(data);
            })
            .catch(error => console.error(error));
    }, [subject, studentId, refreshNeeded]);

   const refreshHandler=()=>{setRefreshNeeded((prevState) => !prevState)}

    return (
        <>
            {isEditing ?
                (<GradeEditForm grade={editingGrade} onGoBack={()=>setIsEditing(false)}
                                onRefreshing={refreshHandler}/>) :
                (<EditingMain
                    teacherId={teacherId}
                    gradesOfThisSubject={gradesOfThisSubject}
                    studentId={studentId}
                    studentName={studentName}
                    subject={subject}
                    onEditing={()=>setIsEditing(true)}
                    onEditingGrade={(grade)=>setEditingGrade(grade)}
                    onGoingBack={()=>onGoBack()}
                    onRefreshing={refreshHandler}
                />)}
        </>
    );
}

export default EditGrades;
