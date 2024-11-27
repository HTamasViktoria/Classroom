

const monthNames = [
    "Január", "Február", "Március", "Április", "Május", "Június",
    "Július", "Augusztus", "Szeptember", "Október", "November", "December"
];

export const getAverageBySubjectFullYear = (grades, subject) => {
    const gradesOfSubject = grades.filter(grade => grade.subject === subject);

    if (gradesOfSubject.length === 0) {
        return 0;
    }

    const sum = gradesOfSubject.reduce((accumulator, grade) => accumulator + grade.value, 0);
    const average = sum / gradesOfSubject.length;

    return Math.round(average * 100) / 100;
}

export const getAverageBySubjectByMonth = (grades, subject, monthIndex) => {

    const gradesOfSubjectByMonth = grades.filter(grade => {
        const gradeDate = new Date(grade.date);
        const gradeMonthIndex = gradeDate.getMonth();
        return grade.subject === subject && gradeMonthIndex === monthIndex;
    });

    if (gradesOfSubjectByMonth.length === 0) {
        return 0;
    }
    const sum = gradesOfSubjectByMonth.reduce((accumulator, grade) => accumulator + grade.value, 0);
    const average = sum / gradesOfSubjectByMonth.length;
    return average;
};

export const getDifference = (grades, subject, monthIndex) => {
    const thisMonthAverage = getAverageBySubjectByMonth(grades, subject, monthIndex);
    const prevMonthAverage = getAverageBySubjectByMonth(grades, subject, monthIndex-1);
    if(prevMonthAverage === 0){return "Nincs elég adat az előző hónapról"}
    const diff = (thisMonthAverage/prevMonthAverage)*100;
    const roundedDiff = Math.round(diff);
    const finalDiff = roundedDiff > 0 ? Math.ceil(roundedDiff) : Math.floor(roundedDiff);
    return (`${finalDiff}%`);
}
