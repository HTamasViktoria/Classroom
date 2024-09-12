import {Box, Button, Grid, Typography} from "@mui/material";
import AddAlertIcon from "@mui/icons-material/AddAlert.js";
import AppRegistrationIcon from "@mui/icons-material/AppRegistration.js";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline.js";
import React from "react";

function TaskSelector(props){

    const buttonStyle = {
        color: 'black',
        textTransform: 'none'
    };
    
    
    return (<>
        <Box sx={{ padding: 2 }}>
            <Grid container spacing={2} alignItems="center">
                <Grid item>
                    <Button
                        onClick={()=>props.onChosenTask("addNotification")}
                        startIcon={<AddAlertIcon />}
                        sx={buttonStyle}
                    >
                        <Typography>Értesítést küldök</Typography>
                    </Button>
                </Grid>
            </Grid>
            <Grid container spacing={2} alignItems="center" sx={{ marginTop: 2 }}>
                <Grid item>
                    <Button
                        onClick={()=>props.onChosenTask("addGrade")}
                        startIcon={<AppRegistrationIcon />}
                        sx={buttonStyle}
                    >
                        <Typography>Osztályzatot adok</Typography>
                    </Button>
                </Grid>
            </Grid>
            <Grid container spacing={2} alignItems="center" sx={{ marginTop: 2 }}>
                <Grid item>
                    <Button
                        onClick={()=>props.onChosenTask("addMessage")}
                        startIcon={<ChatBubbleOutlineIcon />}
                        sx={buttonStyle}
                    >
                        <Typography>Üzenet küldök</Typography>
                    </Button>
                </Grid>
            </Grid>
        </Box></>)
}

export default TaskSelector