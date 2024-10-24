import {StatisticsContainer} from "../../../StyledComponents.js";
import {getAverageBySubjectFullYear, getDifference} from "../../../GradeCalculations.js";
import ClassAverageCalculator from "./ClassAverageCalculator.jsx";
import {useEffect} from "react";

function ParentStatistics({grades, subject, chosenMonthIndex, id, averages}){


    useEffect(() => {
        console.log(`grades a ParentStatistics-ben:${grades}`)
    }, []);
    
    return (
        <>
        <span style={{ display: 'block' }}>
            {`éves átlag: ${getAverageBySubjectFullYear(grades, subject)}`}
        </span>
            <span style={{ display: 'block' }}>
            {`előző hónaphoz képest: ${getDifference(grades, subject, chosenMonthIndex)}`}
        </span>
            <ClassAverageCalculator averages={averages} subject={subject} />
        </>
    );

}

export default ParentStatistics