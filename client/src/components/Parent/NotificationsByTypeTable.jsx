import {AButton, BButton, Cell, CustomBox, StyledHeading, TableHeading} from "../../../StyledComponents.js";
import {Table, TableBody, TableCell, TableRow} from "@mui/material";
import React from "react";
import SetToReadButton from "./SetToReadButton.jsx";
import NotifDeleteButton from "./NotifDeleteButton.jsx";
import NotifDuoButton from "./NotifDuoButton.jsx";

function NotificationsByTypeTable({notifications, onRefresh, onGoBack}){
    
    
    return(<> 
        <CustomBox>
        <StyledHeading>Értesítések</StyledHeading>
        <Table>
            <TableHeading>
                <TableRow>
                    <Cell>Státusz</Cell>
                    <Cell>Dátum</Cell>
                    <Cell>Határidő</Cell>
                    <Cell>Tantárgy</Cell>
                    <Cell>Tanár</Cell>
                    <Cell>Leírás</Cell>
                    <Cell>További leírás</Cell>
                    <Cell>Opciók</Cell>
                </TableRow>
            </TableHeading>
            <TableBody>
                {notifications.map(notification => (
                    <TableRow key={`id:${notification.id}`}>
                        <TableCell>{notification.read === true ? "Olvasott" : "Olvasatlan"}</TableCell>
                        <TableCell>{new Date(notification.date).toISOString().split('T')[0]}</TableCell>
                        <TableCell>{new Date(notification.dueDate).toISOString().split('T')[0]}</TableCell>
                        <TableCell>{notification.subjectName}</TableCell>
                        <TableCell>{notification.teacherName}</TableCell>
                        <TableCell>{notification.description}</TableCell>
                        <TableCell>{notification.optionalDescription}</TableCell>

                        <TableCell>
                            {notification.read === false && (
                                <SetToReadButton notification={notification} onRefresh={onRefresh}/>
                               
                            )}
                            <NotifDuoButton notification={notification} onRefresh={onRefresh} onGoBack={onGoBack}/>
                                                      
                            
                            {notification.read && (
                                <NotifDeleteButton notification={notification} onRefresh={onRefresh}/>
                               
                            )}
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
        <BButton onClick={() => onGoBack()}>Vissza</BButton>
    </CustomBox></>)
}


export default NotificationsByTypeTable