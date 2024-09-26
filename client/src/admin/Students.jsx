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


    const goBackHandler =()=>{
        navigate("/admin")
    }
    
    return(<>
        <AdminNavbar/>
        <Button
            variant="contained"
            sx={{
                backgroundColor: '#82b2b8',
                color: '#fff',
                '&:hover': {
                    backgroundColor: '#6e9ea4',
                },
            }}
            onClick={() => navigate("/add-student")}
        >
            Diák hozzáadása
        </Button>

        <AdminStudentList students={allStudents}/>
        <Button variant="contained"
                sx={{
                    backgroundColor: '#bacfb0',
                    color: '#fff',
                    '&:hover': {
                        backgroundColor: '#a8bfa1',
                    },
                }} onClick={goBackHandler}>Vissza</Button></>)
}

export default Students