import React, {useState} from 'react';
import { useNavigate } from 'react-router-dom';
import { NavbarAppBar, NavbarToolbar, NavbarAccountIcon, NavbarButton, NavbarTypography, NavbarBadge, NavbarSpacer, NavbarBox } from '../../../StyledComponents.js';
import { useProfile } from '../../contexts/ProfileContext.jsx';
import GetNewMessagesNum from "../../common/components/GetNewMessagesNum.jsx";

function TeacherNavbarContainer({ profile, id }) {
    const navigate = useNavigate();
    const { logout } = useProfile();

    const [newMessagesNum, setNewMessagesNum] = useState(0)

    const menuItems = [
        { label: 'Jegyek', value: 'grades' },
        { label: 'Üzenetek', value: 'messages',badge: newMessagesNum },
        { label: 'Értesítések', value: 'notifications' },
        { label: 'Hiányzások', value: 'absences' },
        { label: 'Órarend', value: 'schedule' },
        { label: 'Csengetési rend', value: 'bell-schedule' },
    ];

    const logoutHandler = async () => {
        await logout();
        navigate("/signin");
    };

    const profileClickHandler = () => {
        navigate(`/teacher/profile/${profile.id}`);
    };

    const clickHandler = (value) => {
        navigate(`/teacher/${value}/${id}`);
    };

    return (<> <GetNewMessagesNum id={id} onData={(data)=>setNewMessagesNum(data)}/>
        <NavbarAppBar>
            <NavbarToolbar>
                {menuItems.map(item => (
                    <NavbarTypography
                        key={item.value}
                        onClick={() => clickHandler(item.value)}
                    >
                        {item.label}
                        {item.badge > 0 && (
                            <NavbarBadge badgeContent={item.badge} color="error" />
                        )}
                    </NavbarTypography>
                ))}

                <NavbarSpacer />

                {profile && (
                    <NavbarBox onClick={profileClickHandler}>
                        <NavbarTypography>
                            {`${profile.familyName} ${profile.firstName} tanár`}
                        </NavbarTypography>
                        <NavbarAccountIcon />
                    </NavbarBox>
                )}

                <NavbarSpacer />

                <NavbarTypography>
                    <NavbarButton onClick={logoutHandler}>Kilépés</NavbarButton>
                </NavbarTypography>
            </NavbarToolbar>
        </NavbarAppBar>
        </>
    );
}

export default TeacherNavbarContainer;
