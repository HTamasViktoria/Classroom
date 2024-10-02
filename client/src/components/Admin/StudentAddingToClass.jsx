import { useEffect, useState } from 'react';
import { Stack, Typography, CircularProgress } from "@mui/material";
import { CustomBox } from '../../../StyledComponents';
import StudentSelector from "../Teacher/StudentSelector.jsx";
import {StyledButton} from '../../../StyledComponents';
import { useTheme } from '@mui/material/styles';

function StudentAddingToClass(props) {
    const theme = useTheme();
    const [students, setStudents] = useState([]);
    const [selectedStudentId, setSelectedStudentId] = useState('');
    const [loading, setLoading] = useState(true);

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
                setLoading(false);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
                setLoading(false);
            });
    }, []);

    const handleStudentChange = (e) => {
        setSelectedStudentId(e.target.value);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        
        if (!selectedStudentId) {
            alert("Kérjük, válasszon ki egy diákot.");
            return;
        }

        const addingStudentToClassData = {
            studentId: selectedStudentId,
            classId: props.classId
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
                console.log('Student added:', data);
                props.onSuccessfulAdding();
            })
            .catch(error => console.error('Error adding student:', error));
    };

    return (
        <CustomBox>
            <Typography variant="h6" sx={{ marginBottom: 2, color: theme.palette.tertiary.main }}>
                Diák hozzáadása az {props.className} osztályhoz
            </Typography>
            {loading ? (
                <CircularProgress />
            ) : (
                <form noValidate onSubmit={handleSubmit}>
                    <Stack spacing={2} width={400}>
                        <StudentSelector
                            selectedStudentId={selectedStudentId}
                            students={students}
                            handleStudentChange={handleStudentChange}
                        />
                        <StyledButton
                            type='submit'
                            variant='contained'
                        >
                            Hozzáad
                        </StyledButton>
                    </Stack>
                </form>
            )}
        </CustomBox>
    );
}

export default StudentAddingToClass;
