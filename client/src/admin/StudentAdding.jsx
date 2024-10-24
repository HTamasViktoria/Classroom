import React from 'react';
import { useNavigate } from "react-router-dom";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import StudentAddingForm from "../components/Admin/StudentAddingForm.jsx";
import { StyledButton } from "../../StyledComponents";

function StudentAdding() {
    const navigate = useNavigate();

    const postStudent = (student) => {
        console.log('Posting student data:', JSON.stringify(student, null, 2));
        return fetch(`/api/auth/sign-up/student`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(student)
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

    const handleCreateStudent = (student) => {
        postStudent(student)
            .then(() => navigate("/admin/students"))
            .catch(error => console.error('Error:', error));
    };

    const props = {
        onSave: handleCreateStudent,
        onCancel: () => navigate("/admin")
    };

    const goBackHandler = () => {
        navigate("/admin/students");
    };

    return (
        <>
            <AdminNavbar />
            <StudentAddingForm {...props} />
            <StyledButton onClick={goBackHandler}>
                Vissza
            </StyledButton>
        </>
    );
}

export default StudentAdding;
