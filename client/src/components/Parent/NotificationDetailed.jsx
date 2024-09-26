import {
    Box,
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography
} from "@mui/material";
import React from "react";

function NotificationDetailed({ notification, onButtonClick, onRefreshNeeded }) {

    const deleteHandler = (e) => {
        const id = e.target.id;
        onButtonClick();

        fetch(`/api/notifications/delete/${id}`, { method: 'DELETE' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to delete notification');
                }
                return response.json();
            })
            .then(data => {
                console.log(data.message);
            })
            .catch(error => console.error('Error deleting notification:', error));
    };

    const setToReadHandler = (e) => {
        const id = e.target.id;
        fetch(`/api/notifications/setToRead/${id}`, { method: 'POST' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to set notification to read');
                }
                return response.json();
            })
            .then(() => {
                onRefreshNeeded();
                onButtonClick();
            })
            .catch(error => console.error('Error setting notification to read:', error));
    };

    const goBackHandler=()=>{
        onButtonClick();
    }
    
    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                {notification.type}
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="notification table">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Dátum</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Határidő</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Tantárgy</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Tanár</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Leírás</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>További leírás</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <TableRow key={notification.id}>
                            <TableCell>{new Date(notification.date).toLocaleDateString()}</TableCell>
                            <TableCell>{new Date(notification.dueDate).toLocaleDateString()}</TableCell>
                            <TableCell>{notification.subjectName}</TableCell>
                            <TableCell>{notification.teacherName}</TableCell>
                            <TableCell>{notification.description}</TableCell>
                            <TableCell>{notification.optionalDescription}</TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
            <Box display="flex" justifyContent="space-between" sx={{ marginTop: 2 }}>
                {notification.read === false && (
                    <Button
                        onClick={setToReadHandler}
                        id={notification.id}
                        variant="contained"
                        sx={{
                            backgroundColor: '#82b2b8',
                            color: '#fff',
                            flex: 1,
                            marginRight: 1
                        }}>
                        Ok, elolvastam
                    </Button>
                )}
                <Button
                    onClick={goBackHandler}
                    id={notification.id}
                    variant="contained"
                    sx={{
                        backgroundColor: '#a2c4c6',
                        color: '#fff',
                        flex: 1,
                        marginRight: 1
                    }}>
                    "Erre még visszatérek"
                </Button>

                {notification.read && (
                    <Button
                        id={notification.id}
                        onClick={deleteHandler}
                        variant="contained"
                        sx={{
                            backgroundColor: '#d9c2bd',
                            color: '#fff',
                            flex: 1,
                            marginLeft: 1
                        }}>
                        Értesítés törlése
                    </Button>
                )}
            </Box>
        </Box>
    );
}

export default NotificationDetailed;
