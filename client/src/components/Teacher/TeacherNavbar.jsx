import { AppBar, Toolbar } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useTheme } from '@mui/material/styles';
import { StyledTypography } from '../../../StyledComponents';

function TeacherNavbar() {
    const navigate = useNavigate();
    const theme = useTheme();

    return (
        <AppBar sx={{ backgroundColor: theme.palette.navbar.main }}>
            <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', gap: 4, width: '100%' }}>
                <StyledTypography
                    variant="h6"
                    onClick={() => navigate("/grades")}
                    sx={{ cursor: 'pointer' }}
                >
                    Jegyek
                </StyledTypography>
                <AccountCircleIcon
                    sx={{ cursor: 'pointer', fontSize: 30, color: theme.palette.text.primary }}
                />
            </Toolbar>
        </AppBar>
    );
}

export default TeacherNavbar;
