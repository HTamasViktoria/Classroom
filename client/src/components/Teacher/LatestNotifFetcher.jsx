import React, {useEffect} from "react";

function LatestNotifFetcher({teacherId, onData}){
    

    useEffect(()=>{
        fetch(`/api/notifications/newestByTeacherId/${teacherId}`)
            .then(response => {
          
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
            
                onData(data);
            })
            .catch(error => {
                console.error("Fetch error:", error);
                onData(null);
            });

    },[teacherId])
    
    return null;
}

export default LatestNotifFetcher