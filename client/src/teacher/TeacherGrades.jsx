import {AButton,} from '../../StyledComponents';
import {useParams} from "react-router-dom";
import {useNavigate} from "react-router-dom";
import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";

function TeacherGrades() {

    const {id} = useParams();
    const navigate = useNavigate();
    

    return (
        <>
            <TeacherNavbar id={id}/>
            <AButton onClick={()=>navigate(`/teacher/viewGrades/${id}`)}>Jegyek megtekintése</AButton>
            <AButton onClick={()=>navigate(`/teacher/addingOneGrade/${id}`)}>Jegy beírása (egyéni)</AButton>
            <AButton onClick={()=>navigate(`/teacher/bulkGradeAdding/${id}`)}>Jegy beírása(tömeges)</AButton>
        </>
    );
}

export default TeacherGrades;
