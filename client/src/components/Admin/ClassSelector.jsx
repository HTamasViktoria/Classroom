import {StyledFormControl, StyledInputLabel, StyledMenuItem, StyledSelect} from "../../../StyledComponents.js";
import React from "react";

function ClassSelector({classes, handleClassClick, selectedClass }){
    return(<>

        <StyledFormControl>
            <StyledInputLabel id="select-class-label">Osztály:</StyledInputLabel>
            <StyledSelect
                labelId="select-class-label"
                value={selectedClass ? selectedClass.id : ""}
                onChange={(e) =>
                    handleClassClick(classes.find((cls) => cls.id === e.target.value))
                }
            >
                {classes.length > 0 ? (
                    classes.map((cls) => (
                        <StyledMenuItem key={cls.id} value={cls.id}>
                            {cls.name}
                        </StyledMenuItem>
                    ))
                ) : (
                    <StyledMenuItem disabled>No classes available</StyledMenuItem>
                )}
            </StyledSelect>
        </StyledFormControl>
    </>)
}

export default ClassSelector