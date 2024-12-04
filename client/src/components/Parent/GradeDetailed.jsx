import { useEffect } from "react";
import { TableContainer, Paper, Table, TableHead, TableBody, TableCell, TableRow } from "@mui/material";
import { NotifContainer, NotifHead } from "../../../StyledComponents.js";

function GradeDetailed({ grade, onButtonClick, onRefreshNeeded }) {



    useEffect(() => {
        if (grade && grade.id) {
            fetch(`/api/grades/officiallyread/${grade.id}`, {
                method: 'POST',
            })
                .then(response => response.json())
                .then(data => {
                    onRefreshNeeded();
                })
                .catch(error => console.error(error));
        }
    }, [grade, onRefreshNeeded]);



    if (!grade) {
        return <div>Loading...</div>;
    }

    return (
        <NotifContainer>
            <NotifHead>
                Új osztályzat
            </NotifHead>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Dátum</TableCell>
                            <TableCell>Tantárgy</TableCell>
                            <TableCell>Mire kapta</TableCell>
                            <TableCell>Tanár</TableCell>
                            <TableCell>Értéke</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <TableRow key={grade.id}>
                            <TableCell>{new Date(grade.date).toLocaleDateString()}</TableCell>
                            <TableCell>{grade.subject}</TableCell>
                            <TableCell>{grade.forWhat}</TableCell>
                            <TableCell>{grade.teacherId}</TableCell>
                            <TableCell>{grade.value}</TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
            <button onClick={onButtonClick}>Vissza</button>
        </NotifContainer>
    );
}

export default GradeDetailed;
