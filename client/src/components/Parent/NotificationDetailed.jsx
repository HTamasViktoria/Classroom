import {
    Box,
    Paper,
    Table,
    TableBody,
    TableRow,
    TableCell,
    Typography,
    TableContainer,
    useTheme
} from "@mui/material";
import { StyledTableHead, StyledTableCell, StyledButton, StyledSecondaryButton } from '../../../StyledComponents';

function NotificationDetailed({ notification, onButtonClick, onRefreshNeeded }) {
    const theme = useTheme();
    
  

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

    const goBackHandler = () => {
        onButtonClick();
    }


    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                {notification.type}
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="notification table">
                    <StyledTableHead>
                        <TableRow>
                            <StyledTableCell>Dátum</StyledTableCell>
                            <StyledTableCell>Határidő</StyledTableCell>
                            <StyledTableCell>Tantárgy</StyledTableCell>
                            <StyledTableCell>Tanár</StyledTableCell>
                            <StyledTableCell>Leírás</StyledTableCell>
                            <StyledTableCell>További leírás</StyledTableCell>
                        </TableRow>
                    </StyledTableHead>
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
                    <StyledButton
                        onClick={setToReadHandler}
                        id={notification.id}
                        variant="contained"
                        sx={{
                            flex: 1,
                            marginRight: 1
                        }}>
                        Ok, elolvastam
                    </StyledButton>
                )}
                <StyledSecondaryButton
                    onClick={goBackHandler}
                    id={notification.id}
                    variant="contained"
                    sx={{
                        flex: 1,
                        marginRight: 1
                    }}>
                    Erre még visszatérek
                </StyledSecondaryButton>

                {notification.read && (
                    <StyledButton
                        id={notification.id}
                        onClick={deleteHandler}
                        variant="contained"
                        sx={{
                            flex: 1,
                            marginLeft: 1
                        }}>
                        Értesítés törlése
                    </StyledButton>
                )}
            </Box>
        </Box>
    );
}

export default NotificationDetailed;
