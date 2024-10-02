import React, { useState } from "react";
import { Typography } from "@mui/material";
import { StyledTextField } from '../../../StyledComponents';

function DescriptionSelector(props) {
    const [selectedText, setSelectedText] = useState("");

    const handleTextChange = (event) => {
        setSelectedText(event.target.value);
        props.onDescriptionChange(event.target.value);
    };

    const labelText = (() => {
        switch (props.type) {
            case "Homework":
                return "A házi feladat leírása";
            case "Exam":
                return "A dolgozat témája";
            case "MissingEquipment":
                return "A hiányzó felszerelés";
            case "Other":
                return "Leírás";
            default:
                return "Leírás";
        }
    })();

    return (
        <>
            <Typography variant="body1">{labelText}</Typography>
            <StyledTextField
                type="text"
                value={selectedText}
                onChange={handleTextChange}
                InputLabelProps={{ shrink: true }}
                multiline
                rows={6}
            
            />
        </>
    );
}

export default DescriptionSelector;
