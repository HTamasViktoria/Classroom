import React, { useState, useEffect } from 'react';
import { AppBar, Toolbar, Badge, useTheme } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useNavigate } from 'react-router-dom';
import { StyledTypography } from '../../../StyledComponents';
import { useProfile } from "../../contexts/ProfileContext.jsx";

function ParentProfile(){

    const theme = useTheme();
    const navigate = useNavigate();
    const { profile, logout } = useProfile();
    
    const [changingPassword, setChangingPassword] = useState(false)


    return (
        <>
            {changingPassword ? (
                <PasswordChanger />
            ) : (
                <>
                    <p>Név: {profile.familyName} {profile.firstName}</p>
                    <p>Felhasználói név: {profile.userName}</p>
                    <p>Email-cím: {profile.email}</p>
                    <button onClick={() => setChangingPassword(true)}>Jelszó változtatás</button>
                </>
            )}
        </>
    );


}

export default ParentProfile