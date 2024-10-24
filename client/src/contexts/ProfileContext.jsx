import React, { createContext, useContext, useState } from "react";

const ProfileContext = createContext(null);

export const useProfile = () => {
    return useContext(ProfileContext);
};

export const ProfileContextProvider = ({ children }) => {
    const [profile, setProfile] = useState(null);
    const isAuthenticated = !!profile;

    const login = (userProfile) => {
        setProfile(userProfile);
        localStorage.setItem('isLoggedIn', true);
        const logoutTime = new Date();
        logoutTime.setMinutes(logoutTime.getMinutes() + 30);
        localStorage.setItem('logoutTime', logoutTime.getTime());
    };
    

    const logout = async () => {
        try {
            const response = await fetch('/api/auth/sign-out', {
                method: 'POST',
                credentials: 'include',
            });

            if (response.ok) {
                setProfile(null);
                //setIsAuthenticated(false);
            } else {
                console.error('Logout failed');
            }
        } catch (error) {
            console.error('Error during logout:', error);
        }
    };

    return (
        <ProfileContext.Provider value={{ profile, setProfile, login, logout, isAuthenticated }}>
            {children}
        </ProfileContext.Provider>
    );
};
