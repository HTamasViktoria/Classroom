import {useEffect} from "react";

function SentFetcher({id, onData}){
    
    
    useEffect(()=>{
        fetch(`/api/messages/getSents/${id}`)
            .then(response=>response.json())
            .then(data=>onData(data))
            .catch(error=>console.error(error))
    },[id])
    
    return null;
}

export default SentFetcher