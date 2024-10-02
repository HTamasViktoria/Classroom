import React, { useState, useEffect } from "react";
import AdminClassList from "../components/Admin/AdminClassList.jsx";
import AdminNavbar from "../components/Admin/AdminNavbar.jsx";
import { StyledButton } from "../../StyledComponents";
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

    const goBackFromAddingHandler = () => {
        setAddingOrViewing("");
    };

    const goBackFromViewingHandler = () => {
        setAddingOrViewing("");
    };

    const goBackHandler = () => {
        navigate("/admin");
    };

    const successfulAddingHandler = () => {
        setAddingOrViewing("");
    };

    return (
        <>
            <AdminNavbar />

            {addingOrViewing === "adding" ? (
                <>
                    <StudentAddingToClass
                        classId={classId}
                        className={className}
                        onSuccessfulAdding={successfulAddingHandler}
                    />
                    <StyledButton
                        onClick={goBackFromAddingHandler}
                    >
                        Vissza
                    </StyledButton>
                </>
            ) : addingOrViewing === "viewing" ? (
                <StudentsOfClass
                    classId={classId}
                    className={className}
                    onGoBack={goBackFromViewingHandler}
                />
            ) : (
                <>
                    <AdminClassList
                        classes={classes}
                        onViewStudents={handleViewStudents}
                        onAddStudent={handleAddStudent}
                    />
                    <StyledButton
                        onClick={goBackHandler}
                    >
                        Vissza
                    </StyledButton>
                </>
            )}
        </>
    );
}

export default Classes;
