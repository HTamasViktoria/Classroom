import {MenuItem, FormControl, InputLabel} from "@mui/material";
import {StyledSelect, StyledInputLabel, CustomBox} from '../../../StyledComponents';
import React, {useEffect, useState} from "react";
import BulkGradeStudents from "./BulkGradeStudents.jsx";
import DateSelector from "./DateSelector.jsx";
import ForWhatSelector from "./ForWhatSelector.jsx";

function BulkGradeAdding({teacherId, teacherSubjects, onGoBack}) {

    const [selectedSubject, setSelectedSubject] = useState("");
    const [selectedDate, setSelectedDate] = useState("")
    const [selectedForWhat, setSelectedForWhat] = useState("")
    const [selectedSubjectName, setSelectedSubjectName] = useState("")
    
    const handleSubjectChange = (event) => {
        const selectedId = event.target.value;
        setSelectedSubject(selectedId);

        const selectedSubjectObj = teacherSubjects.find(subject => subject.id === selectedId);
        if (selectedSubjectObj) {
            setSelectedSubjectName(selectedSubjectObj.subject);
        } else {
            setSelectedSubjectName("");
        }
    };


    const dateChangeHandler = (e) => setSelectedDate(e);
    const forWhatChangeHandler = (e) => {
        setSelectedForWhat(e.target.value)
    }

    return (<>
            {!selectedSubject && <CustomBox sx={{width: '48%'}}>
                <FormControl fullWidth variant="outlined">
                    <StyledInputLabel id="subject-select-label">Tantárgy:</StyledInputLabel>
                    <StyledSelect
                        labelId="subject-select-label"
                        value={selectedSubject || ""}
                        onChange={handleSubjectChange}
                        label="Tantárgy"
                    >
                        {teacherSubjects.map((teacherSubject) => (
                            <MenuItem key={teacherSubject.id} data-subject-name={teacherSubject.subject} value={teacherSubject.id}>
                                {(`${teacherSubject.subject} - ${teacherSubject.className}`)}
                            </MenuItem>
                        ))}
                    </StyledSelect>
                </FormControl>
            </CustomBox>}

            {selectedSubject && !selectedDate && (
                <DateSelector
                    selectedDate={selectedDate}
                    onDateChange={dateChangeHandler}
                />
            )}
    
        
        {selectedSubject && selectedDate && !selectedForWhat && <ForWhatSelector selectedForWhat={selectedForWhat}
            handleForWhatChange={forWhatChangeHandler}/>}


            {selectedSubject && selectedDate && selectedForWhat && <BulkGradeStudents teacherSubject={selectedSubject}
            selectedDate={selectedDate}
            selectedForWhat={selectedForWhat}
            teacherId={teacherId}
            selectedSubjectName={selectedSubjectName}/>}



            </>
    );
}




export default BulkGradeAdding;
