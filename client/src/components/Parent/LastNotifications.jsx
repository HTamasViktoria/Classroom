import { Box, Card, CardContent, Grid, Typography } from "@mui/material";
import React from "react";

function LastNotifications({ lastNotifications, onClick }) {

    const clickHandler = (e) => {
        onClick(e);
    };

    return (
        <Box sx={{ mt: 4 }}>
            <Typography variant="h4" gutterBottom>
                {lastNotifications.length <=0 ? "Nincs új értesítés" : "Legújabb értesítések" }
            </Typography>
            <Grid container spacing={10}>
                {lastNotifications.map((notification) => (
                    <Grid item xs={12} sm={6} md={4} key={notification.id}>
                        <Card
                            sx={{
                                backgroundColor: '#a2c4c6',
                                boxShadow: 3,
                                transition: '0.3s',
                                minWidth: '175px',
                                '&:hover': {
                                    transform: 'scale(1.05)',
                                    boxShadow: 6
                                }
                            }}
                            onClick={() => clickHandler(notification)}
                        >
                            <CardContent>
                                <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                                    {notification.type}
                                </Typography>
                                <Typography color="text.secondary">
                                    Dátum: {new Date(notification.date).toLocaleDateString()}
                                </Typography>
                                <Typography color="text.secondary">
                                    Határidő: {new Date(notification.dueDate).toLocaleDateString()}
                                </Typography>
                                <Typography variant="body2" sx={{ mt: 1 }}>
                                    Tantárgy: {notification.subjectName}
                                </Typography>
                                <Typography variant="body2" sx={{ mt: 1 }}>
                                    Leírás: {notification.description.length > 10 ? `${notification.description.substring(0, 10)}...` : notification.description}
                                </Typography>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Box>
    );
}

export default LastNotifications;
