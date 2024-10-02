import React from "react";
import AddAlertIcon from "@mui/icons-material/AddAlert";
import AppRegistrationIcon from "@mui/icons-material/AppRegistration";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import { CustomBox, StyledButton, StyledSecondaryButton, StyledTypography } from '../../../StyledComponents';

function TaskSelector(props) {
    return (
        <CustomBox sx={{ padding: 2, display: 'flex', justifyContent: 'space-between', gap: 2 }}>
            <StyledButton
                onClick={() => props.onChosenTask("addNotification")}
                startIcon={<AddAlertIcon />}
            >
                <StyledTypography>Értesítést küldök</StyledTypography>
            </StyledButton>

            <StyledSecondaryButton
                onClick={() => props.onChosenTask("addGrade")}
                startIcon={<AppRegistrationIcon />}
            >
                <StyledTypography>Osztályzatot adok</StyledTypography>
            </StyledSecondaryButton>

            <StyledButton
                onClick={() => props.onChosenTask("addMessage")}
                startIcon={<ChatBubbleOutlineIcon />}
            >
                <StyledTypography>Üzenetet küldök</StyledTypography>
            </StyledButton>
        </CustomBox>
    );
}

export default TaskSelector;
