import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import AdminStudentList from "../components/Admin/AdminStudentList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { StyledButton } from "../../StyledComponents";

function Students() {
    const navigate = useNavigate();

    const [allStudents, setAllStudents] = useState([]);

    useEffect(() => {
        fetch('/api/students')
            .then(response => response.json())
            .then(data => setAllStudents(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    const goBackHandler = () => {
        navigate("/admin");
    }

    return (
        <>
            <AdminNavbar />
            <StyledButton
                onClick={() => navigate("/add-student")}
            >
                Diák hozzáadása
            </StyledButton>
            <StyledButton
                onClick={() => navigate("/add-parent")}
            >
                Szülő hozzáadása
            </StyledButton>

            <AdminStudentList students={allStudents} />
            <StyledButton
                onClick={goBackHandler}
            >
                Vissza
            </StyledButton>
        </>
    );
}

export default Students;
