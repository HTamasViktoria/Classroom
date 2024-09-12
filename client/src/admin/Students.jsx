import { useNavigate } from "react-router-dom";
import {useEffect, useState} from "react";
import Button from '@mui/material/Button';
import AdminStudentList from "../components/Admin/AdminStudentList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import AdminTeacherList from "../components/Admin/AdminTeacherList.jsx";

function Students(){

    const navigate = useNavigate();

    const [allStudents, setAllStudents] = useState([]);

    useEffect(() => {

        fetch('/api/students')
            .then(response => response.json())
            .then(data => setAllStudents(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);
    
    return(<>
        <AdminNavbar/>
        <Button
            variant="contained"
            sx={{
                backgroundColor: '#b5a58d',
                color: '#fff',
                '&:hover': {
                    backgroundColor: '#b5a58d',
                },
            }}
            onClick={() => navigate("/add-student")}
        >
            Diák hozzáadása
        </Button>

        <AdminStudentList students={allStudents}/></>)
}

export default Students