import {useEffect} from "react";

function StudentsOfATeacherSubjectFetcher({selectedSubjectId, onData}){
    
    
    useEffect(()=>{
        if(selectedSubjectId !== ""){ fetch(`/api/teacherSubjects/getStudentsByTeacherSubjectId/${selectedSubjectId}`)
            .then(response => response.json())
            .then(data => {
                onData(data)
            })
            .catch(error => console.error('Error fetching data:', error));}
       
    },[selectedSubjectId])
    
    return null;
}

export default StudentsOfATeacherSubjectFetcher