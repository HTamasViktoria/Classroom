import MessageFetcher from "./MessageFetcher.jsx";
import {useState} from "react";
import {Sidebar, MessageMainContainer, LeftList, LeftListItem} from "../../../StyledComponents.js";
import IncomingMessages from "./IncomingMessages.jsx";
import {useNavigate} from "react-router-dom";
import NewMessage from "./NewMessage.jsx";
import MessageSidebar from "./MessageSidebar.jsx";
import OneMessageViewer from "./OneMessageViewer.jsx";
function MessagesMain({id}) {

    const navigate = useNavigate()

    const [messages, setMessages] = useState([]);
    const [showing, setShowing] = useState("incoming")
const [chosenMessage, setChosenMessage] = useState({})

    const chosenOneHandler = (id) => {
        let message = messages.find(m => m.id === id);
setChosenMessage(message);
        setShowing("oneChosen");
    };

    
    return (
        <>
            <MessageFetcher id={id} onData={(data) => setMessages(data)}/>

            <h2>Üzenetek</h2>
            <MessageMainContainer>
                <MessageSidebar onChosing={(chosen) => setShowing(chosen)}/>

                {showing === "incoming" && (<IncomingMessages messages={messages} onChosing={chosenOneHandler}/>)}
                {showing === "new" && <NewMessage id={id} onSuccessfulSending={()=>setShowing("incoming")}/>}
                {showing === "oneChosen" && <OneMessageViewer message={chosenMessage} onGoBack={()=>setShowing("incoming")}/>}

            </MessageMainContainer>
        </>
    );
}

export default MessagesMain;
