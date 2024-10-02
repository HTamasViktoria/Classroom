import { styled } from '@mui/system';
import { Box, Button, TableHead, TableCell, TextField, Select, FormControl, Typography, InputLabel } from '@mui/material';

export const CustomBox = styled(Box)(({ theme }) => ({
    padding: theme.spacing(2),
    width: '100%',
    maxWidth: '900px',
    margin: '0 auto',
    backgroundColor: theme.palette.background.default,
}));


export const CustomFlexBox = styled(Box)(({ theme }) => ({
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: theme.spacing(2),
}));


export const StyledButton = styled(Button)(({ theme }) => ({
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.common.white,
    '&:hover': {
        backgroundColor: theme.palette.primary.dark,
    },
    marginTop: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
}));


export const StyledSecondaryButton = styled(Button)(({ theme }) => ({
    backgroundColor: theme.palette.secondary.main,
    color: theme.palette.common.white,
    '&:hover': {
        backgroundColor: theme.palette.secondary.dark,
    },
    marginTop: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
}));


export const StyledTableHead = styled(TableHead)({});


export const StyledTableCell = styled(TableCell)(({ theme }) => ({
    backgroundColor: '#d9c2bd',
    color: '#000',
    fontWeight: 'bold',
    border: '1px solid rgba(224, 224, 224, 1)',
}));


export const StyledTextField = styled(TextField)(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    borderRadius: theme.shape.borderRadius,
}));


export const StyledSelect = styled(Select)(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    marginBottom: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
}));


export const StyledFormControl = styled(FormControl)(({ theme }) => ({
    width: '100%',
    maxWidth: '600px',
    margin: '0 auto',
}));


export const StyledTypography = styled(Typography)(({ theme }) => ({
    color: theme.palette.text.primary,
    marginBottom: theme.spacing(2),
}));


export const StyledInputLabel = styled(InputLabel)(({ theme }) => ({
    color: theme.palette.text.primary,
    '&.Mui-focused': {
        color: theme.palette.primary.main,
    },
}));


