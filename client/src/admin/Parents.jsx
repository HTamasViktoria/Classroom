import React, { useState, useEffect } from "react";
import { Typography, TableBody, TableCell } from "@mui/material";
import { CustomBox, Cell, TableHeading, StyledHeading, PaperTableContainer, StyledTable, StyledTableRow } from '../../StyledComponents';
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";

function Parents() {
    const [parents, setParents] = useState([]);

    useEffect(() => {
        fetch(`/api/parents`)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setParents(data);
            })
            .catch(error => console.error(error));
    }, []);

    return (
        <>
            <AdminNavbar />
            <CustomBox>
                <StyledHeading>Szülők</StyledHeading>
                <PaperTableContainer>
                    <StyledTable aria-label="students table">
                        <TableHeading>
                            <StyledTableRow>
                                {['Családnév', 'Keresztnév', 'Gyermek neve', 'Email-cím'].map((header) => (
                                    <Cell key={header}>{header}</Cell>
                                ))}
                            </StyledTableRow>
                        </TableHeading>
                        <TableBody>
                            {parents.map((parent) => (
                                <StyledTableRow key={parent.id}>
                                    <TableCell>{parent.familyName}</TableCell>
                                    <TableCell>{parent.firstName}</TableCell>
                                    <TableCell>{parent.childName}</TableCell>
                                    <TableCell>{parent.email}</TableCell>
                                </StyledTableRow>
                            ))}
                        </TableBody>
                    </StyledTable>
                </PaperTableContainer>
            </CustomBox>
        </>
    );
}

export default Parents;
