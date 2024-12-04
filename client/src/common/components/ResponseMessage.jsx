import { useNavigate } from "react-router-dom";
import { useEffect, useState, useContext } from "react";
import {useProfile} from "../../contexts/ProfileContext.jsx";

function ResponseMessage({ id, onGoBack }) {
    const { profile } = useProfile();
    const [previousMessage, setPreviousMessage] = useState({});
    const [responseText, setResponseText] = useState("");
    const [headText, setHeadText] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`/api/messages/${id}`)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setPreviousMessage(data);
                setResponseText(`${data.senderName} írta: ${data.text}`);
                setHeadText(`Válasz erre: ${data.headText}`);
            })
            .catch(error => console.error(error));
    }, [id]);

    const submitHandler = async (e) => {
        e.preventDefault();

        const messageRequest = {
            FromId: profile.id,
            ReceiverIds: [previousMessage.sender.id],
            HeadText: headText,
            Text: responseText,
            Date: new Date().toISOString(),
        };

        try {
            const response = await fetch(`/api/messages`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(messageRequest),
            });

            if (response.ok) {
                alert("Az üzenet sikeresen elküldve!");
           onGoBack();
            } else {
                const data = await response.json();
                alert(`Hiba: ${data.message}`);
            }
        } catch (error) {
            console.error("Hiba az üzenet küldése közben:", error);
            alert("Hiba történt az üzenet küldésekor.");
        }
    };

    const handleResponseChange = (e) => {
        setResponseText(e.target.value);
    };

    const handleGoBack = () => {
        navigate(`/`);
    };

    return (
        previousMessage && (
            <div>
                <h2>Válasz küldése</h2>

                <div>
                    <label><strong>Címzett:</strong></label>
                    <p>{previousMessage.senderName}</p>
                </div>

                <div>
                    <label><strong>Tárgy:</strong></label>
                    <p>{headText}</p>
                </div>

                <div>
                    <label><strong>Válasz:</strong></label>
                    <textarea
                        rows="10"
                        cols="50"
                        value={responseText}
                        onChange={handleResponseChange}
                        placeholder="Írd meg válaszod..."
                    />
                </div>

                <div>
                    <button onClick={submitHandler}>Küldés</button>
                    <button onClick={handleGoBack}>Vissza</button>
                </div>
            </div>
        )
    );
}

export default ResponseMessage;
