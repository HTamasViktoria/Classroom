import { useState, useCallback } from "react";
import ReceiverSelector from "../components/ReceiverSelector.jsx";
import { FullWidthH2 } from "../../../StyledComponents.js";

function NewMessage({ id, onSuccessfulSending, onGoBack }) {
    const [state, setState] = useState({
        headText: "",
        message: "",
        receivers: [],
    });

    const handleChange = (key, value) => {
        setState((prevState) => ({ ...prevState, [key]: value }));
    };

    const handleReceiverChange = useCallback((receivers) => {
        handleChange("receivers", receivers);
    }, []);

    const submitHandler = async (e) => {
        e.preventDefault();

        const messageRequest = {
            FromId: id,
            ReceiverIds: state.receivers,
            HeadText: state.headText,
            Text: state.message,
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
                onSuccessfulSending();
                
            } else {
                const data = await response.json();
                alert(`Hiba: ${data.message}`);
            }
        } catch (error) {
            console.error("Hiba az üzenet küldése közben:", error);
            alert("Hiba történt az üzenet küldésekor.");
        }
    };

    return (
        <>
            <form>
                <FullWidthH2>Új üzenet írása</FullWidthH2>
                <ReceiverSelector id={id} onReceiverChange={handleReceiverChange} />
                <br />
                <label htmlFor="headText">Tárgy:</label>
                <br />
                <input
                    type="text"
                    id="headText"
                    name="headText"
                    value={state.headText}
                    onChange={(e) => handleChange("headText", e.target.value)}
                />
                <br />
                <label htmlFor="message">Üzenet szövege:</label>
                <br />
                <textarea
                    id="message"
                    name="message"
                    rows="15"
                    cols="60"
                    value={state.message}
                    onChange={(e) => handleChange("message", e.target.value)}
                    placeholder="Írja ide az email szövegét..."
                ></textarea>
                <br />
                <button type="button">Mentés piszkozatként</button>
                <button type="submit" onClick={submitHandler}>
                    Küldés
                </button>
                <button onClick={()=>onGoBack()} type="button">Vissza</button>
            </form>
        </>
    );
}

export default NewMessage;
