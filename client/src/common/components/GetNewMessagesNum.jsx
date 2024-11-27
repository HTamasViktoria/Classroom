import {useEffect} from "react";

function GetNewMessagesNum({id, onData}){
    
    useEffect(()=>{
        fetch(`/api/messages/getNewMessagesNum/${id}`)
            .then(response=>response.json())
            .then(data=>{console.log(data);
            onData(data)})
            .catch(error=>console.error(error))
    },[id])
    
    return null;}


export default GetNewMessagesNum