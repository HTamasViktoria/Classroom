import { useEffect, useState } from 'react';
import { CustomBox } from '../../../StyledComponents';
import SelectFromAllStudents from "./SelectFromAllStudents.jsx";
import { AButton, TableHeading, CenteredStack } from '../../../StyledComponents';

function StudentAddingToClass({ classId, className, onSuccessfulAdding }) {

    const [students, setStudents] = useState([]);
    const [selectedStudentId, setSelectedStudentId] = useState('');

    useEffect(() => {
        fetch('/api/students')
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                setStudents(data);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();

        if (!selectedStudentId) {
            alert("Kérjük, válasszon ki egy diákot.");
            return;
        }

        const addingStudentToClassData = {
            studentId: selectedStudentId,
            classId: classId
        };

        fetch('/api/classes/addStudent', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(addingStudentToClassData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                onSuccessfulAdding();
            })
            .catch(error => console.error('Error adding student:', error));
    };

    return (
        <CustomBox>
            <TableHeading>
                Diák hozzáadása az {className} osztályhoz
            </TableHeading>
            <form noValidate onSubmit={handleSubmit}>
                <CenteredStack>
                    <SelectFromAllStudents
                        selectedStudentId={selectedStudentId}
                        handleStudentChange={(e) => setSelectedStudentId(e.target.value)} // Átadjuk a változó frissítésére szolgáló funkciót
                    />
                    <AButton
                        type='submit'
                        variant='contained'
                    >
                        Hozzáad
                    </AButton>
                </CenteredStack>
            </form>
        </CustomBox>
    );
}

export default StudentAddingToClass;
