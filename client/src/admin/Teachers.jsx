import { useNavigate } from "react-router-dom";
import {useEffect, useState} from "react";
import Button from '@mui/material/Button';
import AdminTeacherList from "../components/Admin/AdminTeacherList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";

function Teachers(){
    const navigate = useNavigate();

    const [allTeachers, setAllTeachers] = useState([]);

    useEffect(() => {

        fetch('/api/teachers')
            .then(response => response.json())
            .then(data => setAllTeachers(data))
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
                backgroundColor: '#b29a88',
                color: '#fff',
                '&:hover': {
                    backgroundColor: '#a0887a',
                },
            }}
            onClick={() => navigate("/add-teacher")}
        >
            Tanár hozzáadása
        </Button>
        
        <AdminTeacherList teachers={allTeachers}/>
        <Button variant="contained"
                sx={{
                    backgroundColor: '#bacfb0',
                    color: '#fff',
                    '&:hover': {
                        backgroundColor: '#a8bfa1',
                    },
                }} onClick={goBackHandler}>Vissza</Button>
    </>)
}

export default Teachers