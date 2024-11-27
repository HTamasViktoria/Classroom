import React from 'react';
import { Navigate } from 'react-router-dom';
import { useProfile } from './ProfileContext.jsx';

const ProtectedRoute = ({ element, allowedRoles = [] }) => {
    const { profile} = useProfile();

  
  
  const actualRole = localStorage.getItem('role')
    const isAuthorized = localStorage.getItem('isLoggedIn') &&
        actualRole &&
        allowedRoles.length > 0 &&
        allowedRoles.includes(actualRole);
    


    return isAuthorized ? element : <Navigate to="/signin" />;
};

export default ProtectedRoute;
