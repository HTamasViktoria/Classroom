import {StatisticsContainer} from "../../../StyledComponents.js";
import {getAverageBySubjectFullYear, getDifference} from "../../../GradeCalculations.js";
import ClassAverageCalculator from "./ClassAverageCalculator.jsx";

function ParentStatistics({grades, subject, chosenMonthIndex, id}){
    return (<>
        <StatisticsContainer>
            <span>{`éves átlag: ${getAverageBySubjectFullYear(grades, subject)}`}</span>
            <span>{`előző hónaphoz képest: ${getDifference(grades, subject, chosenMonthIndex)}`}</span>
            <ClassAverageCalculator id={id} subject={subject}/>
        </StatisticsContainer></>)
}

export default ParentStatistics