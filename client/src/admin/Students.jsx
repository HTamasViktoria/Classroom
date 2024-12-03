import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import AdminStudentList from "../components/Admin/AdminStudentList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { AButton } from "../../StyledComponents";


function Students() {
    const navigate = useNavigate();
    const [allStudents, setAllStudents] = useState([]);
    const [token, setToken] = useState(localStorage.getItem('token'))
    

    
    
    useEffect(() => {
        const fetchStudents = async () => {
            try {
                const response = await fetch('/api/students', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    }
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setAllStudents(data);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchStudents();
    }, [token]);

    return (
        <>
            <AdminNavbar />
            <AButton onClick={() => navigate("/add-student")}>
                Diák hozzáadása
            </AButton>

            <AdminStudentList students={allStudents} />
            <AButton onClick={() => navigate("/admin")}>
                Vissza
            </AButton>
        </>
    );
}

export default Students;
