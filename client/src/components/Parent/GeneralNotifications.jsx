import React, { useEffect, useState } from 'react';
import { CustomBox, StyledButton, StyledTypography, StyledTableHead, StyledTableCell } from '../../../StyledComponents';
import { Table, TableBody, TableRow, TableCell } from '@mui/material';

function GeneralNotifications({ chosen, onRefreshing, onGoBack }) {
    const [refreshNeeded, setRefreshNeeded] = useState(false);
    const [notifications, setNotifications] = useState([]);

    useEffect(() => {
        fetch(`/api/notifications/${chosen}`)
            .then(response => response.json())
            .then(data => setNotifications(data))
            .catch(error => console.error('Error fetching data:', error));
    }, [chosen, refreshNeeded]);

    const deleteHandler = (e) => {
        const id = e.target.id;
        onRefreshing();

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
        onRefreshing();

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
    
    const goBackHandler=()=>{
        onGoBack();
    }

    return (
        <CustomBox>
            <StyledTypography variant="h5">Értesítések</StyledTypography>
            <Table>
                <StyledTableHead>
                    <TableRow>
                        <StyledTableCell>Dátum</StyledTableCell>
                        <StyledTableCell>Határidő</StyledTableCell>
                        <StyledTableCell>Tantárgy</StyledTableCell>
                        <StyledTableCell>Tanár</StyledTableCell>
                        <StyledTableCell>Leírás</StyledTableCell>
                        <StyledTableCell>További leírás</StyledTableCell>
                        <StyledTableCell>Opciók</StyledTableCell>
                    </TableRow>
                </StyledTableHead>
                <TableBody>
                    {notifications.map(notification => (
                        <TableRow key={`id:${notification.id}`}>
                            <TableCell>{new Date(notification.date).toISOString().split('T')[0]}</TableCell>
                            <TableCell>{new Date(notification.dueDate).toISOString().split('T')[0]}</TableCell>
                            <TableCell>{notification.subjectName}</TableCell>
                            <TableCell>{notification.teacherName}</TableCell>
                            <TableCell>{notification.description}</TableCell>
                            <TableCell>{notification.optionalDescription}</TableCell>
                            
                            <TableCell>
                                {notification.read === false && (
                                    <StyledButton
                                        onClick={setToReadHandler}
                                        id={notification.id}
                                        variant="contained"
                                        color="primary"
                                        size="small">
                                        Ok, elolvastam
                                    </StyledButton>
                                )}
                                <StyledButton
                                    onClick={setToReadHandler}
                                    id={notification.id}
                                    variant="contained"
                                    color="secondary"
                                    size="small">
                                    {notification.read ? "Olvasatlannak jelöl" : "Erre még visszatérek"}
                                </StyledButton>
                                {notification.read && (
                                    <StyledButton
                                        id={notification.id}
                                        onClick={deleteHandler}
                                        variant="contained"
                                        color="error"
                                        size="small">
                                        Értesítés törlése
                                    </StyledButton>
                                )}
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
            <StyledButton onClick={goBackHandler}>Vissza</StyledButton>
        </CustomBox>
    );
}

export default GeneralNotifications;
