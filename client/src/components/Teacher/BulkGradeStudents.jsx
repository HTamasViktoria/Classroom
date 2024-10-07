import { useEffect, useState } from "react";
import { CustomBox, StyledTypography, StyledButton } from '../../../StyledComponents';
import BulkGradeAddingByPerson from "./BulkGradeAddingByPerson.jsx";
function BulkGradeStudents({ teacherSubject, teacherId, selectedDate, selectedForWhat, selectedSubjectName }) {
    const [students, setStudents] = useState([]);
    

    useEffect(() => {
        fetch(`/api/teacherSubjects/getStudentsByTeacherSubjectId/${teacherSubject}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Hiba történt a diákok betöltésekor');
                }
                return response.json();
            })
            .then(data => {
                console.log(`selectedsubjectname a bulkgradestudents-ban:${selectedSubjectName}`)
                setStudents(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [teacherSubject]);



    
    return (
        <CustomBox>
            {students.length > 0 ? (
                <ul style={{ padding: 0 }}>
                    {students.map(student => (
                        <li key={student.id} style={{ listStyle: 'none', marginBottom: '10px', display: 'flex', alignItems: 'center' }}>
                            <StyledButton style={{ marginRight: '10px' }}>
                                {student.familyName} {student.firstName}
                            </StyledButton>
                    <BulkGradeAddingByPerson teacherId={teacherId} selectedForWhat={selectedForWhat} selectedDate={selectedDate}
                                             selectedSubjectName={selectedSubjectName}
                                             student={student} />
                        </li>
                    ))}
                </ul>
            ) : (
                <StyledTypography>Nincsenek diákok a kiválasztott tantárgyban</StyledTypography>
            )}
        </CustomBox>
    );
}

export default BulkGradeStudents;
