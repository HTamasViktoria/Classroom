import {useParams} from 'react-router-dom';
import React, {useState, useEffect} from 'react';
import {TableBody,TableCell} from "@mui/material";
import {CustomBox, AButton,TableHeading, StyledHeading, StyledTableContainer, StyledTableRow, Cell, StyledTable} from '../../../StyledComponents';
import {useNavigate} from "react-router-dom";

function ParentsOfAStudent() {
    const {id} = useParams();
    const navigate = useNavigate();

    const [parents, setParents] = useState([]);

    useEffect(() => {
        fetch(`/api/getbyStudentId/${id}`)
            .then(response => response.json())
            .then(data => {
                setParents(data);
            })
            .catch(error => console.error(error));
    }, [id]);


    return (
        <CustomBox>
            <AButton onClick={() => navigate(`/admin/parents/add-parent/${id}`)}>Szülő hozzáadása</AButton>
            <StyledHeading>
                {parents[0]?.childName} szülei
            </StyledHeading>
            {parents.length === 0 && (<div>Ennek a diáknak nincs még hozzáadott szülője</div>)}
            <StyledTableContainer>
                <StyledTable>
                    <TableHeading>
                        <StyledTableRow>
                            {['Családnév', 'Keresztnév'].map((header) => (
                                <Cell key={header}>
                                    {header}
                                </Cell>
                            ))}
                        </StyledTableRow>
                    </TableHeading>
                    <TableBody>
                        {parents.map((parent) => (
                            <StyledTableRow key={parent.id}>
                                <TableCell>{parent.familyName}</TableCell>
                                <TableCell>{parent.firstName}</TableCell>
                            </StyledTableRow>
                        ))}
                    </TableBody>
                </StyledTable>
            </StyledTableContainer>
            <AButton onClick={() => navigate('/admin/students')}>Vissza</AButton>
        </CustomBox>
    );
}

export default ParentsOfAStudent;
