import {useEffect} from "react";

function GradesOfClassBySubjectFetcher({classId, subject, onData}){


    useEffect(() => {
        fetch(`/api/grades/${classId}/${subject}`)
            .then(response => response.json())
            .then(data => {
                onData(data)
            })
            .catch(error => console.error(`Error:`, error));
    }, [classId, subject]);
    
    return null;
}

export  default GradesOfClassBySubjectFetcher