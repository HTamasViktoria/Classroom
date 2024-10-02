import { Box } from "@mui/material";
import React, { useState } from "react";
import LastNotifications from "./LastNotifications.jsx";
import NotificationDetailed from "./NotificationDetailed.jsx";
import { StyledButton } from '../../../StyledComponents';
import { useNavigate } from "react-router-dom";

function ParentMain({ lastNotifications, studentId, onRefreshNeeded }) {
    const navigate = useNavigate();
    const [chosenNotification, setChosenNotification] = useState("");

    const clickHandler = (notification) => {
        setChosenNotification(notification);
    };

    const resetHandler = () => {
        setChosenNotification("");
    };

    const viewAllNotifsHandler = () => {
        navigate(`/parent/notifications/${studentId}`);
    };

    return (
        <>
            {chosenNotification === "" ? (
                <>
                    <LastNotifications lastNotifications={lastNotifications} onClick={clickHandler} />
                    <Box sx={{ marginTop: lastNotifications.length ? 2 : 12 }}>
                        {lastNotifications.length > 0 && (
                            <StyledButton onClick={viewAllNotifsHandler}>
                                Összes értesítés megtekintése
                            </StyledButton>
                        )}
                    </Box>
                </>
            ) : (
                <NotificationDetailed
                    notification={chosenNotification}
                    onButtonClick={resetHandler}
                    onRefreshNeeded={onRefreshNeeded}
                />
            )}
        </>
    );
}

export default ParentMain;
