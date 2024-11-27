import {useEffect} from "react";

function StudentFetcherByClassId({classId, onData}){


    useEffect(() => {
        fetch(`/api/classes/getStudents/${classId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then(data => {
               onData(data)
            })
            .catch(error => console.error(`Error:`, error));
    }, [classId]);
    
    return null;
}

export default StudentFetcherByClassId