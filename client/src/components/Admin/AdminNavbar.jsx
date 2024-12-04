import React from "react";
import { useNavigate } from "react-router-dom";
import { useProfile } from "../../contexts/ProfileContext.jsx";
import { NavbarAppBar, NavbarToolbar, NavbarTypography, NavbarBox, NavbarAccountIcon, NavbarBadge, NavbarButton, NavbarSpacer } from "../../../StyledComponents.js";

function AdminNavbar() {
    const navigate = useNavigate();
    const { profile, logout } = useProfile();

    const logoutHandler = async () => {
        await logout();
        navigate("/signin");
    };

    return (
        <NavbarAppBar>
            <NavbarToolbar>
                <NavbarTypography onClick={() => navigate("/admin/teachers")}>
                    Tanárok
                </NavbarTypography>
                <NavbarTypography onClick={() => navigate("/admin/students")}>
                    Diákok
                </NavbarTypography>
                <NavbarTypography onClick={() => navigate("/admin/parents")}>
                    Szülők
                </NavbarTypography>
                <NavbarTypography onClick={() => navigate("/admin/classes")}>
                    Osztályok
                </NavbarTypography>

                <NavbarSpacer />

                <NavbarTypography>
                    {profile.role}
                </NavbarTypography>

                <NavbarTypography>
                    <NavbarButton onClick={logoutHandler}>Kilépés</NavbarButton>
                </NavbarTypography>
            </NavbarToolbar>
        </NavbarAppBar>
    );
}

export default AdminNavbar;
