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
            onClick={() => navigate("/add-teacher")}
        >
            Tanár hozzáadása
        </Button>
        
        <AdminTeacherList teachers={allTeachers}/>
    </>)
}

export default Teachers