import {AppBar} from '@mui/material'
import {Toolbar, Typography, Box} from "@mui/material";
import { useNavigate } from "react-router-dom";



function AdminNavbar() {

    const navigate = useNavigate();

    return (
        <>
            <AppBar sx={{ backgroundColor: '#b5a58d' }}>
                <Toolbar>
                    <Box sx={{ display: 'flex', justifyContent: 'left', gap: 4, width: '100%' }}>
                        <Typography
                            component='div'
                            onClick={() => navigate("/teachers") }
                            style={{ cursor: 'pointer' }}
                        >
                            Tanárok
                        </Typography>
                        <Typography
                            component='div'
                            onClick={() => navigate("/students") }
                            style={{cursor:'pointer'}}>Diákok</Typography>
                    </Box>
                </Toolbar>
            </AppBar>           
        </>
    );
}

export default AdminNavbar
