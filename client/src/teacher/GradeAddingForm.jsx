import {useState } from "react";
import { AButton, BButton, GradeAddingHeading, GradeAddingStack } from "../../StyledComponents";
import SubjectSelector from "../components/Teacher/SubjectSelector.jsx";
import StudentSelector from "../components/Teacher/StudentSelector.jsx";
import DateSelector from "../components/Teacher/DateSelector.jsx";
import GradeValueSelector from "../components/Teacher/GradeValueSelector.jsx";
import ForWhatSelector from "../components/Teacher/ForWhatSelector.jsx";
import { useNavigate } from 'react-router-dom';
import {useParams} from "react-router-dom";

function GradeAddingForm() {
  
    const {id} = useParams();
    const navigate = useNavigate();
    
    const [selectedStudentId, setSelectedStudentId] = useState("");
    const [selectedSubjectId, setSelectedSubjectId] = useState("");
    const [selectedSubjectName, setSelectedSubjectName] = useState("");
    const [selectedGrade, setSelectedGrade] = useState("");
    const [selectedDate, setSelectedDate] = useState("");
    const [selectedForWhat, setSelectedForWhat] = useState("")

   
    

    const studentChangeHandler = (e) => setSelectedStudentId(e.target.value);

  
    const subjectChangeHandler = (subjectId, subjectName) => {
        setSelectedSubjectId(subjectId);
        setSelectedSubjectName(subjectName);       
    };

    const gradeChangeHandler = (e) => setSelectedGrade(e.target.value);
    const dateChangeHandler = (e) => setSelectedDate(e);
    const forWhatChangeHandler =(e)=>{
        setSelectedForWhat(e.target.value)
    }
    

    const handleSubmit = (e) => {
        e.preventDefault();

        const formattedDate = new Date(selectedDate).toISOString();

        const gradeData = {
            teacherId: id,
            studentId: selectedStudentId,
            subject: selectedSubjectName,
            forWhat: selectedForWhat,
            read: false,
            value: selectedGrade,
            date: formattedDate
        };

        fetch('/api/grades', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(gradeData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                console.log('Grade added:', data);
                alert("Jegy sikeresen hozzáadva")
                navigate(`/teacher/grades/${id}`)
            })
            .catch(error => console.error('Error adding grade:', error));
    };

    return (
        <>
            <GradeAddingHeading>
                Jegy hozzáadása
            </GradeAddingHeading>
            
            <form noValidate onSubmit={handleSubmit}>
                <GradeAddingStack>
                    
                    <SubjectSelector
                        teacherId={id}
                        selectedSubjectId={selectedSubjectId}
                        onSubjectChange={subjectChangeHandler}
                    />
                    <StudentSelector
                        selectedSubjectId={selectedSubjectId}
                        selectedStudentId={selectedStudentId}
                        handleStudentChange={studentChangeHandler}
                    />
                    <ForWhatSelector
                    selectedForWhat={selectedForWhat}
                    handleForWhatChange={forWhatChangeHandler}
                    />
                    <GradeValueSelector
                        selectedGrade={selectedGrade}
                        handleGradeChange={gradeChangeHandler}
                    />
                    <DateSelector
                        selectedDate={selectedDate}
                        onDateChange={dateChangeHandler}
                    />
                    <AButton type='submit'>
                        Jegy hozzáadása
                    </AButton>
                    <BButton onClick={()=>navigate(`/teacher/grades/${id}`)}> Mégse </BButton>
                </GradeAddingStack>
            </form>
        </>
    );
}

export default GradeAddingForm;
