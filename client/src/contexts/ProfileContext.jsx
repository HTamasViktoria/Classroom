import React, { createContext, useContext, useState, useEffect } from "react";

const ProfileContext = createContext(null);

export const useProfile = () => {
    return useContext(ProfileContext);
};

export const ProfileContextProvider = ({ children }) => {
    
    const [profile, setProfile] = useState(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const storedIsLoggedIn = localStorage.getItem('isLoggedIn');
        const storedProfile = localStorage.getItem('profile');

        if (storedIsLoggedIn && storedProfile) {
            const parsedProfile = JSON.parse(storedProfile);
            setProfile(parsedProfile);
            setIsAuthenticated(true);
        }
    }, []);

    const login = (userProfile) => {
        setProfile(userProfile);
        setIsAuthenticated(true);
        

        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('profile', JSON.stringify(userProfile));
        localStorage.setItem('role', userProfile.role);
        localStorage.setItem('id', userProfile.id);

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
                setIsAuthenticated(false);
                localStorage.removeItem('isLoggedIn');
                localStorage.removeItem('profile');
                localStorage.removeItem('logoutTime');
                localStorage.removeItem('role');
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
