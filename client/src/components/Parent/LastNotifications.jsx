import React from "react";
import { LastNotificationsBox, StyledHeading, LastNotificationsGrid, LastNotifHead, LastNotifLabel, LastNotifInfo, LastNotificationsCard } from '../../../StyledComponents';

function LastNotifications({ newNotifications, onClick }) {
    const clickHandler = (notification) => {
        onClick(notification);
    };

    return (
        <LastNotificationsBox>
            <StyledHeading>
                {newNotifications.length <= 0 ? "Nincs új értesítés" : "Legújabb értesítések"}
            </StyledHeading>
            <LastNotificationsGrid>
                {newNotifications.map((notification) => (
                    <LastNotificationsCard key={notification.id} onClick={() => clickHandler(notification)}>
                        <LastNotifHead>
                            {notification.type === "Homework" ? "Házi feladat" :
                                notification.type === "Other" ? "Egyéb értesítés" :
                                    notification.type === "Exam" ? "Dolgozat" :
                                        notification.type === "MissingEquipment" ? "Hiányzó felszerelés" : ""}
                        </LastNotifHead>
                        <LastNotifLabel>
                            Dátum: {new Date(notification.date).toLocaleDateString()}
                        </LastNotifLabel>
                        <LastNotifLabel>
                            Határidő: {new Date(notification.dueDate).toLocaleDateString()}
                        </LastNotifLabel>
                        <LastNotifInfo>
                            Tantárgy: {notification.subjectName}
                        </LastNotifInfo>
                        <LastNotifInfo>
                            Leírás: {notification.description.length > 3 ? `${notification.description.substring(0, 3)}...` : notification.description}
                        </LastNotifInfo>
                    </LastNotificationsCard>
                ))}
            </LastNotificationsGrid>
        </LastNotificationsBox>
    );
}

export default LastNotifications;
