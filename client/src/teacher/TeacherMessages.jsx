import MessagesMain from "../common/components/MessagesMain.jsx";
import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";
import {useParams} from "react-router-dom";
function TeacherMessages(){
    
    const {id} = useParams()
    
    return(<>
        <TeacherNavbar id={id}/>
        <MessagesMain id={id}/>
    </>)
}


export default TeacherMessages