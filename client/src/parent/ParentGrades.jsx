import React from 'react';
import {useParams} from 'react-router-dom';
import MainGradeTable from "../common/components/MainGradeTable.jsx";
import ParentNavbar from "../components/Parent/ParentNavbar.jsx";


function ParentGrades() {
    const {id} = useParams();

    console.log(`id a ParentGrades-ben:${id}`)

    return (<>
            <ParentNavbar studentId={id}/>
            <MainGradeTable studentId={id} isEditable={false}/>
        </>
    );
}

export default ParentGrades;
