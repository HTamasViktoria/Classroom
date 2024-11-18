import React, { useEffect } from "react";
import { useProfile } from "../../contexts/ProfileContext.jsx";
import TeacherNavbarContainer from "./TeacherNavbarContainer.jsx";

function TeacherNavbar({ id }) {
    const { profile, logout } = useProfile();

    useEffect(() => {
       
        const isLoggedIn = localStorage.getItem('isLoggedIn');
        console.log("isLoggedIn from localStorage:", isLoggedIn);
    }, []);

    return (
        <TeacherNavbarContainer id={id} profile={profile} />
    );
}

export default TeacherNavbar;
