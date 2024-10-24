import React, { useState, useEffect } from 'react';
import { AppBar, Toolbar, Badge, useTheme } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useNavigate } from 'react-router-dom';
import { StyledTypography } from '../../../StyledComponents';
import { useProfile } from "../../contexts/ProfileContext.jsx";

function ParentNavbar({ studentId, refreshNeeded }) {
    const theme = useTheme();
    const navigate = useNavigate();
    const [newNotificationsLength, setNewNotificationsLength] = useState(0);
    const [notifications, setNotifications] = useState([]);
    const { profile, logout } = useProfile();

    useEffect(() => {
        fetch(`/api/notifications/byStudentId/${studentId}`)
            .then(response => response.json())
            .then(data => {
                setNotifications(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [studentId, refreshNeeded, profile]);

    useEffect(() => {
        if (notifications.length > 0) {
            const newNotifications = notifications.filter(n => !n.read);
            setNewNotificationsLength(newNotifications.length);
        }
    }, [notifications]);

    const clickHandler = (value) => {
        console.log(`studentid a parentnavbar clickhandlerjében: ${studentId}`);
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

    const logoutHandler = async () => {
        await logout();
        navigate("/signin");
    };

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
                <div>
                    {profile && (
                        <StyledTypography sx={{ marginRight: 2 }}>
                            {`${profile.familyName} ${profile.firstName} - (gyermek:${profile.childName})`}
                        </StyledTypography>
                    )}
                    <AccountCircleIcon
                        sx={{ cursor: 'pointer', fontSize: 30, color: theme.palette.text.primary }}
                    />  <StyledTypography sx={{ marginRight: 2 }}>
                    <button onClick={logoutHandler}>Kilépés</button>
                </StyledTypography>
                </div>
            </Toolbar>
        </AppBar>
    );
}

export default ParentNavbar;
