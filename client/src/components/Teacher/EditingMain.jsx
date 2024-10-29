import {Table, TableBody, TableCell, TableContainer, TableRow} from "@mui/material";
import {StyledButton, StyledTableCell, StyledTableHead} from "../../../StyledComponents.js";

function EditingMain({onEditChosen, onGoingBack, teacherId, grades, studentName, studentId, onRefreshing, subject}) {


   const editHandler=(id)=>{
       onEditChosen(id)
   }

    const deleteHandler = (id) => {
        fetch(`/api/grades/delete/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
 
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log(data.message);
                onRefreshing()
            })
            .catch(error => {
                console.error('Error:', error);
            });
    };



    const goBackHandler=()=>{
       onGoingBack()
 }
    
    
    return (<><h1>{studentName} tanuló jegyei {subject} tantárgyból </h1>
            <TableContainer>
                <Table>
                    <StyledTableHead>
                        <TableRow>
                            <StyledTableCell>Dátum</StyledTableCell>
                            <StyledTableCell>Jegy értéke</StyledTableCell>
                            <StyledTableCell>Mire kapta</StyledTableCell>
                            <StyledTableCell>Műveletek</StyledTableCell>
                        </TableRow>
                    </StyledTableHead>
                    <TableBody>
                        {grades.map((grade, index) => <TableRow key={index}>
                            <TableCell>{grade.date.slice(0, -9)}</TableCell>
                            <TableCell>{grade.value}</TableCell>
                            <TableCell>{grade.forWhat}</TableCell>
                            <TableCell>
                                <StyledButton onClick={() => editHandler(grade.id)}>Szerkesztés</StyledButton>
                                <StyledButton onClick={() => deleteHandler(grade.id)}>Törlés</StyledButton>
                            </TableCell>
                        </TableRow>)}
                    </TableBody>
                </Table>
            </TableContainer>
            <StyledButton onClick={goBackHandler}>Vissza</StyledButton></>
    )
}


export default EditingMain