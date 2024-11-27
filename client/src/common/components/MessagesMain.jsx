import {useEffect, useState} from "react";
import {MessageMainContainer, ChosenHeading} from "../../../StyledComponents.js";
import IncomingMessages from "./IncomingMessages.jsx";
import {useNavigate} from "react-router-dom";
import NewMessage from "./NewMessage.jsx";
import MessageSidebar from "./MessageSidebar.jsx";
import OneMessageViewer from "./OneMessageViewer.jsx";
import DeletedMessages from "./DeletedMessages.jsx";
import SentMessages from "./SentMessages.jsx";
import ResponseMessage from "./ResponseMessage.jsx";

function MessagesMain({id}) {

    const navigate = useNavigate()

    const [showing, setShowing] = useState("incoming")
    const [chosenMessage, setChosenMessage] = useState({})
    const [chosenHeading, setChosenHeading] = useState("Beérkezett üzenetek")
    const [responsingId, setResponsingId] = useState("");


    useEffect(() => {
        if (showing === "new") {
            setChosenHeading("Új üzenet írása")
        } else if (showing === "incoming") {
            setChosenHeading("Beérkezett üzenetek")
        } else if (showing === "trash") {
            setChosenHeading("Kuka")
        } else if (showing === "sent") {
            setChosenHeading("Elküldött üzenetek")

        } else if (showing === "oneChosen") {
            setChosenHeading("Üzenet megtekintése")
        } else if (showing === "response") {
            setChosenHeading("Válasz küldése")
        }
    }, [showing])


    const chosingHandler = (m) => {
        setChosenMessage(m)
        setShowing("oneChosen")
    }

    const responsingHandler = (id) => {
        setResponsingId(id);
        setShowing("response")
    }

    return (
        <>
            <ChosenHeading>{chosenHeading}</ChosenHeading>
            <MessageMainContainer>
                <MessageSidebar onChosing={(chosen) => setShowing(chosen)}/>


                {showing === "incoming" && (<IncomingMessages id={id} onChosing={chosingHandler}/>)}
                {showing === "new" && <NewMessage id={id} onSuccessfulSending={() => setShowing("incoming")}
                onGoBack={()=>setShowing("incoming")}/>}
                {showing === "oneChosen" &&
                    <OneMessageViewer message={chosenMessage} onGoBack={() => setShowing("incoming")}
                                      onResponse={responsingHandler}/>}
                {showing === "trash" && <DeletedMessages id={id}/>}
                {showing === "sent" && <SentMessages id={id}/>}
                {showing === "response" && <ResponseMessage id={responsingId} onGoBack={() => setShowing("incoming")}/>}
            </MessageMainContainer>
        </>
    );
}

export default MessagesMain;
