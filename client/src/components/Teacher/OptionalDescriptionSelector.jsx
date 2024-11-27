import { TextField } from "@mui/material";
import { useState } from "react";
import { CustomBox } from '../../../StyledComponents';

function OptionalDescriptionSelector(props) {
    const [selectedDescription, setSelectedDescription] = useState("");

    const descriptionChangeHandler = (event) => {
        const newDescription = event.target.value;
        setSelectedDescription(newDescription);
        props.onOptionalDescriptionChange(newDescription);
    };

    return (
        <CustomBox sx={{ width: '100%' }}>
            <TextField
                label="Leírás - opcionális"
                type="text"
                value={selectedDescription}
                onChange={descriptionChangeHandler}
                InputLabelProps={{ shrink: true }}
                multiline
                rows={6}
                fullWidth
            />
        </CustomBox>
    );
}

export default OptionalDescriptionSelector;
