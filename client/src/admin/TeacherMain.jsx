import { useParams } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from 'react';
import { Card, Typography, Stack } from '@mui/material';
import AddingTeacherSubject from "../components/Admin/AddingTeacherSubject.jsx";
import TeacherDetailed from "../components/Admin/TeacherDetailed.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { StyledButton } from "../../StyledComponents";

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
        return (
            <Typography variant="body1" color="error" align="center">
                Hiba történt az adatok betöltése közben.
            </Typography>
        );
    }

    if (!teacher) {
        return (
            <Typography variant="body1" align="center">
                Nincs elérhető adat.
            </Typography>
        );
    }

    const handleSuccessfulAdding = () => {
        fetchTeacherSubjects(id);
        setAddingSubject(false);
    };

    const goBackHandler = () => {
        navigate("/admin/teachers");
    }

    return (
        <>
            <AdminNavbar />
            <div>
                {!addingSubject ? (
                    <Card sx={{ maxWidth: 600, margin: '3em auto 2em auto', padding: 2 }}>
                        <TeacherDetailed teacher={teacher} subjects={subjects} />
                        <StyledButton
                            onClick={() => setAddingSubject(true)}
                        >
                            Tantárgy hozzáadása
                        </StyledButton>
                    </Card>
                ) : (
                    <AddingTeacherSubject teacher={teacher} onSuccessfulAdding={handleSuccessfulAdding} />
                )}
            </div>
            <Stack spacing={2} sx={{ marginTop: 2, alignItems: 'center' }}>
                <StyledButton
                    onClick={goBackHandler}
                >
                    Vissza
                </StyledButton>
            </Stack>
        </>
    );
}

export default TeacherMain;
