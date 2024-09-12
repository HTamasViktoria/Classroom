import {AppBar} from '@mui/material'
import {Toolbar, Typography, Box} from "@mui/material";
import { useNavigate } from "react-router-dom";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';



function TeacherNavbar() {

    const navigate = useNavigate();

    return (
        <>
            <AppBar sx={{ backgroundColor: '#b5a58d' }}>
                <Toolbar>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 4, width: '100%' }}>
                        <Typography
                            component='div'
                            onClick={() => navigate("/grades") }
                            style={{ cursor: 'pointer' }}
                        >
                            Jegyek
                        </Typography>
                        <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
                            <AccountCircleIcon
                                sx={{ cursor: 'pointer', fontSize: 30 }}
                            />
                        </Box>
                    </Box>
                </Toolbar>
            </AppBar>
        </>
    );
}

export default TeacherNavbar
