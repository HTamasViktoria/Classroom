import { StyledBadge, StyledIconsBox, StyledNotificationBadgeBox, StyledNotificationIcon, StyledNotificationsBadgeTypography } from "../../../StyledComponents.js";
import React from "react";

function NotificationIconItem({ count, onClick, dataValue, Icon, label }) {
    return (
        <StyledIconsBox>
            <StyledBadge badgeContent={count} color="error">
                <StyledNotificationBadgeBox onClick={onClick} data-value={dataValue}>
                    <StyledNotificationIcon as={Icon} />
                </StyledNotificationBadgeBox>
            </StyledBadge>
            <StyledNotificationsBadgeTypography>
                {label}
            </StyledNotificationsBadgeTypography>
        </StyledIconsBox>
    );
}

export default NotificationIconItem;
