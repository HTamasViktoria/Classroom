import React from "react";
import { TableCell } from "@mui/material";
import ParentStatistics from "./ParentStatistics.jsx";
import { StyledButton } from "../../../StyledComponents.js";

function RenderGrades({
                          subject,
                          index,
                          filterGradesByMonth,
                          startHover,
                          finishHover,
                          id,
                          grades,
                          chosenMonthIndex,
                          classAverages,
                          isEditable,
                          teacherSubjects,
                          nameOfClass,
                          editHandler,
                      }) {
    const filteredGrades = filterGradesByMonth(subject);

    return (
        <tr key={`${subject}-${index}`}>
            <TableCell>{subject}</TableCell>
            <TableCell>
                {filteredGrades.map((grade) => (
                    <span
                        onMouseEnter={(e) => startHover(grade, e.currentTarget)}
                        onMouseLeave={finishHover}
                        key={grade.id}
                        className="grade"
                        style={{ margin: "0 10px" }}
                    >
                        {grade.value}
                    </span>
                ))}
            </TableCell>
            <TableCell>
                <ParentStatistics
                    id={id}
                    grades={grades}
                    subject={subject}
                    chosenMonthIndex={chosenMonthIndex}
                    averages={classAverages}
                />
            </TableCell>
            {isEditable && teacherSubjects.some(ts => (ts.className === nameOfClass) && (ts.subject === subject)) ? (
                <TableCell>
                    <StyledButton id={subject} onClick={editHandler}>Szerkesztés</StyledButton>
                </TableCell>
            ) : null}
        </tr>
    );
}

export default RenderGrades;
