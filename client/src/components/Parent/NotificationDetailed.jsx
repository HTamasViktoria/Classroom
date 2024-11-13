import {
    Paper,
    TableBody,
    TableRow,
    TableCell,
    TableContainer,
} from "@mui/material";
import {
    TableHeading,
    Cell,
    NotifContainer, NotifHead, NotifTable,
} from '../../../StyledComponents';
import NotifButtonContainer from "./NotifButtonContainer.jsx";
import {useEffect} from "react";

function NotificationDetailed({ notification, onButtonClick, onRefreshNeeded }) {



    useEffect(() => {
        fetch(`/api/notifications/setToOfficiallyRead/${notification.id}`, {
            method: 'POST',
        })
            .then(response => response.json())
            .catch(error => console.error(error));
    }, [notification]);




    return (
        <NotifContainer>
            <NotifHead>
                {notification.type}
            </NotifHead>
            <TableContainer component={Paper}>
                <NotifTable>
                    <TableHeading>
                        <TableRow>
                            <Cell>Dátum</Cell>
                            <Cell>Határidő</Cell>
                            <Cell>Tantárgy</Cell>
                            <Cell>Tanár</Cell>
                            <Cell>Leírás</Cell>
                            <Cell>További leírás</Cell>
                        </TableRow>
                    </TableHeading>
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
                </NotifTable>
            </TableContainer>
            
<NotifButtonContainer notification={notification} onGoBack={()=>onButtonClick()} onRefresh={onRefreshNeeded}/>
            
        </NotifContainer>
    );
}

export default NotificationDetailed;