import React, {useState, useEffect} from "react";
import {AButton} from "../../../StyledComponents.js";
import GradesOfClassBySubjectFetcher from "./GradesOfClassBySubjectFetcher.jsx";
import BySubjByClassTableCont from "./BySubjByClassTableCont.jsx";


function GradeDetailsBySubjectByClass({subject, classId, onGoBack, className}) {
    const [studentsOfClass, setStudentsOfClass] = useState([]);
    const [grades, setGrades] = useState([]);


    useEffect(() => {
        fetch(`/api/classes/students-of-a-class/${classId}`)
            .then(response => response.json())
            .then(data => {
                setStudentsOfClass(data);
            })
            .catch(error => console.error(`Error:`, error));
    }, [classId]);


    return (
        <>
            <GradesOfClassBySubjectFetcher classId={classId} subject={subject} onData={(data) => setGrades(data)}/>
            <BySubjByClassTableCont studentsOfClass={studentsOfClass}
                                    subject={subject}
                                    grades={grades}
             className={className}/>

            <AButton onClick={() => onGoBack()}>Vissza</AButton>
        </>
    );
}

export default GradeDetailsBySubjectByClass;
