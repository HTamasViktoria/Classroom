import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import AdminStudentList from "../components/Admin/AdminStudentList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { AButton } from "../../StyledComponents";

function Students() {
    const navigate = useNavigate();

    const [allStudents, setAllStudents] = useState([]);

    useEffect(() => {
        fetch('/api/students')
            .then(response => response.json())
            .then(data => setAllStudents(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

 

    return (
        <>
            <AdminNavbar />
            <AButton
                onClick={() => navigate("/add-student")}
            >
                Diák hozzáadása
            </AButton>
      

            <AdminStudentList students={allStudents} />
            <AButton
                onClick={()=>navigate("/admin")}
            >
                Vissza
            </AButton>
        </>
    );
}

export default Students;
