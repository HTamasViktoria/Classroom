import { Box, Container, FormControl, InputLabel, MenuItem, Select} from "@mui/material";
function ForWhatSelector({ selectedForWhat, handleForWhatChange }){
    
    const forWhats = ["Röpdolgozat", "Felelés", "Órai munka", "Beadandó", "Témazáró"]


    return (
        <Container>
            <Box my={4}>
             
                <FormControl fullWidth variant="outlined" sx={{ mb: 2 }}>
                    <InputLabel id="grade-select-label">Mire kapja a jegyet:</InputLabel>
                    <Select
                        labelId="grade-select-label"
                        value={selectedForWhat}
                        onChange={handleForWhatChange}
                        label="Mire kapja a jegyet?"
                        sx={{
                            backgroundColor: 'background.default',
                            color: 'text.primary',
                            '& .MuiOutlinedInput-notchedOutline': {
                                borderColor: 'primary.main',
                            },
                            '&:hover .MuiOutlinedInput-notchedOutline': {
                                borderColor: 'primary.dark',
                            },
                            '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                                borderColor: 'primary.dark',
                            },
                        }}
                    >

                        {forWhats.map((forWhat, index) => (
                            <MenuItem key={index} value={forWhat}>
                                {forWhat}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </Box>
        </Container>
    );
}
export default ForWhatSelector