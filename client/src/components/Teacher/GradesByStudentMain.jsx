import {useState} from "react";
import MainGradeTable from "../../common/components/MainGradeTable.jsx";
import TeacherSubjectsFetcher from "./TeacherSubjectsFetcher.jsx";
import AllGradesFetcher from "../Parent/AllGradesFetcher.jsx";
import SubjectFetcher from "../Parent/SubjectFetcher.jsx";
function GradesByStudentMain({studentId, teacherId, nameOfClass, studentName, onGoBack}){
    
    const [grades, setGrades] = useState([])
    const [subjects, setSubjects] = useState([])
    const [refreshNeeded, setRefreshNeeded] = useState(false)
    const [teacherSubjects, setTeacherSubjects] = useState([])


    const refreshHandler = () => {
        setGrades([])
        setRefreshNeeded((prevState) => !prevState)
    }
       
    
    return(<>
        <TeacherSubjectsFetcher id={teacherId} onData={(data)=>setTeacherSubjects(data)}/>
        <AllGradesFetcher studentId={studentId} onData={(data)=>setGrades(data)}/>
        <SubjectFetcher onData={(data)=>setSubjects(data)}/>
        
        <MainGradeTable onGoBack={()=>onGoBack()} 
                      studentName={studentName} 
                      teacherId={teacherId} 
                      studentId={studentId} 
                      isEditable={true} 
                      subjects={subjects} 
                      grades={grades} 
                      nameOfClass={nameOfClass} 
                      teacherSubjects={teacherSubjects}
                      onRefresh={refreshHandler}/>
    </>)
}

export default GradesByStudentMain