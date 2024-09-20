import { useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import { Card, CardContent, Typography, List, ListItem, ListItemText, Button } from '@mui/material';
import AddingTeacherSubjectForm from "../components/Admin/AddingTeacherSubjectForm.jsx";

function Teacher() {
    const { id } = useParams();
    const [teacher, setTeacher] = useState(null);
    const [subjects, setSubjects] = useState([]);
    const [error, setError] = useState(null);
    const [addingSubject, setAddingSubject] = useState(false);

    useEffect(() => {
        
        fetch(`/api/teachers/${id}`)
            .then(response => response.json())
            .then(data => {
                setTeacher(data);
            })
            .catch(error => {
                console.error('Error fetching teacher data:', error);
                setError(error);
            });

        
        fetch(`/api/teacherSubjects/getByTeacherId/${id}`)
            .then(response => response.json())
            .then(data => {
                console.log(data)
                setSubjects(data);
            })
            .catch(error => {
                console.error('Error fetching teacher subjects:', error);
                setError(error);
            });
    }, [id]);

    if (error) {
        return <Typography variant="body1" color="error" align="center">
            Hiba történt az adatok betöltése közben.
        </Typography>;
    }

    if (!teacher) {
        return <Typography variant="body1" align="center">
            Nincs elérhető adat.
        </Typography>;
    }

    return (
        <div>
            {!addingSubject ? (
                <Card sx={{ maxWidth: 600, margin: 'auto', padding: 2 }}>
                    <CardContent>
                        <Typography variant="h5" component="div" gutterBottom>
                            {teacher.firstName} {teacher.familyName}
                        </Typography>
                        <Typography variant="subtitle1" color="text.secondary" gutterBottom>
                            Tanár ID: {teacher.id}
                        </Typography>
                        <Typography variant="h6" component="div" gutterBottom>
                            Tantárgyai:
                        </Typography>
                        {subjects && subjects.length > 0 ? (
                            <List>
                                {subjects.map((subject) => (
                                    <ListItem key={subject.id}>
                                        <ListItemText primary={subject.subject} />
                                    </ListItem>
                                ))}
                            </List>
                        ) : (
                            <Typography variant="body2" color="text.secondary">
                                Nincsenek tantárgyai.
                            </Typography>
                        )}
                    </CardContent>
                    <Button onClick={() => setAddingSubject(true)} variant="contained" color="primary">
                        Tantárgy hozzáadása
                    </Button>
                </Card>
            ) : (
                <AddingTeacherSubjectForm teacher={teacher} />
            )}
        </div>
    );
}

export default Teacher;
