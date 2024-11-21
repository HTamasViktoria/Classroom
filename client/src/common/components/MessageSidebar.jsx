import {LeftList, LeftListItem, Sidebar} from "../../../StyledComponents.js";

function MessageSidebar({onChosing}){
    return(<>
        <Sidebar>
        <LeftList>
            <LeftListItem onClick={()=>onChosing("new")}>
                Új üzenet írása
            </LeftListItem>
            <LeftListItem onClick={()=>onChosing("incoming")}>
                Beérkezett üzenetek
            </LeftListItem>
            <LeftListItem onClick={()=>onChosing("sent")}>
                Elküldött üzenetek
            </LeftListItem>
            <LeftListItem onClick={()=>onChosing("trash")}>
                Kuka
            </LeftListItem>
        </LeftList>
    </Sidebar></>)
}

export default MessageSidebar