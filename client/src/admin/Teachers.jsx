import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import AdminTeacherList from "../components/Admin/AdminTeacherList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { StyledButton } from "../../StyledComponents";

function Teachers() {
    const navigate = useNavigate();

    const [allTeachers, setAllTeachers] = useState([]);

    useEffect(() => {
        fetch('/api/teachers')
            .then(response => response.json())
            .then(data => setAllTeachers(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    const goBackHandler = () => {
        navigate("/admin");
    }

    return (
        <>
            <AdminNavbar />
            <StyledButton
                onClick={() => navigate("/add-teacher")}
            >
                Tanár hozzáadása
            </StyledButton>

            <AdminTeacherList teachers={allTeachers} />
            <StyledButton
                onClick={goBackHandler}
            >
                Vissza
            </StyledButton>
        </>
    );
}

export default Teachers;
