import {AButton} from "../../../StyledComponents.js";
import TeacherNavbar from "./TeacherNavbar.jsx";
import {useParams} from "react-router-dom";
import { useNavigate } from 'react-router-dom';
function Notifications() {

    const {id} = useParams();
    const navigate = useNavigate();
    
    return (<>
        <TeacherNavbar id={id}/>
        <AButton onClick={()=>navigate(`/teacher/notifications/add/${id}`)}>Új értesítés rögzítése</AButton>
        <AButton onClick={()=>navigate(`/teacher/notifalerts/${id}`)}>Értesítési visszaigazolások megtekintése</AButton>
        
    </>)
}

export default Notifications