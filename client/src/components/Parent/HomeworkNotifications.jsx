import React, { useState, useEffect } from 'react';
import { Card, CardContent, Typography, Button, Box } from '@mui/material';
import { Home } from '@mui/icons-material';

function HomeworkNotifications(props) {
    const [refreshNeeded, setRefreshNeeded] = useState(false);
    const [homeworks, setHomeworks] = useState([]);

    useEffect(() => {
        fetch(`/api/notifications/homeworks`)
            .then(response => response.json())
            .then(data => setHomeworks(data))
            .catch(error => console.error('Error fetching data:', error));
    }, [refreshNeeded]);

    const deleteHandler = (e) => {
        const id = e.target.id;
        props.onRefreshing();

        fetch(`/api/notifications/delete/${id}`, { method: 'DELETE' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to delete notification');
                }
                return response.json();
            })
            .then(data => {
                console.log(data.message);
                setRefreshNeeded(prev => !prev);
            })
            .catch(error => console.error('Error deleting notification:', error));
    };

    const setToReadHandler = (e) => {
        const id = e.target.id;
        props.onRefreshing();

        fetch(`/api/notifications/setToRead/${id}`, { method: 'POST' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to set notification to read');
                }
                return response.json();
            })
            .then(() => {
                setRefreshNeeded(prev => !prev);
            })
            .catch(error => console.error('Error setting notification to read:', error));
    };

    return (
        <div>
            {homeworks.map(homework => (
                <Card key={`id:${homework.id}`} sx={{ marginBottom: 2, backgroundColor: '#f0f0f0', border: '1px solid #ccc', borderRadius: 2 }}>
                    <CardContent>
                        <Typography variant="h6" sx={{ color: '#82b2b8' }}>
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
                            Dátum: {new Date(homework.date).toISOString().split('T')[0]}
                        </Typography>
                        {homework.optionalDescription && (
                            <Typography variant="body2" color="textSecondary" sx={{ marginY: 1 }}>
                                Opcionális leírás: {homework.optionalDescription}
                            </Typography>
                        )}
                        <Box display="flex" justifyContent="space-between" sx={{ marginTop: 2 }}>
                            {homework.read == false && (
                                <Button
                                    onClick={setToReadHandler}
                                    id={homework.id}
                                    variant="contained"
                                    sx={{
                                        flex: 1,
                                        marginRight: 1,
                                        backgroundColor: '#82b2b8',
                                        '&:hover': { backgroundColor: '#6a9fa2' },
                                        color: '#fff'
                                    }}
                                    size="small">
                                    Ok, elolvastam
                                </Button>
                            )}
                            <Button
                                onClick={setToReadHandler}
                                id={homework.id}
                                variant="contained"
                                sx={{
                                    flex: 1,
                                    marginRight: 1,
                                    backgroundColor: '#a2c4c6',
                                    '&:hover': { backgroundColor: '#8eaeb0' },
                                    color: '#fff'
                                }}
                                size="small">
                                {homework.read ? "Olvasatlannak jelöl" : "Erre még visszatérek"}
                            </Button>
                            {homework.read && (
                                <Button
                                    id={homework.id}
                                    onClick={deleteHandler}
                                    variant="contained"
                                    sx={{
                                        flex: 1,
                                        marginLeft: 1,
                                        backgroundColor: '#d9c2bd',
                                        '&:hover': { backgroundColor: '#c2a6a0' },
                                        color: '#fff'
                                    }}
                                    size="small">
                                    Értesítés törlése
                                </Button>
                            )}
                        </Box>
                    </CardContent>
                </Card>
            ))}
        </div>
    );
}

export default HomeworkNotifications;
