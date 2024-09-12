import { TextField } from "@mui/material";
import { useState } from "react";
function DescriptionSelector(props){
    
    const [selectedText, setSelectedText] = useState("")
    const handleTextChange = (event)=>{
        setSelectedText(event.target.value)
        props.onDescriptionChange(event.target.value)
    }
    
    
    
    return(<>
        <TextField
            label={props.type == "Homework" ? "A házi feladat leírása" :
                props.type == "Exam" ? "A dolgozat témája" :
        props.type == "MissingEquipment" ? "a hiányzó felszerelés" :
        props.type == "Other" ? "leírás" : "leírás"}
            type="text"
            value={selectedText}
            onChange={handleTextChange}
            InputLabelProps={{ shrink: true }}
            multiline
            rows={6}
            sx={{ width: '100%' }}
        />
    </>)
}
export default DescriptionSelector