import { styled } from '@mui/system';
import { Box, Button, TableHead, TableCell, TextField, Select, FormControl, Typography, InputLabel } from '@mui/material';

// CustomBox for general use
export const CustomBox = styled(Box)(({ theme }) => ({
    padding: theme.spacing(2),
    width: '100%',
    maxWidth: '900px',
    margin: '0 auto',
    backgroundColor: theme.palette.background.default,
}));

// CustomFlexBox for general use with spacing
export const CustomFlexBox = styled(Box)(({ theme }) => ({
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: theme.spacing(2),
}));

// FlexColumnBox for vertical stacking
export const FlexColumnBox = styled(Box)(({ theme }) => ({
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-start',
}));

// Styled heading for uniform headings
export const StyledHeading = styled(Typography)(({ theme }) => ({
    marginBottom: theme.spacing(1), // Space between heading and button
    fontWeight: 'bold', // Optional: Makes heading bold
}));

// Styled button for primary actions
export const StyledButton = styled(Button)(({ theme }) => ({
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.common.white,
    '&:hover': {
        backgroundColor: theme.palette.primary.dark,
    },
    marginTop: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
}));

// Styled secondary button for alternative actions
export const StyledSecondaryButton = styled(Button)(({ theme }) => ({
    backgroundColor: theme.palette.secondary.main,
    color: theme.palette.common.white,
    '&:hover': {
        backgroundColor: theme.palette.secondary.dark,
    },
    marginTop: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
}));

// Styled table head with default styles
export const StyledTableHead = styled(TableHead)({});

// Styled table cell with custom styles
export const StyledTableCell = styled(TableCell)(({ theme }) => ({
    backgroundColor: '#d9c2bd',
    color: '#000',
    fontWeight: 'bold',
    border: '1px solid rgba(224, 224, 224, 1)',
}));

// Styled text field for input
export const StyledTextField = styled(TextField)(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    borderRadius: theme.shape.borderRadius,
}));
// Styled select dropdown
// Styled select dropdown
export const StyledSelect = styled(Select)(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    marginBottom: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
    width: 'auto', // Automatikus szélesség
    minWidth: '150px', // Minimális szélesség, hogy ne legyen túl kicsi
    padding: theme.spacing(1), // Padding, hogy a szöveg ne legyen túl közel a széléhez
}));



// Styled form control for form elements
export const StyledFormControl = styled(FormControl)(({ theme }) => ({
    width: '100%',
    maxWidth: '600px',
    margin: '0 auto',
}));

// Styled typography for text elements
export const StyledTypography = styled(Typography)(({ theme }) => ({
    color: theme.palette.text.primary,
    marginBottom: theme.spacing(2),
}));

// Styled input label for form inputs
export const StyledInputLabel = styled(InputLabel)(({ theme }) => ({
    color: theme.palette.text.primary,
    '&.Mui-focused': {
        color: theme.palette.primary.main,
    },
}));

// Popup container for dynamic positioning
export const PopupContainer = styled('div')`
    position: absolute;
    top: ${({ top }) => top}px; // Dynamically use the top value
    left: ${({ left }) => left}px; // Dynamically use the left value
    padding: 10px;
    background-color: rgba(255, 255, 255, 0.9);
    border: 1px solid #ccc;
    border-radius: 5px;
    display: ${({ visible }) => (visible ? 'flex' : 'none')}; // Dynamically control visibility
    flex-direction: column;
`;

// StatisticsContainer styled component using MUI styled
export const StatisticsContainer = styled(TableCell)`
    display: flex;
    flex-direction: column;
`;
