import { Box, Card, CardContent, Grid } from "@mui/material";
import React from "react";
import { StyledTypography } from '../../../StyledComponents';

function LastNotifications({ lastNotifications, onClick }) {
    const clickHandler = (e) => {
        onClick(e);
    };

    return (
        <Box sx={{ mt: 4 }}>
            <StyledTypography variant="h4" gutterBottom>
                {lastNotifications.length <= 0 ? "Nincs új értesítés" : "Legújabb értesítések"}
            </StyledTypography>
            <Grid container spacing={3}>
                {lastNotifications.map((notification) => (
                    <Grid item xs={12} sm={6} md={4} key={notification.id}>
                        <Card
                            sx={{
                                backgroundColor: 'secondary.main',
                                boxShadow: 3,
                                transition: '0.3s',
                                minWidth: '175px',
                                '&:hover': {
                                    transform: 'scale(1.05)',
                                    boxShadow: 6,
                                },
                            }}
                            onClick={() => clickHandler(notification)}
                        >
                            <CardContent>
                                <StyledTypography variant="h6" sx={{ fontWeight: 'bold', color: 'text.secondary' }}>
                                    {notification.type === "Homework" ? "Házi feladat" : 
                                    notification.type === "Other" ? "Egyéb értesítés" :
                                    notification.type === "Exam" ? "Dolgozat":
                                    notification.type === "MissingEquipment" ? "Hiányzó felszerelés" : ""}
                                </StyledTypography>
                                <StyledTypography color="text.secondary">
                                    Dátum: {new Date(notification.date).toLocaleDateString()}
                                </StyledTypography>
                                <StyledTypography color="text.secondary">
                                    Határidő: {new Date(notification.dueDate).toLocaleDateString()}
                                </StyledTypography>
                                <StyledTypography variant="body2" sx={{ mt: 1 }}>
                                    Tantárgy: {notification.subjectName}
                                </StyledTypography>
                                <StyledTypography variant="body2" sx={{ mt: 1 }}>
                                    Leírás: {notification.description.length > 3 ? `${notification.description.substring(0, 3)}...` : notification.description}
                                </StyledTypography>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Box>
    );
}

export default LastNotifications;
