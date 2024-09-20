import React from 'react';
import { Card, CardContent, Typography, Button, Box } from '@mui/material';
import { Home } from '@mui/icons-material';

function HomeworkNotifications({ homeworks }) {
    return (
        <div>
            {homeworks.map(homework => (
                <Card key={`id:${homework.id}`} sx={{ marginBottom: 2, border: '1px solid #ccc', borderRadius: 2 }}>
                    <CardContent>
                        <Typography variant="h6" color="primary">
                            <Home sx={{ verticalAlign: 'middle', marginRight: 1 }} />
                            {homework.subjectName}
                        </Typography>
                        <Typography variant="subtitle1" color="textSecondary">
                            Tanár: {homework.teacherName}
                        </Typography>
                        <Typography variant="body1" sx={{ marginY: 1 }}>
                            {homework.description}
                        </Typography>
                        <Typography variant="body2" color="textSecondary">
                            Határidő: {homework.date}
                        </Typography>
                        {homework.optionalDescription && (
                            <Typography variant="body2" color="textSecondary" sx={{ marginY: 1 }}>
                                Opcionális leírás: {homework.optionalDescription}
                            </Typography>
                        )}
                        <Box display="flex" justifyContent="space-between" sx={{ marginTop: 2 }}>
                            {homework.read == false && <Button variant="contained" color="primary" size="small" sx={{flex: 1, marginRight: 1}}>
                                Ok, elolvastam
                            </Button>}
                            {homework.read == true && <Button variant="contained" color="primary" size="small" sx={{flex: 1, marginRight: 1}}>
                                Olvasatlannak jelöl
                            </Button>}
                            <Button variant="outlined" color="secondary" size="small" sx={{ flex: 1 }}>
                                Erre még visszatérek
                            </Button>
                        </Box>
                    </CardContent>
                </Card>
            ))}
        </div>
    );
}

export default HomeworkNotifications;
