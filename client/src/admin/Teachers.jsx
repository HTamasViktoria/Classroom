import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import AdminTeacherList from "../components/Admin/AdminTeacherList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { AButton } from "../../StyledComponents";

function Teachers() {
    const navigate = useNavigate();

    const [allTeachers, setAllTeachers] = useState([]);

    useEffect(() => {
        fetch('/api/teachers')
            .then(response => response.json())
            .then(data => setAllTeachers(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);



    return (
        <>
            <AdminNavbar />
            <AButton
                onClick={() => navigate("/add-teacher")}
            >
                Tanár hozzáadása
            </AButton>

            <AdminTeacherList teachers={allTeachers} />
            <AButton
                onClick={()=>navigate("/admin")}
            >
                Vissza
            </AButton>
        </>
    );
}

export default Teachers;
