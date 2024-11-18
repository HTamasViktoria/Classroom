import React, { useEffect } from 'react';

function GetNewGradesNum({studentId, onLength, refreshNeeded}){
    
    
    useEffect(()=>{
        fetch(`/api/grades/getNewGradesNumber/${studentId}`)
            .then(response=>response.json())
            .then(data=>onLength(data))
            .catch(error=>console.error(error))
    },[studentId, refreshNeeded])

    return null;
}

export default GetNewGradesNum