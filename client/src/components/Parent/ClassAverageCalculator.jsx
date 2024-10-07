import React, { useState, useEffect } from 'react';



function ClassAverageCalculator({id, subject}){
    
    const [classAverages, setClassAverages] = useState([])
    const [averageToShow, setAverageToShow] = useState("")
    
    useEffect(()=>{
        fetch(`/api/grades/class-averages/${id}`)
            .then(response=>response.json())
            .then(data=> {
                console.log(data)
                setClassAverages(data)
            })
            .catch(error=>console.error(`Error fetching data:`, error))
    },[id])


    useEffect(() => {
        if(classAverages[`${subject}`]){setAverageToShow(classAverages[`${subject}`])}
        else{
            setAverageToShow("Nincs elég adat")
        }
    }, []);
    
    
    return(<>
    <span>Osztályátlag:{averageToShow} </span>
    
    
    </>)
}

export default ClassAverageCalculator