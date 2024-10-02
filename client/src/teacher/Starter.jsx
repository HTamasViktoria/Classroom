import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";
import { useEffect, useState } from "react";
import { Container, MenuItem } from '@mui/material';
import Tasks from "../components/Teacher/Tasks.jsx";
import { StyledSelect, StyledFormControl, StyledInputLabel, StyledTypography } from '../../StyledComponents';

function Starter() {
    const [teachers, setTeachers] = useState([]);
    const [selectedTeacherId, setSelectedTeacherId] = useState('');
    const [selectedTeacher, setSelectedTeacher] = useState(null);

    useEffect(() => {
        fetch('/api/teachers')
            .then(response => response.json())
            .then(data => {
                setTeachers(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    useEffect(() => {
        const teacher = teachers.find(teacher => teacher.id === selectedTeacherId);
        setSelectedTeacher(teacher);
    }, [selectedTeacherId, teachers]);

    const handleChange = (event) => {
        setSelectedTeacherId(event.target.value);
    };

    return (
        <>
            <TeacherNavbar teacher={selectedTeacher} />
            <Container>
                <StyledTypography variant="h4" gutterBottom>
                    {selectedTeacher ? '' : 'Én vagyok:'}
                </StyledTypography>
                {selectedTeacher && (
                    <Tasks teacherId={selectedTeacherId} teacherName={`${selectedTeacher.familyName} ${selectedTeacher.firstName}`} />
                )}
                {!selectedTeacher && (
                    <StyledFormControl variant="outlined">
                        <StyledInputLabel id="teacher-select-label">Tanár kiválasztása:</StyledInputLabel>
                        <StyledSelect
                            labelId="teacher-select-label"
                            value={selectedTeacherId}
                            onChange={handleChange}
                            label="Tanár kiválasztása:"
                        >
                            {teachers.length > 0 ? (
                                teachers.map((teacher) => (
                                    <MenuItem key={teacher.id} value={teacher.id}>
                                        {teacher.familyName} {teacher.firstName}
                                    </MenuItem>
                                ))
                            ) : (
                                <MenuItem disabled>No teachers available</MenuItem>
                            )}
                        </StyledSelect>
                    </StyledFormControl>
                )}
            </Container>
        </>
    );
}

export default Starter;
