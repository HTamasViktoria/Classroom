import { AppBar, Toolbar } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useTheme } from '@mui/material/styles';
import { StyledTypography } from '../../../StyledComponents';
import React from "react";
import { useProfile } from "../../contexts/ProfileContext.jsx";

function TeacherNavbar() {
    const navigate = useNavigate();
    const theme = useTheme();
    const { profile, logout } = useProfile();

    const logoutHandler = async () => {
        await logout();
        navigate("/signin");
    };

    return (
        <AppBar sx={{ backgroundColor: theme.palette.navbar.main }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                <StyledTypography
                    variant="h6"
                    onClick={() => navigate("/grades")}
                    sx={{ cursor: 'pointer' }}
                >
                    Jegyek
                </StyledTypography>

                <div style={{ flexGrow: 1 }} />

                <StyledTypography sx={{ marginRight: 2 }}>
                    {`${profile.familyName} ${profile.firstName} - tanár`}
                </StyledTypography>

                <AccountCircleIcon
                    sx={{ cursor: 'pointer', fontSize: 30, color: theme.palette.text.primary }}
                />
                <StyledTypography sx={{ marginRight: 2 }}>
                    <button onClick={logoutHandler}>Kilépés</button>
                </StyledTypography>
            </Toolbar>
        </AppBar>
    );
}

export default TeacherNavbar;
