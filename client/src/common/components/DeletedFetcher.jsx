import {useEffect} from "react";

function DeletedFetcher({id, onData, refreshNeeded}){
    
    
    useEffect(()=>{
        fetch(`/api/messages/getDeleteds/${id}`)
            .then(response=>response.json())
            .then(data=>onData(data))
            .catch(error=>console.error(error))
    },[id, refreshNeeded])
    
    
    return null;
}

export default DeletedFetcher