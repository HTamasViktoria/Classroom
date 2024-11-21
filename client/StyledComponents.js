import { styled } from '@mui/system';
import { Box, Button, TableHead,Grid, TableCell, TextField, MenuItem, Select, FormControl, Typography, InputLabel, Paper, Card, Stack, Table, TableContainer, TableRow } from '@mui/material';
import { AppBar, Toolbar,Badge, Icon } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';

// CustomBox for general use
export const CustomBox = styled(Box)(({ theme }) => ({
    padding: theme.spacing(0),
    width: '100%',
    maxWidth: '900px',
    margin: '0 auto',
    backgroundColor: theme.palette.background.default,
    paddingTop: theme.spacing(8), // megnövelt padding-top, próbáld ki
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
    marginBottom: theme.spacing(1),
    fontWeight: 'bold',
}));

// Styled button for primary actions
export const AButton = styled(Button)(({ theme }) => ({
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.common.white,
    '&:hover': {
        backgroundColor: theme.palette.primary.dark,
    },
    margin: theme.spacing(1), // minimális margin minden oldalra
    marginTop: theme.spacing(3), // felső margin hozzáadása
    borderRadius: theme.shape.borderRadius,
}));

// Styled secondary button for alternative actions
export const BButton = styled(Button)(({ theme }) => ({
    backgroundColor: theme.palette.secondary.main,
    color: theme.palette.common.white,
    '&:hover': {
        backgroundColor: theme.palette.secondary.dark,
    },
    marginTop: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
}));

// Styled table head with default styles
export const TableHeading = styled(TableHead)({});

