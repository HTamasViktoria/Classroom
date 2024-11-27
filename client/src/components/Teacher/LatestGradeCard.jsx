import {LastNotifHead, LastNotificationsCard, LastNotifInfo, LastNotifLabel} from "../../../StyledComponents.js";
import {CardContent} from "@mui/material";
import React from "react";

function LatestGradeCard({grade}) {


    return (<LastNotificationsCard>
        <CardContent>
            <LastNotifHead>
                Utolsó rögzített jegy
            </LastNotifHead>
            <LastNotifLabel>
                Dátum: {new Date(grade.date).toLocaleDateString()}
            </LastNotifLabel>
            <LastNotifLabel>
                Tanuló: {grade.studentName}!!!!!!!!!!!!!!!
            </LastNotifLabel>
            <LastNotifLabel>
                Tantárgy: {grade.subject}
            </LastNotifLabel>
            <LastNotifInfo>
                Mire kapta: {grade.forWhat}
            </LastNotifInfo>
        </CardContent>
    </LastNotificationsCard>)
}

export default LatestGradeCard