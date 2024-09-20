import { AppBar, Toolbar, Typography, Box, Badge } from '@mui/material';
import NotificationsIcon from '@mui/icons-material/Notifications';
import React, { useState, useEffect } from 'react';

function ParentNavbar(props) {

    const [newNotificationsLength, setNewNotificationsLength] = useState("");
    
    useEffect(() => {
        const newNotifications = props.notifications.filter(n=>n.read == false);
        setNewNotificationsLength(newNotifications.length)
    }, [props]);


    const clickHandler = (event) => {
        const chosen = event.target.getAttribute('data-value');
        console.log(chosen);
        props.onChosen(chosen);
    };

    return (
        <AppBar sx={{ backgroundColor: '#d2a679' }}>
            <Toolbar>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                    <Typography onClick={clickHandler} component='div' data-value="messages">
                        Üzenetek
                    </Typography>
                    <Typography onClick={clickHandler} component='div' data-value="grades">
                        Osztályzatok
                    </Typography>
                    <Typography onClick={clickHandler} component='div' data-value="notifications">
                        Értesítések
                        <span style={{ marginLeft: '8px', backgroundColor: '#f44336', color: 'white', borderRadius: '12px', padding: '4px 8px' }}>
        {newNotificationsLength}
    </span>
                    </Typography>

                    <Typography onClick={clickHandler} component='div' data-value="absences">
                        Hiányzások
                    </Typography>
                    <Typography onClick={clickHandler} component='div' data-value="schedule">
                        Órarend
                    </Typography>
                    <Typography onClick={clickHandler} component='div' data-value="bell-schedule">
                        Csengetési rend
                    </Typography>
                </Box>
            </Toolbar>
        </AppBar>
    );
}

export default ParentNavbar;
