import MessagesMain from "../common/components/MessagesMain.jsx";
import ParentNavbar from "../components/Parent/ParentNavbar.jsx";
import {useParams} from "react-router-dom";
import {useProfile} from "../contexts/ProfileContext.jsx"
function ParentMessages(){

    const {id} = useParams()
    const {logout, profile} = useProfile()

    return(<>
        <ParentNavbar id={id}/>
        <MessagesMain id={profile.id}/>
    </>)
}


export default ParentMessages