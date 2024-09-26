import { AppBar, Toolbar, Typography, Box, Badge } from '@mui/material';
import React, { useState, useEffect } from 'react';
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import { useNavigate } from "react-router-dom";

function ParentNavbar({ notifications, studentId }) {
    const navigate = useNavigate();
    const [newNotificationsLength, setNewNotificationsLength] = useState(0);

    useEffect(() => {
        const newNotifications = notifications.filter(n => !n.read);
        setNewNotificationsLength(newNotifications.length);
    }, [notifications]);

    const clickHandler = (event) => {
        const chosen = event.target.getAttribute('data-value');
        navigate(`/parent/${chosen}/${studentId}`);
    };

    const menuItems = [
        { label: 'Üzenetek', value: 'messages' },
        { label: 'Osztályzatok', value: 'grades' },
        { label: 'Értesítések', value: 'notifications', badge: newNotificationsLength },
        { label: 'Hiányzások', value: 'absences' },
        { label: 'Órarend', value: 'schedule' },
        { label: 'Csengetési rend', value: 'bell-schedule' },
    ];

    return (
        <AppBar sx={{ backgroundColor: '#c6ac85' }}>
            <Toolbar>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                    {menuItems.map(item => (
                        <Typography
                            key={item.value}
                            onClick={clickHandler}
                            component='div'
                            data-value={item.value}
                            sx={{ cursor: 'pointer', position: 'relative', marginRight: '16px' }}
                        >
                            {item.label}
                            {item.badge > 0 && (
                                <Badge
                                    badgeContent={item.badge}
                                    color="error"
                                    sx={{ marginLeft: '8px' }}
                                />
                            )}
                        </Typography>
                    ))}
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <AccountCircleIcon
                            sx={{ cursor: 'pointer', fontSize: 30, marginLeft: '16px' }}
                        />
                    </Box>
                </Box>
            </Toolbar>
        </AppBar>
    );
}

export default ParentNavbar;
