import {StatisticsContainer, StatisticsSpan} from "../../../StyledComponents.js";
import {getAverageBySubjectFullYear, getDifference} from "../../../GradeCalculations.js";
import ClassAverageCalculator from "./ClassAverageCalculator.jsx";
import {useEffect, useState} from "react";
import AllGradesFetcher from "./AllGradesFetcher.jsx";

function ParentStatistics({subject, chosenMonthIndex, studentId}){


   const [grades, setGrades] = useState([])
    const [averages, setAverages] = useState([])


    
    return (
        <>
            <AllGradesFetcher studentId={studentId} onData={(data)=>setGrades(data)}/>
            <ClassAverageCalculator studentId={studentId} subject={subject} onData={(data)=>setAverages(data)} />
        <StatisticsSpan>
            {`éves átlag: ${getAverageBySubjectFullYear(grades, subject)}`}
        </StatisticsSpan>
            <StatisticsSpan>
            {`előző hónaphoz képest: ${getDifference(grades, subject, chosenMonthIndex)}`}
        </StatisticsSpan>
            <ClassAverageCalculator averages={averages} subject={subject} />
        </>
    );

}

export default ParentStatistics