import React from "react";
import {useParams} from "react-router-dom";
import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";
import TeacherMain from "../components/Teacher/TeacherMain.jsx";

function Starter() {
    const {id} = useParams();


    return (
        <>
            <TeacherNavbar id={id}/>
            <TeacherMain id={id}/>
        </>
    );
}

export default Starter;
