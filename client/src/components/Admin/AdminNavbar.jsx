import { AppBar, Toolbar, Typography } from '@mui/material';
import { useNavigate } from "react-router-dom";
import {StyledTypography} from "../../../StyledComponents.js";
import React from "react";
import { useProfile } from "../../contexts/ProfileContext.jsx";


function AdminNavbar() {
    const navigate = useNavigate();


    const { profile, logout } = useProfile();

    const logoutHandler = async () => {
        await logout();
        navigate("/signin");
    };
    
    
    return (
        <AppBar sx={{ backgroundColor: '#c6ac85' }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'left', gap: 6, width: '100%' }}>
                <Typography
                    component='div'
                    onClick={() => navigate("/admin/teachers")}
                    style={{ cursor: 'pointer' }}
                >
                    Tanárok
                </Typography>
                <Typography
                    component='div'
                    onClick={() => navigate("/admin/students")}
                    style={{ cursor: 'pointer' }}
                >
                    Diákok
                </Typography>
                <Typography
                    component='div'
                    onClick={() => navigate("/admin/classes")}
                    style={{ cursor: 'pointer' }}
                >
                    Osztályok
                </Typography>
                <StyledTypography sx={{ marginRight: 2 }}>
                    <button onClick={logoutHandler}>Kilépés</button>
                </StyledTypography>
            </Toolbar>
        </AppBar>
    );
}

export default AdminNavbar;
