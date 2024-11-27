import React from "react";
import AddAlertIcon from "@mui/icons-material/AddAlert";
import AppRegistrationIcon from "@mui/icons-material/AppRegistration";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import { CustomBox, AButton, BButton, StyledTypography } from '../../../StyledComponents';

function TaskSelector(props) {
    return (
        <CustomBox sx={{ padding: 2, display: 'flex', justifyContent: 'space-between', gap: 2 }}>
            <AButton
                onClick={() => props.onChosenTask("addNotification")}
                startIcon={<AddAlertIcon />}
            >
                <StyledTypography>Értesítést küldök</StyledTypography>
            </AButton>

            <BButton
                onClick={() => props.onChosenTask("grades")}
                startIcon={<AppRegistrationIcon />}
            >
                <StyledTypography>Osztályzat</StyledTypography>
            </BButton>

            <AButton
                onClick={() => props.onChosenTask("addMessage")}
                startIcon={<ChatBubbleOutlineIcon />}
            >
                <StyledTypography>Üzenetet küldök</StyledTypography>
            </AButton>
        </CustomBox>
    );
}

export default TaskSelector;
