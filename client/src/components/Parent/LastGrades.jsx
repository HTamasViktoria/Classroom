import React from "react";
import { CardContent } from "@mui/material";
import { LastNotificationsBox, StyledHeading, LastNotificationsGrid, LastNotificationsCard, LastNotifLabel, LastNotifInfo } from '../../../StyledComponents';

function LastGrades({ newGrades, onClick }) {
    return (
        <LastNotificationsBox>
            <StyledHeading>
                {newGrades.length <= 0 ? "Nincs új osztályzat" : "Legújabb osztályzatok"}
            </StyledHeading>
            <LastNotificationsGrid container spacing={3}>
                {newGrades.map((grade) => (
                    <LastNotificationsGrid item key={grade.id} xs={12} sm={6} md={4}>
                        <LastNotificationsCard onClick={() => onClick(grade)}>
                            <CardContent>
                                <LastNotifLabel>
                                    Dátum: {new Date(grade.date).toLocaleDateString()}
                                </LastNotifLabel>
                                <LastNotifLabel>
                                    Tantárgy: {grade.subject}
                                </LastNotifLabel>
                                <LastNotifInfo>
                                    {grade.forWhat}
                                </LastNotifInfo>
                                <LastNotifInfo>
                                    Osztályzat: {grade.value}
                                </LastNotifInfo>
                            </CardContent>
                        </LastNotificationsCard>
                    </LastNotificationsGrid>
                ))}
            </LastNotificationsGrid>
        </LastNotificationsBox>
    );
}

export default LastGrades;
