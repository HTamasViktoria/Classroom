import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    palette: {
        primary: {
            main: '#d9c2bd',
            dark: '#c2a6a0',
        },
        secondary: {
            main: '#a2c4c6',
            dark: '#8ab2b5',
        },
        tertiary: {
            main: '#82b2b8',
            dark: '#6e9ea4',
        },
        navbar: {
            main: '#c6ac85',
        },
        background: {
            default: '#e2e5cb',
        },
        text: {
            primary: '#000',
            secondary: '#000',
        },
    },
    typography: {
        fontFamily: 'Inter, system-ui, Avenir, Helvetica, Arial, sans-serif',
        fontWeightRegular: 400,
        h6: {
            fontSize: '1.25rem',
            fontWeight: 600,
            color: '#000',
        },
        body1: {
            fontSize: '0.875rem',
            color: '#000',
        },
        button: {
            fontSize: '1rem',
            fontWeight: 600,
            color: '#fff',
        },
    },
    components: {
        MuiButton: {
            styleOverrides: {
                root: {
                    backgroundColor: '#82b2b8',
                    '&:hover': {
                        backgroundColor: '#6e9ea4',
                    },
                    margin: '8px 0',
                },
            },
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    marginBottom: '16px',
                    width: '100%',
                },
            },
        },
    },
});

export default theme;
