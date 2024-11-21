import {NavbarAppBar, NavbarToolbar, NavbarAccountIcon,NavbarButton,  NavbarTypography, NavbarBadge, NavbarSpacer, NavbarBox} from '../../../StyledComponents.js'
import { useNavigate } from 'react-router-dom';
import {useProfile} from "../../contexts/ProfileContext.jsx";
import GetNewMessagesNum from "../../common/components/GetNewMessagesNum.jsx";
import {useState} from "react";
function ParentNavbarContainer({newGradesLength, newNotificationsLength,studentId}){
    
    
    const navigate = useNavigate();
    const [newMessagesNum, setNewMessagesNum] = useState(0)
    
    const {logout, profile} = useProfile()
    
    const menuItems = [
        { label: 'Üzenetek', value: 'messages' , badge: newMessagesNum},
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
        <>
            <GetNewMessagesNum id={profile.id} onData={(data)=>setNewMessagesNum(data)}/>
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
        </NavbarAppBar>
        </>)
}


export default ParentNavbarContainer