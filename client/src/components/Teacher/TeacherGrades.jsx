import { StyledSecondaryButton, CustomFlexBox, StyledButton, FlexColumnBox, StyledHeading } from '../../../StyledComponents';
import { Box } from "@mui/material";

function TeacherGrades({onChosenTask, onGoBack}) {
    
    const oneGradeHandler=()=>{
        onChosenTask("addGrades")
    }
    
    const manyGradeHandler=()=>{
        onChosenTask("addingBulkGrades")
    }
    
    const onGradeViewHandler=()=>{
        onChosenTask("viewingGrades")
    }
    
    const goBackHandler=()=>{
        onGoBack()
    }
    
    return (
        <CustomFlexBox sx={{ gap: 4 }}>
            <FlexColumnBox>
                <h2>Jegy megtekintése</h2>
                <StyledSecondaryButton onClick={onGradeViewHandler}>Jegyek megtekintése</StyledSecondaryButton>
            </FlexColumnBox>
            <FlexColumnBox>
                <h2 variant="h3">Jegy beírása</h2>
                <Box sx={{ display: 'flex', gap: 2 }}>
                    <StyledSecondaryButton onClick={oneGradeHandler}>Egyéni</StyledSecondaryButton>
                    <StyledSecondaryButton onClick={manyGradeHandler}>Tömeges</StyledSecondaryButton>
                </Box>
                <StyledButton onClick={goBackHandler}>Vissza</StyledButton>
            </FlexColumnBox>
        </CustomFlexBox>
    );
}

export default TeacherGrades;
