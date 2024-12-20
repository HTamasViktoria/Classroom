import { Checkbox, FormControlLabel, FormGroup } from "@mui/material";
import { useEffect, useState } from "react";

function ChooseFromStudentsSelector(props) {
 
    const [selectedStudents, setSelectedStudents] = useState([]);
    const [selectAll, setSelectAll] = useState(false);
    

    
    useEffect(() => {
        if (selectAll) {
            setSelectedStudents(props.students.map((student) => student.id));
        } else {
            setSelectedStudents([]);
        }
    }, [selectAll, props.students]);

    
    const handleSelectAllChange = (event) => {
        setSelectAll(event.target.checked);
    };

    const handleStudentChange = (studentId) => {
        setSelectedStudents((prevSelectedStudents) => {
            const newSelectedStudents = prevSelectedStudents.includes(studentId)
                ? prevSelectedStudents.filter((id) => id !== studentId)
                : [...prevSelectedStudents, studentId];
            return newSelectedStudents;
        });
    };

    useEffect(() => {
        props.onStudentChange(selectedStudents);
    }, [selectedStudents, props]);

    return (
        <FormGroup>
            <FormControlLabel
                control={
                    <Checkbox
                        checked={selectAll}
                        onChange={handleSelectAllChange}
                    />
                }
                label="Összes diák kiválasztása"
            />
            {props.students.map((student) => (
                <FormControlLabel
                    key={student.id}
                    control={
                        <Checkbox
                            checked={selectedStudents.includes(student.id)}
                            onChange={() => handleStudentChange(student.id)}
                        />
                    }
                    label={`${student.firstName} ${student.familyName}`}
                />
            ))}
        </FormGroup>
    );
}

export default ChooseFromStudentsSelector;
