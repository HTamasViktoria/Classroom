import {useEffect} from "react";


function AllGradesFetcher({studentId, onData}){


    useEffect(() => {
        fetch(`/api/grades/${studentId}`)
            .then(response => response.json())
            .then(data => {
               onData(data)
            })
            .catch(error => console.error('Error fetching data:', error));
    }, []);
    
    return null;
}

export default AllGradesFetcher