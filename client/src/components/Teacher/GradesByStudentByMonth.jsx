import React, {useState} from "react";
import {PopupContainer} from "../../../StyledComponents.js";

function GradesByStudentByMonth({studentGradesByMonth}) {

    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({top: 0, left: 0});
    const startHover = (grade, element) => {
        const date = new Date(grade.date).toLocaleDateString();
        setHoverDate(date);
        setHoverForWhat(grade.forWhat);
        const rect = element.getBoundingClientRect();
        setPopupPosition({
            top: rect.top + window.scrollY - 40,
            left: rect.left + window.scrollX + rect.width + 10,
        });
    };
    const finishHover = () => {
        setHoverDate("");
        setHoverForWhat("");
    };


    return (
        <> <PopupContainer
            id="popUp"
            top={popupPosition.top}
            left={popupPosition.left}
            visible={!!hoverDate && !!hoverForWhat}>
            <span>{hoverForWhat}</span>
            <span>{hoverDate}</span>
        </PopupContainer>
            
            {studentGradesByMonth.map((grade) => (
                <span
                    onMouseEnter={(e) => startHover(grade, e.currentTarget)}
                    onMouseLeave={finishHover}
                    key={grade.id}
                    className="grade"
                    style={{margin: "0 10px"}}
                >
                    {grade.value}
                </span>
            ))}
        </>
    );
}

export default GradesByStudentByMonth