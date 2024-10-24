import React from 'react';
import { useNavigate } from "react-router-dom";
import TeacherAddingForm from "../components/Admin/TeacherAddingForm.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";

function TeacherAdding() {
    const navigate = useNavigate();

    const postTeacher = (teacher) => {
        return fetch(`/api/auth/sign-up/teacher`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(teacher)
        })
            .then((res) => {
                if (!res.ok) {
                    throw new Error(`HTTP error! status: ${res.status}`);
                }
                return res.text();
            })
            .then((text) => {
                console.log('Server response:', text);
                return text;
            })
            .catch((error) => {
                console.error('Error:', error);
                throw error;
            });
    };


    const handleCreateTeacher = (teacher) => {
        postTeacher(teacher)
            .then(() => navigate("/admin/teachers"))
            .catch(error => console.error('Error:', error));
    };

    const props = {
        onSave: handleCreateTeacher,
        onCancel: () => navigate("/admin")
    };

    return (
        <><AdminNavbar/>
            <TeacherAddingForm {...props}/>
        </>
    );
}

export default TeacherAdding;
