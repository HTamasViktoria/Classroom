import React, { useEffect, useState } from "react";
import LastNotifications from "./LastNotifications.jsx";
import NotificationDetailed from "./NotificationDetailed.jsx";
import { AButton } from '../../../StyledComponents';
import { useNavigate } from "react-router-dom";
import LastGrades from "./LastGrades.jsx";
import NewNotificationFetcher from "./NewNotificationFetcher.jsx";
import NewGradeFetcher from "./NewGradeFetcher.jsx";

function ParentMain({ studentId, onRefreshNeeded }) {
    const navigate = useNavigate();
    const [chosenNotification, setChosenNotification] = useState("");
    const [newNotifications, setNewNotifications] = useState([]);
    const [newGrades, setNewGrades] = useState([]);

    return (
        <>
            <NewNotificationFetcher
                studentId={studentId}
                onData={(data) => setNewNotifications(data)}
                refreshNeeded={onRefreshNeeded}
            />

            <NewGradeFetcher
                studentId={studentId}
                onData={(data) => setNewGrades(data)}
                refreshNeeded={onRefreshNeeded}
            />

            {chosenNotification === "" ? (
                <>
                    <LastNotifications
                        newNotifications={newNotifications}
                        onClick={(notification) => setChosenNotification(notification)}
                    />

                    {newNotifications.length > 0 && (
                        <AButton onClick={() => navigate(`/parent/notifications/${studentId}`)}>
                            Összes értesítés megtekintése
                        </AButton>
                    )}

                    <LastGrades newGrades={newGrades} />
                    <AButton onClick={() => navigate(`/parent/grades/${studentId}`)}>
                        Összes osztályzat megtekintése
                    </AButton>
                </>
            ) : (
                <NotificationDetailed
                    notification={chosenNotification}
                    onButtonClick={() => setChosenNotification("")}
                    onRefreshNeeded={onRefreshNeeded}
                />
            )}
        </>
    );
}

export default ParentMain;
