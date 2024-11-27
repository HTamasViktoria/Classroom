import { useParams } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import { StyledErrorMessage, StyledNoDataMessage } from "../../StyledComponents";
import AddingTeacherSubject from "../components/Admin/AddingTeacherSubject.jsx";
import TeacherDetailed from "../components/Admin/TeacherDetailed.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { AButton, StyledCard, CenteredStack } from "../../StyledComponents";

function TeacherMain() {

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
            <StyledErrorMessage>
                Hiba történt az adatok betöltése közben.
            </StyledErrorMessage>
        );
    }

    if (!teacher) {
        return (
            <StyledNoDataMessage>
                Nincs elérhető adat.
            </StyledNoDataMessage>
        );
    }

    const handleSuccessfulAdding = () => {
        fetchTeacherSubjects(id);
        setAddingSubject(false);
    };

    return (
        <>
            <AdminNavbar />
            <div>
                {!addingSubject ? (
                    <StyledCard>
                        <TeacherDetailed teacher={teacher} subjects={subjects} />
                        <AButton onClick={() => setAddingSubject(true)}>
                            Tantárgy hozzáadása
                        </AButton>
                    </StyledCard>
                ) : (
                    <AddingTeacherSubject teacher={teacher} onSuccessfulAdding={handleSuccessfulAdding} />
                )}
            </div>
            <CenteredStack spacing={2}>
            </CenteredStack>
        </>
    );
}

export default TeacherMain;
