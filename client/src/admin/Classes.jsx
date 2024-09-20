import React, {useState, useEffect} from "react";
import MainClasses from "../components/Admin/MainClasses.jsx";
import StudentsOfClass from "../components/Admin/StudentsOfClass.jsx";
import StudentAddingToClass from "../components/Admin/StudentAddingToClass.jsx";


function Classes() {
   
    
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

    
const handleViewStudents = (classId, className) =>{
    setClassId(classId)
    setClassName(className)
    setAddingOrViewing("viewing")
}

const handleAddStudent =(classId, className) =>{
    setClassName(className);
    setClassId(classId)
    setAddingOrViewing("adding")
}

    return (
        <>
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
    );
}

export default Classes;
