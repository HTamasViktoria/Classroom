import { useEffect, useState } from "react";
import {AButton} from "../../../StyledComponents.js";
import DateSelector from "./DateSelector.jsx";
import ForWhatSelector from "./ForWhatSelector.jsx";
import EditingGradeValueSelector from "./EditingGradeValueSelector.jsx";


function GradeEditForm({ grade, onGoBack, onRefreshing }) {
    
    const [selectedGrade, setSelectedGrade] = useState(grade.value);
    const [selectedDate, setSelectedDate] = useState(grade.date.split("T")[0]);
    const [selectedForWhat, setSelectedForWhat] = useState(grade.forWhat);
    const [grades, setGrades] = useState([]);
    const [updatedSelectedGrade, setUpdatedSelectedGrade] = useState("");

    
    
    useEffect(() => {
        fetch('/api/grades/gradevalues')
            .then(response => response.json())
            .then(data => setGrades(data))
            .catch(error => console.error('Error fetching data:', error));
    }, [grade]);

    
    useEffect(() => {
        if (grades.length > 0) {
            const selectedGradeString = selectedGrade.toString();
            const prevGrade = grades.find((grade) => grade.includes(selectedGradeString));
            if (prevGrade) {
                setUpdatedSelectedGrade(prevGrade);
            }
        }
    }, [grades, selectedGrade]);

    const gradeChangeHandler = (e) => setSelectedGrade(e.target.value);
    const dateChangeHandler = (newDate) => setSelectedDate(newDate);
    const forWhatChangeHandler = (e) => setSelectedForWhat(e.target.value);
    const goBackHandler = () => onGoBack();

    const submitHandler = (e) => {
        e.preventDefault();

        const formattedDate = new Date(selectedDate).toISOString();
        const gradeRequest = {
            teacherId: grade.teacherId.toString(),
            studentId: grade.studentId.toString(),
            subject: grade.subject,
            forWhat: selectedForWhat,
            value: selectedGrade,
            date: formattedDate,
        };
        
     
        fetch(`/api/grades/${grade.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(gradeRequest),
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                onRefreshing()
                onGoBack();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    };


    return (
        <>
            <form>
                <DateSelector selectedDate={selectedDate} onDateChange={dateChangeHandler} />
                <ForWhatSelector selectedForWhat={selectedForWhat} handleForWhatChange={forWhatChangeHandler} />
                <EditingGradeValueSelector selectedGrade={updatedSelectedGrade} handleGradeChange={gradeChangeHandler} grades={grades} />
                <button type="submit" onClick={submitHandler}>Módosítom</button>
            </form>
            <AButton onClick={goBackHandler}>Vissza</AButton>
        </>
    );
}

export default GradeEditForm;
