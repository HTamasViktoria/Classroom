import {HeadingTH, MessageItem, MessagesTable, IncomingMessagesContainer, UnreadButton} from "../../../StyledComponents.js";

function IncomingMessages({messages, onChosing}){

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');

        return `${year}-${month}-${day} ${hours}:${minutes}`;
    };
    
    

    
    return(<>
        <IncomingMessagesContainer>
        <MessagesTable>
            <thead>
            <tr>
                <HeadingTH></HeadingTH>
                <HeadingTH>Dátum</HeadingTH>
                <HeadingTH>Feladó</HeadingTH>
                <HeadingTH></HeadingTH>
                <HeadingTH></HeadingTH>
            </tr>
            </thead>
            <tbody>
            {messages.map((message) => (
                <tr key={message.id} onClick={()=>onChosing(message.id)}>
                    {message.read == true && (<button>Olvasatlannak jelöl</button>)}
                    <MessageItem>{formatDate(message.date)}</MessageItem>
                    <MessageItem>{message.senderName}</MessageItem>
                    <MessageItem>{message.headText}</MessageItem>
                    <MessageItem>
                        {message.text.length > 20 ? message.text.substring(0, 20) + "..." : message.text}
                    </MessageItem>
<button>Törlés</button>
                </tr>
            ))}
            </tbody>
        </MessagesTable>
    </IncomingMessagesContainer></>)
}


export default IncomingMessages