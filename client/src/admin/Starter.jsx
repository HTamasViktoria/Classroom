
import {AppBar} from '@mui/material'
import {Toolbar, Typography, Box} from "@mui/material";


function Starter() {
    return (
        <AppBar  sx={{
            backgroundColor: '#d2a679'
        }}>
            <Toolbar>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                    <Typography component='div'>
                        Tanár hozzáadása
                    </Typography>
                    
                </Box>
            </Toolbar>
        </AppBar>
    )
}

export default Starter
