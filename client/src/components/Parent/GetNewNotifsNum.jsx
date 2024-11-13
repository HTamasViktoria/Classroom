import React, { useEffect } from 'react';
function GetNewNotifsNum({studentId, onLength, refreshNeeded}){
    
  
    
    useEffect(()=>{
        fetch(`/api/notifications/getNewNotifsNumber/${studentId}`)
            .then(response=>response.json())
            .then(data=>onLength(data))
            .catch(error=>console.error(error))
    },[studentId, refreshNeeded])
    
    return null;
}

export default GetNewNotifsNum