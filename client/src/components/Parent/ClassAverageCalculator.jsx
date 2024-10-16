import React, { useState, useEffect } from 'react';



function ClassAverageCalculator({averages, subject}){
    
   
    const [averageToShow, setAverageToShow] = useState("")  
    
    
    useEffect(() => {
        if(averages[`${subject}`]){setAverageToShow(averages[`${subject}`])}
        else{
            setAverageToShow("Nincs elég adat")
        }
    }, [averages]);
    
    
    return(<>
    <span>Osztályátlag:{averageToShow} </span>
    
    
    </>)
}

export default ClassAverageCalculator