import React, { useState, useEffect } from "react";
import {
    AButton,
} from "../../../StyledComponents.js";
import GradeDetailsBySubjectByClass from "./GradeDetailsBySubjectByClass.jsx";
import ByClassTableContainer from "./ByClassTableContainer.jsx";
import SubjectAveragesFetcher from "./SubjectAveragesFetcher.jsx";

function ClassOfStudentsSelector({ subject, onGoBack }) {
    
    const [classes, setClasses] = useState([]);
    const [viewingDetails, setViewingDetails] = useState(false);
    const [classId, setClassId] = useState("")
    const [className, setClassName] = useState("")
    const [averages, setAverages] = useState([])

    useEffect(() => {
        fetch(`/api/classes/getClassesBySubject/${subject}/`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                setClasses(data);
            })
            .catch(error => console.error('Error fetching classes:', error));
    }, [subject]);


    const viewDetailsHandler = (id) => {
      
        const chosenClassId = id;
        const selectedClass = classes.find(cls => cls.id === chosenClassId);

        if (selectedClass) {
            setClassName(selectedClass.name);
            setClassId(chosenClassId);
            setViewingDetails(true);
        }
    };


    
    return (
        <>
            <SubjectAveragesFetcher subject={subject} onData={(data)=>setAverages(data)}/>
            {viewingDetails===true ?
                (<GradeDetailsBySubjectByClass 
                    onGoBack={()=> onGoBack()} 
                    subject={subject} classId={classId} 
                    className={className}/>) : 
                
                (<>
                    <ByClassTableContainer subject={subject} averages={averages} 
                                           onChosenClass={(id)=>viewDetailsHandler(id)} classes={classes}/>
                    
                    <AButton onClick={()=>onGoBack()}>Vissza</AButton>
                </>)}
           
        </>
    );
}

export default ClassOfStudentsSelector;
