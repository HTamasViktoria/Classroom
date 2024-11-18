import React, { useEffect, useState } from "react";
import LastNotifications from "./LastNotifications.jsx";
import NotificationDetailed from "./NotificationDetailed.jsx";
import { AButton } from '../../../StyledComponents';
import { useNavigate } from "react-router-dom";
import LastGrades from "./LastGrades.jsx";
import NewNotificationFetcher from "./NewNotificationFetcher.jsx";
import NewGradeFetcher from "./NewGradeFetcher.jsx";
import GradeDetailed from "./GradeDetailed.jsx";

function ParentMain({ studentId, onRefreshNeeded }) {
    const navigate = useNavigate();
    const [chosenNotification, setChosenNotification] = useState(null);
    const [chosenGrade, setChosenGrade] = useState(null);
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
            
            {chosenNotification ? (
                <NotificationDetailed
                    notification={chosenNotification}
                    onButtonClick={() => setChosenNotification(null)}
                    onRefreshNeeded={onRefreshNeeded}
                />
            ) : chosenGrade ? (
                <GradeDetailed
                    grade={chosenGrade}
                    onButtonClick={() => setChosenGrade(null)}
                    onRefreshNeeded={onRefreshNeeded}
                />
            ) : (
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

                    <LastGrades
                        newGrades={newGrades}
                        onClick={(grade) => setChosenGrade(grade)}
                    />
                    <AButton onClick={() => navigate(`/parent/grades/${studentId}`)}>
                        Összes osztályzat megtekintése
                    </AButton>
                </>
            )}
        </>
    );
}

export default ParentMain;
