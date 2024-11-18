import {Table, TableBody, TableCell, TableContainer, TableRow} from "@mui/material";
import {AButton, Cell, TableHeading} from "../../../StyledComponents.js";

function EditingMain({onEditingGrade, onEditing, onGoingBack, teacherId, gradesOfThisSubject, studentName, studentId, onRefreshing, subject}) {

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
                onRefreshing()
            })
            .catch(error => {
                console.error('Error:', error);
            });
    };



    const getCurrentTerm = () => {
        const now = new Date();
        const year = now.getFullYear();

        const firstTermStart = new Date(year, 8, 1); // September 1
        const firstTermEnd = new Date(year + 1, 1, 15); // February 15
        const secondTermStart = new Date(year, 1, 16); // February 16
        const secondTermEnd = new Date(year, 6, 31); // July 31

        if (now >= firstTermStart && now <= firstTermEnd) {
            return { startDate: firstTermStart, endDate: firstTermEnd };
        } else if (now >= secondTermStart && now <= secondTermEnd) {
            return { startDate: secondTermStart, endDate: secondTermEnd };
        } else {

            return null;
        }
    };

    const editHandler = (id) => {
        const gradeToEdit = gradesOfThisSubject.find((grade) => grade.id === id);
        const currentTerm = getCurrentTerm();

        if (currentTerm) {

            const gradeDate = new Date(gradeToEdit.date);
            const termStartDate = currentTerm.startDate;
            const termEndDate = currentTerm.endDate;

            if (gradeDate >= termStartDate && gradeDate <= termEndDate) {
                onEditingGrade(gradeToEdit);
                onEditing(true);
            } else {
                alert("A kiválasztott jegy nem ebbe a félévbe tartozik!");
            }
        } else {
            alert("Nem található aktuális félév!");
        }
    };
 
    
    return (<>
            <h1>{studentName} tanuló jegyei {subject} tantárgyból </h1>
            <TableContainer>
                <Table>
                    <TableHeading>
                        <TableRow>
                            <Cell>Dátum</Cell>
                            <Cell>Jegy értéke</Cell>
                            <Cell>Mire kapta</Cell>
                            <Cell>Műveletek</Cell>
                        </TableRow>
                    </TableHeading>
                    <TableBody>
                        {gradesOfThisSubject.map((grade, index) => <TableRow key={index}>
                            <TableCell>{grade.date.slice(0, -9)}</TableCell>
                            <TableCell>{grade.value}</TableCell>
                            <TableCell>{grade.forWhat}</TableCell>
                            <TableCell>
                                <AButton onClick={() => editHandler(grade.id)}>Szerkesztés</AButton>
                                <AButton onClick={() => deleteHandler(grade.id)}>Törlés</AButton>
                            </TableCell>
                        </TableRow>)}
                    </TableBody>
                </Table>
            </TableContainer>
            <AButton onClick={()=>onGoingBack()}>Vissza</AButton></>
    )
}


export default EditingMain