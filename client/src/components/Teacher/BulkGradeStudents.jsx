import { useState } from "react";
import { CustomBox, StyledTypography, AButton, BulkStudentsList, ListItem} from '../../../StyledComponents';
import BulkGradeAddingByPerson from "./BulkGradeAddingByPerson.jsx";
import StudentsOfATeacherSubjectFetcher from "./StudentsOfATeacherSubjectFetcher.jsx";
function BulkGradeStudents({ selectedSubjectId, teacherId, selectedDate, selectedForWhat, selectedSubjectName }) {
    const [students, setStudents] = useState([]);
    
   
    return (
        <>
            <StudentsOfATeacherSubjectFetcher 
                selectedSubjectId={selectedSubjectId} 
                onData={(data)=>setStudents(data)}/>
            
        <CustomBox>
            {students.length > 0 ? (
                <BulkStudentsList>
                    {students.map(student => (
                        <ListItem key={student.id}>
                            <AButton>
                                {student.familyName} {student.firstName}
                            </AButton>
                    <BulkGradeAddingByPerson teacherId={teacherId} 
                                             selectedForWhat={selectedForWhat} 
                                             selectedDate={selectedDate}
                                             selectedSubjectName={selectedSubjectName}
                                             student={student} />
                        </ListItem>
                    ))}
                </BulkStudentsList>
            ) : (
                <StyledTypography>Nincsenek diákok a kiválasztott tantárgyban</StyledTypography>
            )}
        </CustomBox></>
    );
}

export default BulkGradeStudents;
