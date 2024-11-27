import React, { useState, useEffect } from 'react';


function NewGradeFetcher({onData,studentId, refreshNeeded}){





    useEffect(()=>{
        fetch(`/api/grades/newGrades/${studentId}`)
            .then(response=>response.json())
            .then(data=> onData(data))
            .catch(error=>console.error(error))
    },[studentId, refreshNeeded])



    return null;
}

export default NewGradeFetcher