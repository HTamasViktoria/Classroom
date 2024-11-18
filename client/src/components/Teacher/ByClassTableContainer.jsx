import {Table, TableBody, TableContainer, TableRow} from "@mui/material";
import {AButton, Cell, TableHeading} from "../../../StyledComponents.js";
import React from "react";

function ByClassTableContainer({classes, onChosenClass, averages}){


    return(
        <TableContainer>
            <Table>
                <TableHeading>
                    <TableRow>
                        <Cell>Osztály</Cell>
                        <Cell>Átlag</Cell>
                        <Cell>Műveletek</Cell>
                    </TableRow>
                </TableHeading>
                <TableBody>
                    {classes.map((classItem) => (
                        <TableRow key={classItem.id}>
                            <Cell>{classItem.name}</Cell>
                            <Cell>{averages[classItem.name]}</Cell>
                            <Cell>
                                <AButton id={classItem.id} 
                                         onClick={(e)=>onChosenClass(classItem.id)} >
                                    Részletek
                                </AButton>
                            </Cell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}

export default ByClassTableContainer