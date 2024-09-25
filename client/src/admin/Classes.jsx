import React, {useState, useEffect} from "react";
import AdminClassList from "../components/Admin/AdminClassList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import Button from "@mui/material/Button";
import StudentAddingToClass from "../components/Admin/StudentAddingToClass.jsx";
import {useNavigate} from "react-router-dom";
import StudentsOfClass from "../components/Admin/StudentsOfClass.jsx";


function Classes() {

    const navigate = useNavigate();

    const [classes, setClasses] = useState([]);
    const [classId, setClassId] = useState(null)
    const [className, setClassName] = useState("")
    const [addingOrViewing, setAddingOrViewing] = useState("")

    useEffect(() => {
        fetch('/api/classes')
            .then(response => response.json())
            .then(data => setClasses(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);


    const handleViewStudents = (classId, className) => {
        setClassId(classId)
        setClassName(className)
        setAddingOrViewing("viewing")
    }

    const handleAddStudent = (classId, className) => {
        setClassName(className);
        setClassId(classId)
        setAddingOrViewing("adding")
    }

    const goBackFromAddingHandler=()=>{
        setAddingOrViewing("")
    }

    const goBackFromViewingHandler=()=>{
        setAddingOrViewing("")
    }
    
    const goBackHandler = () => {
        navigate("/admin")
    }

    return (
        <>
            <AdminNavbar/>

            {addingOrViewing === "adding" ? (
                <><StudentAddingToClass classId={classId} className={className}/>
                    <Button variant="contained"
                            sx={{
                                backgroundColor: '#c7b19f',
                                color: '#fff',
                                '&:hover': {
                                    backgroundColor: '#b29f8f',
                                },
                                marginTop: 2,
                            }}
                            onClick={goBackFromAddingHandler}>
                        Vissza
                    </Button></>
            ) : addingOrViewing === "viewing" ? (
                <StudentsOfClass classId={classId} className={className} onGoBack={goBackFromViewingHandler}/>
            ) : (<>
                <AdminClassList
                    classes={classes}
                    onViewStudents={handleViewStudents}
                    onAddStudent={handleAddStudent}
                /><Button variant="contained"
                          sx={{
                              backgroundColor: '#c7b19f',
                              color: '#fff',
                              '&:hover': {
                                  backgroundColor: '#b29f8f',
                              },
                              marginTop: 2,
                          }}
                          onClick={goBackHandler}>
                    Vissza
                </Button>
                    </>
            )}
           {/* */}

        </>
    );
}

export default Classes;


/*
* <>
        <AdminNavbar/>
            {addingOrViewing === "adding" ? (
                <StudentAddingToClass classId={classId} className={className} />
            ) : addingOrViewing === "viewing"? (
                <StudentsOfClass classId={classId} className={className} />
            ) : (
                <MainClasses
                    classes={classes}
                    onViewStudents={handleViewStudents}
                    onAddStudent={handleAddStudent}
                />
            )}
        </>
* */