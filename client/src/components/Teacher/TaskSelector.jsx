import { Box, Button, Typography } from "@mui/material";
import AddAlertIcon from "@mui/icons-material/AddAlert.js";
import AppRegistrationIcon from "@mui/icons-material/AppRegistration.js";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline.js";
import React from "react";

function TaskSelector(props) {
    const buttonStyle = (bgColor) => ({
        color: 'black',
        textTransform: 'none',
        backgroundColor: bgColor,
        padding: '10px 20px',
        borderRadius: '8px',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        '&:hover': {
            backgroundColor: bgColor,
            opacity: 0.8
        }
    });

    return (
        <Box sx={{ padding: 2, display: 'flex', justifyContent: 'space-between', gap: 2 }}>
            <Button
                onClick={() => props.onChosenTask("addNotification")}
                startIcon={<AddAlertIcon />}
                sx={buttonStyle('#d9c2bd')}
            >
                <Typography>Értesítést küldök</Typography>
            </Button>
            <Button
                onClick={() => props.onChosenTask("addGrade")}
                startIcon={<AppRegistrationIcon />}
                sx={buttonStyle('#a2c4c6')}
            >
                <Typography>Osztályzatot adok</Typography>
            </Button>
            <Button
                onClick={() => props.onChosenTask("addMessage")}
                startIcon={<ChatBubbleOutlineIcon />}
                sx={buttonStyle('#82b2bd')}
            >
                <Typography>Üzenetet küldök</Typography>
            </Button>
        </Box>
    );
}

export default TaskSelector;
