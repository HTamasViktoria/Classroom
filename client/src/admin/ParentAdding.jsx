import React from 'react';
import { useNavigate } from "react-router-dom";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import ParentAddingForm from "../components/Admin/ParentAddingForm.jsx";
import { AButton } from "../../StyledComponents";
import { useParams } from 'react-router-dom';

function ParentAdding() {
    const navigate = useNavigate();
    const { id } = useParams();

    const postParent = (parent) => {
        console.log('Posting parent data:', parent, JSON.stringify(parent, null, 2));
        return fetch(`/api/auth/sign-up/parent`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(parent)
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
                alert("Szülő sikeresen hozzáadva");
                return data;
            })
            .catch((error) => {
                console.error('Error:', error);
                throw error;
            });
    };

    const handleCreateParent = (parent) => {
        postParent(parent)
            .then(() => navigate("/admin/parents"))
            .catch(error => console.error('Error:', error));
    };

  

    const goBackHandler = () => {
        navigate("/admin/parents");
    };

    return (
        <>
            <AdminNavbar />
            <ParentAddingForm onSave={handleCreateParent} onCancel={()=>navigate("/admin")} studentId={id}/>
            <AButton onClick={goBackHandler}>
                Vissza
            </AButton>
        </>
    );
}
    
  

export default ParentAdding;
