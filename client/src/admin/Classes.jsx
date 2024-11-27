import React, { useState, useEffect } from "react";
import AdminClassList from "../components/Admin/AdminClassList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { AButton } from "../../StyledComponents";
import StudentAddingToClass from "../components/Admin/StudentAddingToClass.jsx";
import { useNavigate } from "react-router-dom";
import StudentsOfClass from "../components/Admin/StudentsOfClass.jsx";

function Classes() {
    const navigate = useNavigate();

    const [classes, setClasses] = useState([]);
    const [classId, setClassId] = useState(null);
    const [className, setClassName] = useState("");
    const [addingOrViewing, setAddingOrViewing] = useState("");

    useEffect(() => {
        fetch('/api/classes')
            .then(response => response.json())
            .then(data => setClasses(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    const handleViewStudents = (classId, className) => {
        setClassId(classId);
        setClassName(className);
        setAddingOrViewing("viewing");
    };

    const handleAddStudent = (classId, className) => {
        setClassName(className);
        setClassId(classId);
        setAddingOrViewing("adding");
    };

  


    return (
        <>
            <AdminNavbar />

            {addingOrViewing === "adding" ? (
                <>
                    <StudentAddingToClass
                        classId={classId}
                        className={className}
                        onSuccessfulAdding={()=>setAddingOrViewing("")}
                    />
                    <AButton
                        onClick={()=> setAddingOrViewing("")}
                    >
                        Vissza
                    </AButton>
                </>
            ) : addingOrViewing === "viewing" ? (
                <StudentsOfClass
                    classId={classId}
                    className={className}
                    onGoBack={()=> setAddingOrViewing("")}
                />
            ) : (
                <>
                    <AdminClassList
                        classes={classes}
                        onViewStudents={handleViewStudents}
                        onAddStudent={handleAddStudent}
                    />
                    <AButton
                        onClick={()=> navigate("/admin")}
                    >
                        Vissza
                    </AButton>
                </>
            )}
        </>
    );
}

export default Classes;
