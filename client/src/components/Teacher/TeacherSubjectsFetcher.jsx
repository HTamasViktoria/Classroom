import {useEffect} from "react";


function TeacherSubjectsFetcher({teacherId, onData}){
    
    useEffect(()=>{
       fetch(`/api/teacherSubjects/getByTeacherId/${teacherId}`) 
           .then(response=>response.json())
           .then(data=> {console.log(data)
               onData(data)
           })
           .catch(error=>console.error(error))
    },[teacherId])
    
    return null;
}

export default TeacherSubjectsFetcher