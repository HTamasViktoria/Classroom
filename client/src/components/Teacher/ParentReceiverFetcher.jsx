import {useEffect} from "react";

function ParentReceiverFetcher({onData}){
    
    
    
    useEffect(()=>{
        fetch(`/api/users/parentreceivers`)
            .then(response=>response.json())
            .then(data=>onData(data))
            .catch(error=>console.error(error))
    },[])
    return null;
}

export default ParentReceiverFetcher