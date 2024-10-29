import React, {useState, useEffect} from "react";
import { Box, Typography, Table, TableBody, TableContainer, TableHead, TableRow, Paper, TableCell } from "@mui/material";
import { CustomBox, StyledTableCell, StyledTableHead } from '../../StyledComponents';
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
function Parents(){

    const [parents, setParents] = useState([])
    
    useEffect(()=>{
        fetch(`/api/parents`)
            .then(response=>response.json())
            .then(data=> {
                console.log(data)
                setParents(data)
            })
            .catch(error=>console.error(error))
    },[])
    
    return(<><AdminNavbar/>
        <CustomBox>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                Szülők
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="students table">
                    <StyledTableHead>
                        <TableRow>
                            {[ 'Családnév', 'Keresztnév', 'Gyermek neve', 'Email-cím' ].map((header) => (
                                <StyledTableCell key={header}>
                                    {header}
                                </StyledTableCell>
                            ))}
                        </TableRow>
                    </StyledTableHead>
                    <TableBody>
                        {parents.map((parent) => (
                            <TableRow key={parent.id}>
                                <TableCell>{parent.familyName}</TableCell>
                                <TableCell>{parent.firstName}</TableCell>
                                <TableCell>{parent.childName}</TableCell>
                                <TableCell>{parent.email}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </CustomBox>
    
    </>)
}

export default Parents