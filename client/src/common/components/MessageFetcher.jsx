import {useEffect} from "react";

function MessageFetcher({id, onData}){
    
    useEffect(()=>{
        fetch(`/api/messages/getIncomings/${id}`)
            .then(response=>response.json())
            .then(data=>onData(data))
            .catch(error=>console.error(error))
    },[id])
    
    return null;
}


export default MessageFetcher