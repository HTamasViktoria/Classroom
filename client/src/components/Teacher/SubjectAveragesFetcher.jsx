import {useEffect, useState} from "react";

function SubjectAveragesFetcher({subject, onData}){


    const [averages, setAverages] = useState([])

    useEffect(()=>{
        fetch(`/api/grades/class-averages/bySubject/${subject}`)
            .then(response=>response.json())
            .then(data=> {console.log(data)
                onData(data)
            })
            .catch(error=>console.error(`Error:`,error))

    },[subject])

    return null;
}

export default SubjectAveragesFetcher