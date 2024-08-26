
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
                        Üzenetek
                    </Typography>
                    <Typography component='div'>
                        Osztályzatok
                    </Typography>
                    <Typography component='div'>
                        Értesítések
                    </Typography>
                    <Typography component='div'>
                        Hiányzások
                    </Typography>
                    <Typography component='div'>
                        Órarend
                    </Typography>
                    <Typography component='div'>
                        Csengetési rend
                    </Typography>
                </Box>
            </Toolbar>
        </AppBar>
    )
}

export default Starter
