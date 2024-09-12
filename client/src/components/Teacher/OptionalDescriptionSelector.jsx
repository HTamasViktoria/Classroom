import { TextField } from "@mui/material";
import { useState } from "react";
function OptionalDescriptionSelector(props){
    
    const [selectedDescription, setSelectedDescription] = useState("")
    const descriptionChangeHandler = (event) =>{
        setSelectedDescription(event.target.value)
        props.onOptionalDescriptionChange(selectedDescription)
    }
    
    
    return(<>
        <TextField
            label="leírás - opcionális"
            type="text"
            value={selectedDescription}
            onChange={descriptionChangeHandler}
            InputLabelProps={{ shrink: true }}
            multiline
            rows={6}
            sx={{ width: '100%' }}
        />
    </>)
}
export default OptionalDescriptionSelector