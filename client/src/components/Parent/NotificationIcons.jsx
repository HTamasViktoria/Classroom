
import AssignmentIcon from "@mui/icons-material/Assignment";
import BackpackIcon from "@mui/icons-material/Backpack";
import HomeWorkIcon from "@mui/icons-material/HomeWork";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import React from "react";
import {NotificationIconsBox } from "../../../StyledComponents.js";
import NotificationIconItem from "./NotificationIconItem.jsx";


function NotificationIcons({ notifications, onClick }) {
    const { exams, homeworks, missingEquipments, others } = notifications;

    const clickHandler = (e) => {
        const chosen = e.currentTarget.getAttribute("data-value");
        onClick(chosen);
    };

    return (
        <NotificationIconsBox>
            <NotificationIconItem
                count={exams.length}
                onClick={clickHandler}
                dataValue={"exams"}
                Icon={AssignmentIcon}
                label="Vizsgák"
            />
            <NotificationIconItem
                count={missingEquipments.length}
                onClick={clickHandler}
                dataValue={"missingEquipments"}
                Icon={BackpackIcon}
                label="Hiányzó Eszközök"
            />
            <NotificationIconItem
                count={homeworks.length}
                onClick={clickHandler}
                dataValue={"homeworks"}
                Icon={HomeWorkIcon}
                label="Házi Feladatok"
            />
            <NotificationIconItem
                count={others.length}
                onClick={clickHandler}
                dataValue={"others"}
                Icon={InfoOutlinedIcon}
                label="Egyéb Értesítések"
            />
        </NotificationIconsBox>
    );
}

export default NotificationIcons;
