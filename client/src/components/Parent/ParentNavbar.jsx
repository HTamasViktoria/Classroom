import React, { useState, useEffect } from 'react';
import { AppBar, Toolbar, Badge, useTheme } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useNavigate } from 'react-router-dom';
import { StyledTypography } from '../../../StyledComponents';

function ParentNavbar({studentId, refreshNeeded }) {
    const theme = useTheme();
    const navigate = useNavigate();
    const [newNotificationsLength, setNewNotificationsLength] = useState(0);
    const [notifications, setNotifications] = useState([])

    useEffect(() => {
        fetch(`/api/notifications/byStudentId/${studentId}`)
            .then(response => response.json())
            .then(data => {
                setNotifications(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [studentId, refreshNeeded]);

    useEffect(() => {
        if (notifications.length > 0) {
            const newNotifications = notifications.filter(n => !n.read);
            setNewNotificationsLength(newNotifications.length);
        }
    }, [notifications]);

    const clickHandler = (value) => {
        navigate(`/parent/${value}/${studentId}`);
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
        <AppBar sx={{ backgroundColor: theme.palette.navbar.main }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', gap: 4, width: '100%' }}>
                {menuItems.map(item => (
                    <StyledTypography
                        key={item.value}
                        onClick={() => clickHandler(item.value)}
                        sx={{ cursor: 'pointer', marginRight: 2 }}
                    >
                        {item.label}
                        {item.badge > 0 && (
                            <Badge
                                badgeContent={item.badge}
                                color="error"
                            />
                        )}
                    </StyledTypography>
                ))}
                <AccountCircleIcon
                    sx={{ cursor: 'pointer', fontSize: 30, color: theme.palette.text.primary }}
                />
            </Toolbar>
        </AppBar>
    );
}

export default ParentNavbar;
