import {useState} from "react";
import {AButton} from "../../StyledComponents.js";
import ViewGradesBySubjects from "../components/Teacher/ViewGradesBySubjects.jsx";
import ViewGradesByClass from "../components/Teacher/ViewGradesByClass.jsx";
import ViewGradesByStudent from "../components/Teacher/ViewGradesByStudent.jsx";
import {useNavigate, useParams} from "react-router-dom";
import TeacherNavbar from "../components/Teacher/TeacherNavbar.jsx";
function TeacherViewingGrades() {

    const {id} = useParams();
    const navigate = useNavigate();
    
    const [chosen, setChosen] = useState("")

    const chosenHandler = (e) => {
        setChosen(e.target.id)
    }


    const chosenNoneHandler = () => {
        setChosen("")
    }

    return (<>  <TeacherNavbar id={id}/>

            {chosen === "bySubject" ? (<ViewGradesBySubjects onGoBack={chosenNoneHandler} teacherid={id}/>) :

                chosen === "byClass" ? (<ViewGradesByClass onGoBack={chosenNoneHandler} teacherid={id}/>) :

                    chosen === "byStudent" ? (
                            <ViewGradesByStudent onGoBack={chosenNoneHandler}
                                                 teacherId={id}/>) :

                        (<>
                            <AButton id={"bySubject"} onClick={chosenHandler}>Tantárgy alapján</AButton>
                            <AButton id={"byClass"} onClick={chosenHandler}>Osztály alapján</AButton>
                            <AButton id={"byStudent"} onClick={chosenHandler}>Tanuló alapján</AButton>
                            
                            <AButton onClick={() => navigate(`/teacher/grades/${id}`)}>Vissza</AButton>
                        </> )}
        </>
    )


}

export default TeacherViewingGrades