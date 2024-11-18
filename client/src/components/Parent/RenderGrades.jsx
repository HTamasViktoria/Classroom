import React, { useState, useCallback, useEffect } from "react";
import { TableCell, TableRow } from "@mui/material";
import ParentStatistics from "./ParentStatistics.jsx";
import { AButton, PopupContainer } from "../../../StyledComponents.js";
import FilteredGrades from "./FilteredGrades.jsx";

function RenderGrades({
                          studentId,
                          subject,
                          index,
                          grades,
                          chosenMonthIndex,
                          isEditable,
                          teacherSubjects,
                          nameOfClass,
                          onEditHandler
                      }) {
    const [hoverDate, setHoverDate] = useState("");
    const [hoverForWhat, setHoverForWhat] = useState("");
    const [popupPosition, setPopupPosition] = useState({ top: 0, left: 0 });
    const [filteredGrades, setFilteredGrades] = useState([]);

    const handleHover = useCallback((grade, element) => {
        const rect = element.getBoundingClientRect();
        setHoverDate(new Date(grade.date).toLocaleDateString());
        setHoverForWhat(grade.forWhat);
        setPopupPosition({
            top: rect.top + window.scrollY - 40,
            left: rect.left + window.scrollX + rect.width + 10,
        });
    }, []);

    const clearHover = useCallback(() => {
        setHoverDate("");
        setHoverForWhat("");
    }, []);

    const editHandler = useCallback((e) => {
        let chosenEditSubject = e.target.id;
        onEditHandler(chosenEditSubject);
    }, [onEditHandler]);

    useEffect(() => {
        setFilteredGrades([]);
    }, [studentId, subject, chosenMonthIndex]);

    return (
        <>
            <FilteredGrades
                studentId={studentId}
                onData={(data) => setFilteredGrades(data)}
                subject={subject}
                chosenMonthIndex={chosenMonthIndex}
            />
            <PopupContainer
                id="popUp"
                top={popupPosition.top}
                left={popupPosition.left}
                visible={!!hoverDate && !!hoverForWhat}
            >
                <span>{hoverForWhat}</span>
                <span>{hoverDate}</span>
            </PopupContainer>
            <TableRow key={`${subject}-${index}`}>
                <TableCell>{subject}</TableCell>
                <TableCell>
                    {filteredGrades.map((grade) => (
                        <span
                            key={grade.id}
                            onMouseEnter={(e) => handleHover(grade, e.currentTarget)}
                            onMouseLeave={clearHover}
                            className="grade"
                            style={{ margin: "0 10px" }}
                        >
                            {grade.value}
                        </span>
                    ))}
                </TableCell>
                <TableCell>
                    <ParentStatistics
                        studentId={studentId}
                        grades={grades}
                        subject={subject}
                        chosenMonthIndex={chosenMonthIndex}
                    />
                </TableCell>
                {isEditable && teacherSubjects.some((ts) => ts.className === nameOfClass && ts.subject === subject) && (
                    <TableCell>
                        <AButton id={subject} onClick={editHandler}>Szerkesztés</AButton>
                    </TableCell>
                )}
            </TableRow>
        </>
    );
}

export default RenderGrades;
