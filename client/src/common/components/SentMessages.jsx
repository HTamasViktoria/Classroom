import SentFetcher from "./SentFetcher.jsx";
import {useState} from "react";
import {IncomingMessagesContainer, MessageDetail, MessageLine} from "../../../StyledComponents.js";
import formatDate from "./formatDate.js";
function SentMessages({id}){
    
    const [sentMessages, setSentMessages] = useState([])
    
    
    return(<>
    <SentFetcher id={id} onData={(data)=>setSentMessages(data)}/>
        <IncomingMessagesContainer>
            {sentMessages.length === 0  &&(<h1>Nincsenek elküldött üzenetek</h1>)}
            {sentMessages.map((message) => (
                <MessageLine key={message.id} onClick={()=>onChosing(message.id) }>
                    <MessageDetail>{formatDate(message.date)}</MessageDetail>
                    <MessageDetail>{message.senderName}</MessageDetail>
                    <MessageDetail>{message.headText}</MessageDetail>
                    <MessageDetail>
                        {message.text.length > 20 ? message.text.substring(0, 20) + "..." : message.text}
                    </MessageDetail>
                  

                </MessageLine>
            ))}

        </IncomingMessagesContainer>
    </>)
}

export default SentMessages