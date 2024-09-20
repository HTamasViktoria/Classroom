import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";
import { useEffect, useState } from "react";
import { Select, MenuItem, FormControl, InputLabel, Typography, Container, Box } from '@mui/material';
import GradeAddingForm from "../components/Teacher/GradeAddingForm.jsx";
import Tasks from "../components/Teacher/Tasks.jsx";
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
                <Box my={4}>
                    <Typography variant="h4" gutterBottom>
                        {selectedTeacher ? '' : 'Én vagyok:'}
                    </Typography>
                    {selectedTeacher ? (
                        <Tasks teacherId={selectedTeacherId} teacherName={`${selectedTeacher.familyName} ${selectedTeacher.firstName}`} />


                    ) : (
                        <FormControl fullWidth variant="outlined">
                            <InputLabel id="teacher-select-label">Tanár kiválasztása:</InputLabel>
                            <Select
                                labelId="teacher-select-label"
                                value={selectedTeacherId}
                                onChange={handleChange}
                                label="Select Teacher"
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
                            </Select>
                        </FormControl>
                    )}
                </Box>
            </Container>
        </>
    );
}

export default Starter;