// Styled table cell with custom styles
export const Cell = styled(TableCell)(({ theme }) => ({
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
export const StyledSelect = styled(Select)(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    marginBottom: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
    width: 'auto',
    minWidth: '150px',
    padding: theme.spacing(1),
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

export const PopupContainer = styled('td')`
    position: absolute;
    top: ${({ top }) => top}px;
    left: ${({ left }) => left}px;
    padding: 10px;
    background-color: rgba(255, 255, 255, 0.9);
    border: 1px solid #ccc;
    border-radius: 5px;
    display: ${({ visible }) => (visible ? 'flex' : 'none')};
    flex-direction: column;
`;

// StatisticsContainer styled component using MUI styled
export const StatisticsContainer = styled(TableCell)`
    display: flex;
    flex-direction: column;
`;

// Styled card for general use
export const StyledCard = styled(Card)(({ theme }) => ({
    maxWidth: 600,
    margin: '3em auto 2em auto',
    padding: theme.spacing(2),
}));

export const StyledTableContainer = styled(TableContainer)(({ theme }) => ({
    // ide bármit hozzáadhatsz, amit a StyledTableContainer-nek szeretnél
    // pl. padding, margin, stb.
    padding: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
    boxShadow: theme.shadows[2],
}));

// Styled table
export const StyledTable = styled(Table)({});

// Styled table row
export const StyledTableRow = styled(TableRow)({});
// Styled component for error messages
export const StyledErrorMessage = styled(Typography)(({ theme }) => ({
    color: theme.palette.error.main,
    textAlign: 'center',
    fontSize: '1.1rem',
    marginTop: theme.spacing(2),
}));

// Styled component for empty data message
export const StyledNoDataMessage = styled(Typography)(({ theme }) => ({
    textAlign: 'center',
    fontSize: '1.1rem',
    marginTop: theme.spacing(2),
}));
// Styled paper
export const StyledPaper = styled(Paper)({});

// Paper table container
export const PaperTableContainer = styled(Paper)(({ theme }) => ({}));

// Centered stack
export const CenteredStack = styled(Stack)(({ theme }) => ({
    marginTop: theme.spacing(2),
    alignItems: 'center',
}));



// Styled input
export const StyledInput = styled(TextField)({
    width: '100%',
    marginBottom: '1rem',
});



// Error message styling
export const ErrorMessage = styled('p')({
    color: 'red',
    textAlign: 'center',
    fontSize: '1.1rem',
    marginTop: '1rem',
});

// Styled form wrapper
export const StyledForm = styled('form')({
    maxWidth: '400px',
    margin: '0 auto',
    padding: '2rem',
    border: '1px solid #ddd',
    borderRadius: '8px',
    backgroundColor: '#f9f9f9',
});


export const StyledMenuItem = styled(MenuItem)(({ theme }) => ({
    color: theme.palette.text.primary,
}));


export const StyledStack = styled(Stack)(({ theme }) => ({
    width: '100%',
    gap: theme.spacing(4),
}));

/*
export const StyledCustomHeading = styled(Typography)(({ theme }) => ({
    fontSize: '2.5rem', // Beállíthatod tetszés szerint
    fontWeight: 700,    // Erősebb kiemelés
    color: theme.palette.primary.main, // Szín a fő szín alapján
    marginBottom: theme.spacing(3), // Alsó margó, hogy legyen hely az alatta lévő tartalomnak
    textAlign: 'center', // Ha középre akarod igazítani
     // Minden betű legyen nagybetű
}));
*/


// Styled grid container for input fields
export const StyledGrid = styled(Grid)(({ theme }) => ({
    width: '100%',
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: theme.spacing(3), // margin az egyes elemek között
}));

// A grid item elemek egyéni stílusokkal
export const StyledGridItem = styled(Grid)(({ theme }) => ({
    [theme.breakpoints.down('sm')]: {
        // Kis képernyőkön teljes szélesség
        flexBasis: '100%',
    },
    [theme.breakpoints.up('sm')]: {
        // Nagyobb képernyőkön fél szélesség
        flexBasis: '48%', // 2 elem egymás mellett (48% hely)
    },
}));








export const AdminStyledTableContainer = styled(TableContainer)`
    overflow: hidden;
`;

export const AdminStyledTableHead = styled(TableHead)`
    background-color: #f5f5f5;
`;

export const AdminStyledTableCell = styled(TableCell)`
    font-weight: bold;
    color: #333;
`;



export const AdminStyledTableRow = styled(TableRow)`
    &:nth-of-type(odd) {
        background-color: #f9f9f9;
    }
    &:nth-of-type(even) {
        background-color: #ffffff;
    }
`;





export const NavbarAppBar = styled(AppBar)`
    background-color: ${({ theme }) => theme.palette.navbar.main};
    position:fixed;
`;


export const NavbarSpacer = styled('div')`
    flex-grow: 1;
`;
// Styled komponens a Navbar Toolbar-hoz
export const NavbarToolbar = styled(Toolbar)`
    display: flex;
    justify-content: space-between;
    width: 100%;
`;

// Styled komponens a menü elemekhez

export const NavbarTypography = styled(Typography)`
    cursor: pointer;
    margin-right: 16px; /* 2 * 8px */
    font-size: 1.2rem; /* Nagyobb betűméret */
    font-weight: normal; /* Vastagabb betűtípus */
    color: #ffffff; /* Fehér szín */
    font-family: 'Roboto', sans-serif; /* Roboto betűtípus */
    text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.3); /* Finom szövegárnyék */
    transition: color 0.3s ease, text-shadow 0.3s ease; /* Simább animáció */

    &:hover {
        color: #ffcc00; /* Színváltozás hover esetén */
        text-shadow: 1px 1px 4px rgba(0, 0, 0, 0.5); /* Erősebb árnyék hover esetén */
    }
`;

// Styled komponens a profil boxhoz
export const NavbarBox = styled(Box)`
    display: flex;
    align-items: center;
    cursor: pointer;
    margin-right: 32px; /* 4 * 8px */
`;

// Styled komponens a profil ikonnak
export const NavbarAccountIcon = styled(AccountCircleIcon)`
    font-size: 30px;
    color: ${({ theme }) => theme.palette.text.primary};
`;

// Styled komponens a Badge-hez
export const NavbarBadge = styled(Badge)`
    margin-left: 8px;
`;

// Styled komponens a Kilépés gombhoz
export const NavbarButton = styled('button')`
    background: none;
    border: none;
    color: inherit;
    font: inherit;
    cursor: pointer;
    margin-right: 16px;
`;


export const LastNotificationsBox = styled(Box)`
    margin-top: 16px; /* mt: 4 = margin-top 16px a Material-UI-ban */
`;



export const LastNotificationsGrid = styled(Grid)`
    display: flex;
    flex-wrap: wrap;
    gap: 20px; /* spacing={3} = 24px gap */
    justify-content: flex-start; /* align items to the left */
`;


export const LastNotificationsCard = styled(Card)`
  background-color: ${({ theme }) => theme.palette.secondary.main};
  box-shadow: ${({ theme }) => theme.shadows[3]};
  transition: 0.3s;
  min-width: 175px;

  &:hover {
    transform: scale(1.05);
    box-shadow: ${({ theme }) => theme.shadows[6]};
  }
`;


export const LastNotifLabel = styled(Typography)`
 
  color: ${({ theme }) => theme.palette.text.secondary};
`;


export const LastNotifHead = styled(Typography)(({ theme }) => ({
    fontWeight: 'bold',
    color: theme.palette.text.secondary,
}));


export const LastNotifInfo = styled(Typography)(({ theme }) => ({
    marginTop: theme.spacing(1),
}));


export const StyledArrowBackIcon = styled(ArrowBackIosIcon)(({ theme }) => ({
    margin: '0 10px',
    cursor: 'pointer', // biztosítja, hogy az ikon mutatókurzorként jelenjen meg
}));


export const StyledArrowForwardIcon = styled(ArrowForwardIosIcon)(({ theme }) => ({
    margin: '0 10px',
    cursor: 'pointer', // biztosítja, hogy az ikon mutatókurzorként jelenjen meg
}));



export const StyledIconsBox = styled(Box)`
  display: flex;
  flex-direction: row;
  align-items: center;
  text-align: center;
  margin: 1;
`;


export const StyledBadge = styled(Badge)`
  & .MuiBadge-dot {
    background-color: red;
  }
  
  & .MuiBadge-standard {
    background-color: red;
    color: white;
    border-radius: 50%;
    padding: 0 4px;
  }
`;


export const StyledNotificationsBadgeTypography = styled(Typography)`
  margin-left: 1rem;
`;


export const StyledNotificationIcon = styled(Icon)`
  font-size: 4em;
`;

export const StyledNotificationBadgeBox = styled(Box)`
  cursor: pointer;
  padding: 0;
`;


export const NotificationIconsBox = styled(Box)`
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  align-items: flex-start;
  padding: 16px;  /* equivalent to p={4} */
`;



export const NotifContainer = styled(Box)(({ theme }) => ({
    padding: theme.spacing(2),  // padding: 2 a MUI-ban automatikusan átvált a megfelelő spacing értékre (8px * 2 = 16px)
}));


export const NotifHead = styled(Typography)(({ theme }) => ({
    marginBottom: theme.spacing(2),  // marginBottom: 2 a MUI-ban automatikusan átvált a megfelelő spacing értékre (8px * 2 = 16px)
}));


export const NotifBox = styled(Box)(({ theme }) => ({
    display: 'flex',
    justifyContent: 'space-between',
    marginTop: theme.spacing(2), // Ez helyettesíti az sx={{ marginTop: 2 }}-t
}));


export const NotifTable = styled(Table)(({ theme }) => ({
    minWidth: 650, // Helyettesíti az sx={{ minWidth: 650 }}
}));


export const StatisticsSpan = styled('span')({
    display: 'block', // Helyettesíti a style={{ display: 'block' }}
});

export const TeacherNavbarToolbar = styled(Toolbar)`
  display: flex;
  justify-content: space-between;
  width: 100%;
`;

export const SubjectChooseContainer = styled('div')`
  padding: 16px;
  display: flex;
  flex-direction: column;
  gap: 16px;
  /* További stílusokat is hozzáadhatsz, ha szükséges */
`;

export const GradeAddingHeading = styled(Typography)(({ theme }) => ({
    ...theme.typography.h4,
    marginBottom: theme.spacing(2),
}));


export const GradeAddingStack = styled(Stack)(({ theme }) => ({
    spacing: 2,
    width: 400,
}));


export const BulkContainer = styled('div')`
    padding: 24px; /* Ez 3 * 8px, ami megegyezik a padding: 3-al a Material-UI sx propban */
`;

export const BulkBox = styled('div')`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 100%;
    gap: 24px; /* Ez 3 * 8px, ami megegyezik a gap={3}-al a Material-UI sx propban */
`;

export const LeftInnerBox = styled('div')`
    display: flex;
    flex-direction: column;
    width: 50%;
    padding: 16px; /* Ez 2 * 8px, ami megegyezik a padding={2}-al a Material-UI sx propban */
    gap: 16px; /* Ez 2 * 8px, ami megegyezik a gap={2}-al a Material-UI sx propban */
`;

export const RightInnerBox = styled('div')`
    width: 50%;
    max-width: 700px;
    padding: 16px; /* Ez 2 * 8px, ami megegyezik a padding={2}-al a Material-UI sx propban */
`;

export const BulkStudentsList = styled('ul')`
  padding: 0;
  list-style: none; // Hozzáadtam, hogy eltávolítsam az alapértelmezett listajelzőket, ha szükséges
`;

export const ListItem = styled('li')`
    list-style: none;
    margin-bottom: 10px;
    display: flex; // Elemelrendezés sorban
    flex-direction:row;
    gap: 10px; // Ha szükséges, egy kis távolság a gyermekelemek között
    align-items: center; // A gyermekek vertikális középre igazítása
`;

export const nameButton = styled('button')`
    display: inline-block;
    white-space: nowrap;
    padding: 8px 16px;
    border: none;
    background-color: #007bff;
    color: white;
    font-size: 16px;
    cursor: pointer;
    border-radius: 4px;
    text-align: center;

    &:hover {
        background-color: #0056b3;
    }
`;

export const StyledBox = styled('div')`
  margin-top: 32px;  /* my={4} a Material-UI-ban 32px-t jelent, mivel 1 egység = 8px */
  margin-bottom: 32px;
    /* my={4} a Material-UI-ban 32px-t jelent, mivel 1 egység = 8px */
`;

export const BulkHeading = styled(Typography)`
  && {
    font-size: 1.25rem;  /* Ez felel meg az h6-nak a Material-UI-ban */
    margin-bottom: 16px;  /* Ez felel meg a gutterBottom-nak, amely 16px-t ad */
  }
`;


export const BulkFormControl = styled(FormControl)`
  && {
    width: 100%;  /* fullWidth a Material-UI-ban */
    margin-bottom: 16px;  /* sx={{ mb: 2 }} => margin-bottom: 2 * 8px, azaz 16px */
  }
`;


export const BulkSelect = styled(Select)`
  && {
    background-color: ${({ theme }) => theme.palette.background.default}; 
    color: ${({ theme }) => theme.palette.text.primary};
    
    & .MuiOutlinedInput-notchedOutline {
      border-color: ${({ theme }) => theme.palette.primary.main};
    }

    &:hover .MuiOutlinedInput-notchedOutline {
      border-color: ${({ theme }) => theme.palette.primary.dark};
    }

    &.Mui-focused .MuiOutlinedInput-notchedOutline {
      border-color: ${({ theme }) => theme.palette.primary.dark};
    }
  }
`;


export const StudentsHeading = styled(Typography)`
  font-size: ${({ theme }) => theme.typography.h4.fontSize};
  font-weight: ${({ theme }) => theme.typography.h4.fontWeight};
  margin-bottom: ${({ theme }) => theme.spacing(2)}; // Gutter bottom (16px)
`;


export const StudentsFormControl = styled(FormControl)`
  width: 100%; /* fullWidth */
  & .MuiOutlinedInput-root {
    border-radius: 4px; /* Ha szükséges, beállíthatod a szegélyeket is */
  }
`;


export const Sidebar = styled('div')(({ theme }) => ({
    width: '200px',
    padding: '10px',
    marginRight: '1em', // Ez helyes
    // Helyesen hozzáférve a theme objektumhoz
    borderRight: '1px solid #ddd',
}));

export const LeftListItem = styled('li')(({ theme }) => ({
    listStyleType: 'none',   // Listaelemek pontjainak eltávolítása
    margin: '5px 0',         // Margó a listaelemek között
    width: '100%',           // Teljes szélesség beállítása
    // Theme objektum helyes használata
    padding: '10px', 
    borderRadius:'5px',// Belül kis hely
    cursor: 'pointer',       // Mutatóváltás hoverre

    '&:hover': {             // Hover állapot stílusa
      
        backgroundColor: theme.palette.secondary.dark,// Betű vastagítása
        boxShadow: '0px 4px 6px rgba(0, 0, 0, 0.1)', // Árnyék hozzáadása// Háttérszín változtatása
        transition: 'all 0.3s ease', // Simább animáció
    },
}));


export const MessageMainContainer = styled('div')({
    display: 'flex', // Flexbox elrendezés
    flexDirection: 'row',
    alignItems: 'stretch',
    gap: '10px',
});


export const LeftList = styled('ul')({
    listStyleType: 'none', // Nincs lista stílus (pontok)
    padding: '0',          // Nincs padding
});





/*export const IncomingMessagesContainer = styled('div')({
    flex: 1,               // Flex: 1 a jobb oldali résznek, hogy kitöltse a rendelkezésre álló helyet
    padding: '10px', // Padding hozzáadása a tartalom köré
});*/


export const HeadingTH = styled('th')({
    padding: '10px',
    border: '1px solid #ddd',
});

export const MessagesTable = styled('table')({
    width: '100%', 
    minHeight: '200px',// 100%-os szélesség
    borderCollapse: 'collapse', 
   // A cellák közötti határok összeolvadása
});


/*export const MessageLine = styled('div')({
    maxHeight: '5em',        // Maximális magasság beállítása
    overflow: 'hidden',      // Ha a tartalom túl nagy, elvágja
    display: 'flex',         // Flexbox használata a sorok elrendezésére
    flexDirection: 'row',    // Elem közötti elrendezés
    alignItems: 'left',    // Vertikális központosítás
    '&:hover': {             // Hover állapot stílusa
        fontWeight: 'bold',   // Betűk vastagítása// Háttérszín világos szürkére
        transition: 'all 0.3s ease', // Simább átmenet
    },
});

export const MessageDetail = styled('div')({
   /!* maxHeight: '2em', *!/       // Maximális magasság beállítása
    overflow: 'hidden',      // Ha a tartalom túl nagy, elvágja
    padding: '0.5em',        // Padding a tartalom körül
    flex: 1,                 // Minden elem ugyanakkora helyet kapjon
    whiteSpace: 'nowrap',    // Ne törjön a szöveg
    textOverflow: 'ellipsis',// Ha túl hosszú, három ponttal jelzi
});*/


/*export const UnreadButton = styled('td')({
    padding: '10px',
    border: '1px solid #ddd',

    '& button': {
        padding: '5px 10px',
    },
});*/

export const MessageItem = styled('td')({
    padding: '10px',
    border: '1px solid #ddd',
});


export const FullWidthH2 = styled('h2')({
    width: '100%',         // Teljes szélesség
    margin: 0,            // Ne legyen margó
    padding: 0,           // Ne legyen padding
    textAlign: 'center',    // Balra igazítás, ha szeretnéd
});


export const MessageViewerBox = styled(Box)({
    padding: 20,
    maxWidth: '700px',
    margin: '0 auto',
    boxShadow: 3,
    borderRadius: 2,
    backgroundColor: '#fff',
});


export const InnerBox = styled(Box)({
    marginBottom: 2,
});


export const MessageDetailTitle = styled(Typography)({
    fontWeight: 'bold',
    marginBottom: 1,
    variant: 'h6',  // A `variant` nem tud közvetlenül bekerülni így, tehát a `variant="h6"`-ot a komponensnél kell beállítani
});


export const InnerTitle = styled(Typography)({
    variant: 'body1',
    fontWeight: 'normal',
    marginBottom: 1,
});


export const TextHolder = styled(Box)({
    border: '1px solid #ddd',
    padding: 2,
    minHeight: '150px',
    width: '100%',
    borderRadius: 1,
    fontSize: 16,
    lineHeight: 1.6,
    whiteSpace: 'pre-wrap',
    wordWrap: 'break-word',
});

export const ButtonContainer = styled(Box)({
    marginTop: 3,
    textAlign: 'center',
});


export const IncomingMessagesContainer = styled('div')({
    width: '100%',                // Konténer szélessége
    maxWidth: '900px',            // Maximális szélesség beállítása (igény szerint változtatható)
    margin: '1em auto',           // Középre igazított konténer
    padding: '0 1em',             // Padding a konténer körül
    maxHeight: '500px',           // Maximális magasság, hogy görgethető legyen
    overflowY: 'auto',            // Görgetés engedélyezése, ha túl sok az üzenet
    backgroundColor: '#fff',      // Fehér háttérszín
    borderRadius: '8px',          // Lekerekített sarkok
    boxShadow: '0 2px 10px rgba(0, 0, 0, 0.1)',  // Finom árnyék a konténer körül
});

export const MessageLine = styled('div')({
    maxHeight: '5em',              // Maximális magasság
              // Ha a tartalom túl nagy, elvágja
    display: 'flex',               // Flexbox használata a sorok elrendezésére
    flexDirection: 'row',          // Elem közötti elrendezés
    alignItems: 'center',          // Vertikális központosítás
    padding: '0',              // Padding a sor körül
    cursor: 'pointer',             // Mutató kurzor hoverre
    borderBottom: '1px solid #ddd',// Finom elválasztó vonal a sorok között
    '&:hover': {                   // Hover állapot
        fontWeight: 'bold',         // Betűk vastagítása
        backgroundColor: '#f5f5f5', // Háttérszín világos szürkére
        transition: 'all 0.3s ease',// Simább átmenet
    },
});

export const MessageDetail = styled('div')({
    overflow: 'hidden',            // Ha a tartalom túl nagy, elvágja
    padding: '0.5em',              // Padding a tartalom körül
                          // Minden elem egyenlő szélességet kapjon
    whiteSpace: 'nowrap',          // Ne törjön a szöveg
    textOverflow: 'ellipsis',      // Ha túl hosszú, három ponttal jelzi
    fontSize: '0.9em',             // Kisebb betűméret
    color: '#333',                 // Szöveg színe
    display: 'flex',               // Flexbox biztosítása az alább lévő elemek elrendezésére
    justifyContent: 'flex-start',  // Szöveg igazítása balra
    minWidth: '150px',             // Minimum szélesség biztosítása
});



export const ChosenHeading = styled(Typography)(({ theme }) => ({
    fontSize: '2rem',              // Betűméret beállítása
    fontWeight: 'bold',            // Félkövér betűk
    color: theme.palette.primary.main, // Alap szín a téma szerint
    marginBottom: '1em',           // Alsó margó a szöveg alá
    textAlign: 'center',           // Középre igazított szöveg
    [theme.breakpoints.up('md')]: {
        fontSize: '3rem',          // Nagyobb betűméret közepes képernyőméretek és nagyobbak számára
    },
}));

export const UnreadButton = styled('button')({
    backgroundColor: '#2196f3',  // Kék háttérszín
    color: 'white',              // Fehér szöveg
    padding: '0.5em 1em',        // Padding
    border: 'none',              // Nincs keret
    borderRadius: '4px',         // Lekerekített sarkok
    cursor: 'pointer',           // Mutató kurzor
    fontSize: '0.9em',            // Kis betűméret
    '&:hover': {                  // Hover effekt
        backgroundColor: '#1976d2', // Sötétebb kék a hover alatt
    }
});



export const UnreadMessageDetail = styled('div')({
    overflow: 'hidden',            // Ha a tartalom túl nagy, elvágja
    padding: '0.5em',   
    fontWeight : 'bold',// Padding a tartalom körül
    // Minden elem egyenlő szélességet kapjon
    whiteSpace: 'nowrap',          // Ne törjön a szöveg
    textOverflow: 'ellipsis',      // Ha túl hosszú, három ponttal jelzi
    fontSize: '0.9em',             // Kisebb betűméret
    color: '#333',                 // Szöveg színe
    display: 'flex',               // Flexbox biztosítása az alább lévő elemek elrendezésére
    justifyContent: 'flex-start',  // Szöveg igazítása balra
    minWidth: '150px',             // Minimum szélesség biztosítása
});
