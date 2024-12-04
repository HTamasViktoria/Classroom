import {useEffect} from "react";

function IncomingFetcher({id, onData, refreshNeeded}){
    
    useEffect(()=>{
        fetch(`/api/messages/incomings/${id}`)
            .then(response=>response.json())
            .then(data=>onData(data))
            .catch(error=>console.error(error))
    },[id, refreshNeeded])
    
    return null;
}


export default IncomingFetcher