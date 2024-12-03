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
        const storedLogoutTime = localStorage.getItem('logoutTime');

        if (storedIsLoggedIn && storedProfile && storedLogoutTime) {
            const currentTime = new Date().getTime();
            const parsedLogoutTime = parseInt(storedLogoutTime);

 
            if (currentTime > parsedLogoutTime) {
                logout();
            } else {
                const parsedProfile = JSON.parse(storedProfile);
                setProfile(parsedProfile);
                setIsAuthenticated(true);
                startSessionTimer(parsedLogoutTime - currentTime);
            }
        }
    }, []);

    const login = (userProfile) => {
        setProfile(userProfile.user);
        setIsAuthenticated(true);

        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('token', userProfile.token);
        localStorage.setItem('profile', JSON.stringify(userProfile));
       
        localStorage.setItem('role', userProfile.user.role);
      
        localStorage.setItem('id', userProfile.user.id);

        const logoutTime = new Date();
        logoutTime.setMinutes(logoutTime.getMinutes() + 30);
        localStorage.setItem('logoutTime', logoutTime.getTime());

        startSessionTimer(30 * 60 * 1000);
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


    const startSessionTimer = (timeRemaining) => {

        if (window.sessionTimeout) {
            clearTimeout(window.sessionTimeout);
        }


        window.sessionTimeout = setTimeout(() => {
            logout();
        }, timeRemaining);
    };


    useEffect(() => {
        const resetSessionTimer = () => {
            const logoutTime = localStorage.getItem('logoutTime');
            if (logoutTime) {
                const remainingTime = parseInt(logoutTime) - new Date().getTime();
                if (remainingTime > 0) {
                    startSessionTimer(remainingTime);
                }
            }
        };

     
        window.addEventListener("mousemove", resetSessionTimer);

      
        return () => {
            window.removeEventListener("mousemove", resetSessionTimer);
        };
    }, []);

    return (
        <ProfileContext.Provider value={{ profile, setProfile, login, logout, isAuthenticated }}>
            {children}
        </ProfileContext.Provider>
    );
};
