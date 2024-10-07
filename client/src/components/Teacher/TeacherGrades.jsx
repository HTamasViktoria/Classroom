import { StyledSecondaryButton, CustomFlexBox, FlexColumnBox, StyledHeading } from '../../../StyledComponents';
import { Box } from "@mui/material";

function TeacherGrades({onChosenTask}) {
    
    const oneGradeHandler=()=>{
        onChosenTask("addGrades")
    }
    
    const manyGradeHandler=()=>{
        onChosenTask("addingBulkGrades")
    }
    
    return (
        <CustomFlexBox sx={{ gap: 4 }}>
            <FlexColumnBox>
                <h2>Jegy megtekintése</h2>
                <StyledSecondaryButton>Jegyek megtekintése</StyledSecondaryButton>
            </FlexColumnBox>
            <FlexColumnBox>
                <h2 variant="h3">Jegy beírása</h2>
                <Box sx={{ display: 'flex', gap: 2 }}>
                    <StyledSecondaryButton onClick={oneGradeHandler}>Egyéni</StyledSecondaryButton>
                    <StyledSecondaryButton onClick={manyGradeHandler}>Tömeges</StyledSecondaryButton>
                </Box>
            </FlexColumnBox>
        </CustomFlexBox>
    );
}

export default TeacherGrades;
