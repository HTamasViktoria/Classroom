import {AppBar} from '@mui/material'
import {Toolbar, Typography, Box} from "@mui/material";
import {useNavigate} from "react-router-dom";


function AdminNavbar() {

    const navigate = useNavigate();

    return (
        <>
            <AppBar sx={{backgroundColor: '#cfc5b0'}}>
                <Toolbar>
                    <Box sx={{display: 'flex', justifyContent: 'left', gap:6, width: '100%'}}>
                        <Typography
                            component='div'
                            onClick={() => navigate("/admin/teachers")}
                            style={{cursor: 'pointer'}}
                        >
                            Tanárok
                        </Typography>
                        <Typography
                            component='div'
                            onClick={() => navigate("/admin/students")}
                            style={{cursor: 'pointer'}}>Diákok</Typography>
                        <Typography
                            component='div'
                            onClick={() => navigate("/admin/classes")}
                            style={{cursor: 'pointer'}}>Osztályok</Typography>

                    </Box>
                </Toolbar>
            </AppBar>
        </>
    );
    
    
    

}

export default AdminNavbar