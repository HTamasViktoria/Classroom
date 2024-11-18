import {LastNotifHead, LastNotificationsCard, LastNotifInfo, LastNotifLabel} from "../../../StyledComponents.js";
import {CardContent} from "@mui/material";
import React from "react";

function LatestNotifCard({notification}) {


    return (<LastNotificationsCard>
        <CardContent>
            <LastNotifHead>
                Utolsó kiküldött értesítés
            </LastNotifHead>
            <LastNotifLabel>
                Dátum: {new Date(notification.date).toLocaleDateString()}
            </LastNotifLabel>
            <LastNotifLabel>
                Tanuló: {notification.studentName}
            </LastNotifLabel>
            <LastNotifLabel>
                Dátum: {new Date(notification.date).toLocaleString()}
            </LastNotifLabel>
            <LastNotifInfo>
                Tantárgy: {notification.subjectName}
            </LastNotifInfo>
            <LastNotifInfo>
                Leírás: {notification.description.length > 3 ? `${notification.description.substring(0, 3)}...` : notification.description}
            </LastNotifInfo>
        </CardContent>
    </LastNotificationsCard>)
}

export default LatestNotifCard