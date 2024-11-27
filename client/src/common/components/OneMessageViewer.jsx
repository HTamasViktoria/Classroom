import { CustomBox,MessageViewerBox, InnerBox,ButtonContainer, MessageDetailTitle, InnerTitle, TextHolder} from "../../../StyledComponents.js";
import { Typography, Box } from "@mui/material";
import React, {useEffect} from "react";
import formatDate from "./formatDate.js";
import {useNavigate} from "react-router-dom";


function OneMessageViewer({ message, onGoBack, onResponse}) {

    const navigate = useNavigate()

    useEffect(()=>{
        fetch(`/api/messages/setToRead/${message.id}`)
            .then(response=>response.json())
            .then(data=>console.log(data))
            .catch(error=>console.error(error))
    },[])
    
    const deleteHandler = async (e) => {
        const messageId = e.target.id;

        try {
            const response = await fetch(`/api/messages/receiverDelete/${messageId}`, {
                method: 'DELETE',
            });

            if (response.ok) {
            alert("Üzenet törölve");
            onGoBack()
                
            } else {
                throw new Error('Hiba történt az üzenet törlésekor.');
            }
        } catch (error) {console.error(error)
        }
    };
    
    const responseHandler=(e)=>{
        onResponse(e.target.id)
    }
   
    
    return (
        <CustomBox>
            <MessageViewerBox>
                <InnerBox>
                    <MessageDetailTitle>Dátum:</MessageDetailTitle>
                    <InnerTitle>{formatDate(message.date)}</InnerTitle>
                </InnerBox>

                <InnerBox>
                    <MessageDetailTitle>Feladó:</MessageDetailTitle>
                    <InnerTitle>{message.senderName}</InnerTitle>
                </InnerBox>

                <InnerBox>
                    <MessageDetailTitle>Tárgy:</MessageDetailTitle>
                    <InnerTitle>{message.headText}</InnerTitle>
                </InnerBox>

                <Box sx={{ marginBottom: 2 }}>
                    <InnerTitle>Üzenet szövege:</InnerTitle>
                    <TextHolder>{message.text}</TextHolder>
                </Box>
                <ButtonContainer>
                    <button id={message.id} onClick={responseHandler}>
                        Válasz
                    </button>
                    <button id={message.id} onClick={deleteHandler}>
                        Törlés
                    </button>
                    <button onClick={()=>onGoBack()}>Vissza</button>
                </ButtonContainer>
            </MessageViewerBox>
        </CustomBox>
    );
}

export default OneMessageViewer;
