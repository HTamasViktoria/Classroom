import {useEffect} from "react";

function SubjectFetcher({onData}){


    useEffect(() => {
        fetch(`/api/subjects`)
            .then(response => response.json())
            .then(data => {
                onData(data)
            })
            .catch(error => console.error('Error fetching data:', error));
    }, []);


    return null;
}


export default SubjectFetcher;