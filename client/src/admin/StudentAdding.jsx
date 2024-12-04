import React from 'react';
import { useNavigate } from "react-router-dom";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import StudentAddingForm from "../components/Admin/StudentAddingForm.jsx";
import { AButton } from "../../StyledComponents";

function StudentAdding() {
    const navigate = useNavigate();

    const postStudent = (student) => {
        console.log('Posting student data:', JSON.stringify(student, null, 2));

        return fetch(`/api/auth/signup/student`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(student)
        })
            .then((res) => {
                if (!res.ok) {
                    return res.json().then((json) => {
                        const errorMessages = json.RegistrationError || [];

                        if (errorMessages.length > 0) {
                  
                            alert(`Hiba történt: ${errorMessages.join(", ")}`);
                        } else {
                            alert("Ismeretlen hiba történt.");
                        }

                        throw new Error(errorMessages.join(", ") || "Ismeretlen hiba történt.");
                    });
                }

                return res.json();
            })
            .then((data) => {
                console.log('Server response:', data);
                return data;
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


  

    return (
        <>
            <AdminNavbar />
            <StudentAddingForm onSave={handleCreateStudent} onCancel={()=>navigate("/admin")}/>
            <AButton onClick={()=> navigate("/admin/students")}>
                Vissza
            </AButton>
        </>
    );
}

export default StudentAdding;
