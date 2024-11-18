import React, { useEffect, useState } from "react";
import {StyledArrowBackIcon, StyledArrowForwardIcon} from "../../../StyledComponents.js"

function MonthSelector({ onMonthChange }) {
    const [chosenMonth, setChosenMonth] = useState("");
    const [chosenMonthIndex, setChosenMonthIndex] = useState(0);

    useEffect(() => {
        const currentMonthIndex = new Date().getMonth();
        setChosenMonth(monthNames[currentMonthIndex]);
        setChosenMonthIndex(currentMonthIndex);
        onMonthChange(currentMonthIndex);
    }, []);

    const monthNames = [
        "Január", "Február", "Március", "Április", "Május", "Június",
        "Július", "Augusztus", "Szeptember", "Október", "November", "December"
    ];

    const schoolMonths = [
        "Szeptember", "Október", "November", "December", "Január",
        "Február", "Március", "Április", "Május", "Június"
    ];

    const monthBackHandler = () => {
        const actualsIndexInSchoolMonth = schoolMonths.indexOf(chosenMonth);

        if (actualsIndexInSchoolMonth - 1 < 0) {
            alert("Nem lehet visszafelé lapozni");
        } else {
            const nextMonthIndex = (chosenMonthIndex - 1 + monthNames.length) % monthNames.length;
            setChosenMonth(monthNames[nextMonthIndex]);
            setChosenMonthIndex(nextMonthIndex);
            onMonthChange(nextMonthIndex);
        }
    };

    const monthForwardHandler = () => {
        const actualsIndexInSchoolMonth = schoolMonths.indexOf(chosenMonth);

        if (actualsIndexInSchoolMonth + 1 > 9) {
            alert("Nem lehet előre lapozni");
        } else {
            const nextMonthIndex = (chosenMonthIndex + 1) % monthNames.length;
            setChosenMonth(monthNames[nextMonthIndex]);
            setChosenMonthIndex(nextMonthIndex);
            onMonthChange(nextMonthIndex);
        }
    };

    return (<>

            <StyledArrowBackIcon onClick={monthBackHandler}/>
            <span>{chosenMonth}</span>
            <StyledArrowForwardIcon onClick={monthForwardHandler}/>
        </>
    );
}

export default MonthSelector;
