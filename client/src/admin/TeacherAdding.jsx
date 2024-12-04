import React from 'react';
import { useNavigate } from "react-router-dom";
import TeacherAddingForm from "../components/Admin/TeacherAddingForm.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";

function TeacherAdding() {
    const navigate = useNavigate();

    const postTeacher = (teacher) => {
        return fetch(`/api/auth/signup/teacher`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(teacher)
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


    const handleCreateTeacher = (teacher) => {
        postTeacher(teacher)
            .then(() => navigate("/admin/teachers"))
            .catch(error => console.error('Error:', error));
    };

 

    return (
        <><AdminNavbar/>
            <TeacherAddingForm onSave={handleCreateTeacher} onCancel={()=>navigate("/admin")}/>
        </>
    );
}

export default TeacherAdding;
