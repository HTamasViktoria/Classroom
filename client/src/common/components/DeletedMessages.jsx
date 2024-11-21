import {IncomingMessagesContainer, MessageDetail, MessageLine} from "../../../StyledComponents.js";
import {useState} from "react";
import DeletedFetcher from "./DeletedFetcher.jsx";
import formatDate from "./formatDate.js";
function DeletedMessages({id}){

    const [deletedMessages, setDeletedMessages] = useState([])

    const [refreshNeeded, setRefreshNeeded] = useState(false)
    
    

    const restoreHandler=(e)=>{
        console.log(e.target.id)
   fetch(`/api/messages/restore/${e.target.id}/${id}`)
       .then(response=>response.json())
       .then(data=> {
           setRefreshNeeded((prevState)=>!prevState)
           alert("Üzenet visszaállítva")
       })
       .catch(error=>console.error(error))
    }
    
    return(<>
        <DeletedFetcher id={id} onData={(data)=>setDeletedMessages(data)} refreshNeeded={refreshNeeded}/>
        <IncomingMessagesContainer>
            {deletedMessages.length === 0  &&(<h1>Nincsenek törölt üzenetek</h1>)}
            {deletedMessages.map((message) => (
                <MessageLine key={message.id}>
                    {message.read === true && (<button>Olvasatlannak jelöl</button>)}
                    <MessageDetail>{formatDate(message.date)}</MessageDetail>
                    <MessageDetail>{message.senderName}</MessageDetail>
                    <MessageDetail>{message.headText}</MessageDetail>
                    <MessageDetail>
                        {message.text.length > 20 ? message.text.substring(0, 20) + "..." : message.text}
                    </MessageDetail>
                    <MessageDetail>
                        <button>Törlés</button>
                        <button id={message.id} onClick={restoreHandler}>Visszaállítás</button>
                    </MessageDetail>

                </MessageLine>
            ))}

        </IncomingMessagesContainer>
    </>)
}

export default DeletedMessages