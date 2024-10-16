import React, { useEffect, useState } from "react";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos.js";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos.js";

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
            console.log("Nem lehet visszafelé lapozni");
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
            console.log("Nem lehet előre lapozni");
        } else {
            const nextMonthIndex = (chosenMonthIndex + 1) % monthNames.length;
            setChosenMonth(monthNames[nextMonthIndex]);
            setChosenMonthIndex(nextMonthIndex);
            onMonthChange(nextMonthIndex);
        }
    };

    return (<>

            <ArrowBackIosIcon onClick={monthBackHandler} sx={{ margin: '0 10px' }} />
            <span>{chosenMonth}</span>
            <ArrowForwardIosIcon onClick={monthForwardHandler} sx={{ margin: '0 10px' }} />
        </>
    );
}

export default MonthSelector;
