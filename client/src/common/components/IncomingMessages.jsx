import { MessageDetail, MessageLine, IncomingMessagesContainer, UnreadMessageDetail } from "../../../StyledComponents.js";
import { useState } from "react";
import IncomingFetcher from "./IncomingFetcher.jsx";
import formatDate from "./formatDate.js";

function IncomingMessages({ onChosing, id }) {

    const [messages, setMessages] = useState([]);
    const [refreshNeeded, setRefreshNeeded] = useState(false);

    const deleteHandler = async (e) => {
        e.stopPropagation();
        const messageId = e.target.id;
        try {
            const response = await fetch(`/api/messages/receiverDelete/${messageId}`, {
                method: 'DELETE',
            });
            if (response.ok) {
                setRefreshNeeded((prevState) => !prevState);
                alert("Üzenet törölve");
            } else {
                throw new Error('Hiba történt az üzenet törlésekor.');
            }
        } catch (error) {
            console.error(error);
        }
    };

    const setToUnreadHandler = (e) => {
        e.stopPropagation();
        const messageId = e.target.id;
        fetch(`/api/messages/setToUnread/${messageId}`)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setRefreshNeeded((prevState) => !prevState);
            })
            .catch(error => console.error(error));
    };

    return (
        <>
            <IncomingFetcher onData={(data) => setMessages(data)} id={id} refreshNeeded={refreshNeeded} />
            <IncomingMessagesContainer>
                {messages.length === 0 && (<h1>Nincsenek üzenetek</h1>)}
                {messages.map((message) => (
                    <MessageLine key={message.id} onClick={() => onChosing(message)}>
                        {message.read ? (
                            <>
                                <MessageDetail>{formatDate(message.date)}</MessageDetail>
                                <MessageDetail>{message.senderName}</MessageDetail>
                                <MessageDetail>{message.headText}</MessageDetail>
                                <MessageDetail>
                                    {message.text.length > 20 ? message.text.substring(0, 20) + "..." : message.text}
                                </MessageDetail>
                            </>
                        ) : (
                            <>
                                <UnreadMessageDetail>{formatDate(message.date)}</UnreadMessageDetail>
                                <UnreadMessageDetail>{message.senderName}</UnreadMessageDetail>
                                <UnreadMessageDetail>{message.headText}</UnreadMessageDetail>
                                <UnreadMessageDetail>
                                    {message.text.length > 20 ? message.text.substring(0, 20) + "..." : message.text}
                                </UnreadMessageDetail>
                            </>
                        )}
                        <MessageDetail>
                            <button id={message.id} onClick={deleteHandler}>Törlés</button>
                            {message.read && (
                                <button id={message.id} onClick={setToUnreadHandler}>Olvasatlannak jelöl</button>
                            )}
                        </MessageDetail>
                    </MessageLine>
                ))}
            </IncomingMessagesContainer>
        </>
    );
}

export default IncomingMessages;
