import React, {useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import ParentNavbar from "./ParentNavbar.jsx";
import NotificationIcons from "./NotificationIcons.jsx";
import GeneralNotifications from "./GeneralNotifications.jsx";
import AllNotificationFetcher from "./AllNotificationFetcher.jsx";

function ParentNotificationsMain() {
    
    const { id } = useParams();
    const navigate = useNavigate();
    
  
    const [chosen, setChosen] = useState("");
    const [refreshNeeded, setRefreshNeeded] = useState(false);
    const [notifications, setNotifications] = useState({
        exams: [],
        homeworks: [],
        missingEquipments: [],
        others: []
    });

    const refreshHandler = () => {
        setRefreshNeeded(prevState => !prevState);
    };

  

    return (
        <>
            <ParentNavbar studentId={id} refreshNeeded={refreshNeeded} />
            <AllNotificationFetcher id={id} onData={(data)=>setNotifications(data)} refreshNeeded={refreshNeeded}/>
            
            {chosen === "" ? (
                <>
                    <NotificationIcons
                        notifications={notifications}
                        onClick={(chosenName)=>(setChosen(chosenName))}
                    />
                    <button onClick={() => navigate(`/parent/${id}`)}>Vissza a főoldalra</button>
                </>
            ) : (
                <GeneralNotifications
                    id={id}
                    chosen={chosen}
                    onRefreshing={refreshHandler}
                    refreshNeeded={refreshNeeded}
                    onGoBack={()=>setChosen("")}
                />
            )}
        </>
    );
}

export default ParentNotificationsMain;
