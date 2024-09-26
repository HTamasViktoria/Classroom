import { useParams } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from 'react';
import { Card, Typography, Button, Stack } from '@mui/material';
import AddingTeacherSubject from "../components/Admin/AddingTeacherSubject.jsx";
import TeacherDetailed from "../components/Admin/TeacherDetailed.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";

function TeacherMain() {
    const navigate = useNavigate();    
    const { id } = useParams();
    
    const [teacher, setTeacher] = useState(null);
    const [subjects, setSubjects] = useState([]);
    const [error, setError] = useState(null);
    const [addingSubject, setAddingSubject] = useState(false);

    useEffect(() => {
        fetchTeacherData(id);
        fetchTeacherSubjects(id);
    }, [id]);

    const fetchTeacherData = async (teacherId) => {
        try {
            const response = await fetch(`/api/teachers/${teacherId}`);
            const data = await response.json();
            setTeacher(data);
        } catch (error) {
            handleFetchError(error);
        }
    };

    const fetchTeacherSubjects = async (teacherId) => {
        try {
            const response = await fetch(`/api/teacherSubjects/getByTeacherId/${teacherId}`);
            const data = await response.json();
            console.log(data);
            setSubjects(data);
        } catch (error) {
            handleFetchError(error);
        }
    };

    const handleFetchError = (error) => {
        console.error('Error fetching data:', error);
        setError(error);
    };

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

    const handleSuccessfulAdding = () => {
        fetchTeacherSubjects(id);
        setAddingSubject(false);
    };
    
   const goBackHandler=()=>{
    navigate("/admin/teachers")   
   }

    return (
        <><AdminNavbar/>
            <div>
                {!addingSubject ? (
                    <Card sx={{ maxWidth: 600, margin: '3em auto 2em auto', padding: 2 }}>
                        <TeacherDetailed teacher={teacher} subjects={subjects} />
                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#82b2b8',
                                color: '#fff',
                                '&:hover': {
                                    backgroundColor: '#6e9ea4',
                                },
                            }}
                            onClick={() => setAddingSubject(true)}
                        >
                            Tantárgy hozzáadása
                        </Button>
                    </Card>
                ) : (
                    <AddingTeacherSubject teacher={teacher} onSuccessfulAdding={handleSuccessfulAdding} />
                )}
            </div>
            <Stack spacing={2} sx={{ marginTop: 2, alignItems: 'center' }}>
                <Button onClick={goBackHandler}
                    variant="contained"
                    sx={{
                        backgroundColor: '#b0cfc5',
                        color: '#fff',
                        '&:hover': {
                            backgroundColor: '#b29f8f',
                        },
                    }}
                >
                    Vissza
                </Button>
            </Stack>
        </>
    );
}

export default TeacherMain;
