import React from 'react';
import IconButton from '@mui/material/IconButton';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BackpackIcon from '@mui/icons-material/Backpack';
import HomeWorkIcon from '@mui/icons-material/HomeWork';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';

import Box from '@mui/material/Box';

function ParentNotifications(props) {
    const handleClick = (iconName) => {
        console.log(`${iconName} icon clicked`);
    };

    return (
        <Box
            display="flex"
            justifyContent="space-around"
            alignItems="center"
            p={2}
        >
            <IconButton onClick={() => handleClick('Assignment')} sx={{ fontSize: 40 }}>
                <AssignmentIcon />
            </IconButton>
            <IconButton onClick={() => handleClick('Backpack')} sx={{ fontSize: 40 }}>
                <BackpackIcon />
            </IconButton>
            <IconButton onClick={() => handleClick('HomeWork')} sx={{ fontSize: 40 }}>
                <HomeWorkIcon />
            </IconButton>
            <IconButton onClick={() => handleClick('Notifications')} sx={{ fontSize: 40 }}>
                <InfoOutlinedIcon />
            </IconButton>
        </Box>
    );
}

export default ParentNotifications;
