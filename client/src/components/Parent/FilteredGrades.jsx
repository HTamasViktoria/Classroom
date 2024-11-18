
import {useEffect, useState} from "react";

function FilteredGrades({studentId, subject, onData, chosenMonthIndex}) {

    const [grades, setGrades] = useState([])

    
    useEffect(() => {
        const filterGradesByMonth = (subject) =>
            grades.filter((grade) => new Date(grade.date).getMonth() === chosenMonthIndex && grade.subject === subject);

        const filteredGrades = filterGradesByMonth(subject);
        console.log(`filteredgrades a FilteredGradesben:${filteredGrades}`)
        onData(filteredGrades);
    }, [subject, studentId, grades, chosenMonthIndex])


    useEffect(() => {
        fetch(`/api/grades/${studentId}`)
            .then(response => response.json())
            .then(data => {
               setGrades(data)
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [chosenMonthIndex]);

    
    return null;
}

export default FilteredGrades