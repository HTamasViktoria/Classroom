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
                backgroundColor: '#82b2b8',
                color: '#fff',
                '&:hover': {
                    backgroundColor: '#6e9ea4',
                },
            }}
            onClick={() => navigate("/add-teacher")}
        >
            Tanár hozzáadása
        </Button>
        
        <AdminTeacherList teachers={allTeachers}/>
        <Button variant="contained"
                sx={{
                    backgroundColor: '#a2c4c6',
                    color: '#fff',
                    '&:hover': {
                        backgroundColor: '#8ab2b5',
                    },
                }} onClick={goBackHandler}>Vissza</Button>
    </>)
}

export default Teachers