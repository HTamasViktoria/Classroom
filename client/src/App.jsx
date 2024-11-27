import {useState} from 'react'
import {AppBar} from '@mui/material'
import './App.css'
import {IconButton, Toolbar, Typography, Box} from "@mui/material";
import LocalLibraryIcon from '@mui/icons-material/LocalLibrary';
import {ThemeProvider} from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import theme from "../theme.js";

function App() {
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
        </ThemeProvider>
    );
}

export default App
