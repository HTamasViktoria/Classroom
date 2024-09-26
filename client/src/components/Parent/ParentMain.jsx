import {Box, Card, CardContent, Typography, Grid, Button} from "@mui/material";
import React, {useState} from "react";
import LastNotifications from "./LastNotifications.jsx";
import NotificationDetailed from "./NotificationDetailed.jsx";
import { useNavigate } from "react-router-dom";

function ParentMain({lastNotifications, studentId, onRefreshNeeded}) {
    const navigate = useNavigate();
    
    const [chosen, setChosen] = useState("")
    const [chosenNotification, setChosenNotification] = useState("")

    const clickHandler = (notification) => {
        setChosen("notification");
        setChosenNotification(notification);

    };

    const buttonClickHandler = () => {
        setChosen("")
        setChosenNotification("")
    }

    const viewAllNotifsHandler=()=>{
        navigate(`/parent/notifications/${studentId}`)
    }

    const refreshHandler=()=>{
        onRefreshNeeded();
    }
    
    return (
        <>
            {chosen === "" && (
                <>
                    <LastNotifications lastNotifications={lastNotifications} onClick={clickHandler} />
                    <Box sx={{ marginTop: lastNotifications.length ? 2 : 12 }}>
                        {lastNotifications.length > 0 && <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#d9c2bd',
                                color: '#fff',
                                '&:hover': {
                                    backgroundColor: '#c2a6a0',
                                },
                            }}
                            onClick={viewAllNotifsHandler}
                        >
                            Összes értesítés megtekintése
                        </Button> }  
                    </Box>
                </>
            )}
            {chosenNotification !== "" && (
                <NotificationDetailed notification={chosenNotification} onButtonClick={buttonClickHandler} onRefreshNeeded={refreshHandler} />
            )}
        </>
    );
}

export default ParentMain;
