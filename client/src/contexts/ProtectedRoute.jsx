import React from 'react';
import { Navigate } from 'react-router-dom';
import { useProfile } from '../contexts/ProfileContext.jsx';

const ProtectedRoute = ({ element, allowedRoles = [] }) => {
    const { profile, isAuthenticated } = useProfile();

 
    const isAuthorized = isAuthenticated &&
        profile?.role && 
        allowedRoles.length > 0 &&
        allowedRoles.includes(profile.role);

    return isAuthorized ? element : <Navigate to="/signin" />;
};

export default ProtectedRoute;
