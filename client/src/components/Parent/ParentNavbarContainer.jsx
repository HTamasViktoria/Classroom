import {NavbarAppBar, NavbarToolbar, NavbarAccountIcon,NavbarButton,  NavbarTypography, NavbarBadge, NavbarSpacer, NavbarBox} from '../../../StyledComponents.js'
import { useNavigate } from 'react-router-dom';
function ParentNavbarContainer({newGradesLength, newNotificationsLength, profile, studentId}){
    
    
    const navigate = useNavigate();
    
    
    const menuItems = [
        { label: 'Üzenetek', value: 'messages' },
        { label: 'Osztályzatok', value: 'grades', badge: newGradesLength },
        { label: 'Értesítések', value: 'notifications', badge: newNotificationsLength },
        { label: 'Hiányzások', value: 'absences' },
        { label: 'Órarend', value: 'schedule' },
        { label: 'Csengetési rend', value: 'bell-schedule' },
    ];

    const logoutHandler = async () => {
        await logout();
        navigate("/signin");
    };

    const profileClickHandler = () => {
        navigate(`/parent/profile/${profile.id}`);
    };
    const clickHandler = (value) => {
        navigate(`/parent/${value}/${studentId}`);
    };
    
    return(   
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

                <NavbarSpacer/>

                {profile && (
                    <NavbarBox onClick={profileClickHandler}>
                        <NavbarTypography>
                            {`${profile.familyName} ${profile.firstName} - (gyermek: ${profile.childName})`}
                        </NavbarTypography>
                        <NavbarAccountIcon />
                    </NavbarBox>
                )}

                <NavbarSpacer/>

                <NavbarTypography>
                    <NavbarButton onClick={logoutHandler}>Kilépés</NavbarButton>
                </NavbarTypography>
            </NavbarToolbar>
        </NavbarAppBar>)
}


export default ParentNavbarContainer