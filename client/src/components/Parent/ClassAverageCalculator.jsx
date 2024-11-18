import React, {useState, useEffect} from 'react';
import ClassAverageFetcher from "../../common/components/ClassAverageFetcher.jsx";


function ClassAverageCalculator({subject, studentId}) {


    const [averageToShow, setAverageToShow] = useState("")
    const [averages, setAverages] = useState([])

  
    
    useEffect(() => {
        if (averages[`${subject}`]) {
            setAverageToShow(averages[`${subject}`])
        } else {
            setAverageToShow("Nincs elég adat")
        }
    }, [averages]);


    return (<>
        <ClassAverageFetcher studentId={studentId} onData={(data) => setAverages(data)}/>
        <span>Osztályátlag:{averageToShow} </span>

    </>)
}

export default ClassAverageCalculator