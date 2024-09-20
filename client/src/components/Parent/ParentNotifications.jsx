import React, {useEffect, useState} from 'react';
import IconButton from '@mui/material/IconButton';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BackpackIcon from '@mui/icons-material/Backpack';
import HomeWorkIcon from '@mui/icons-material/HomeWork';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import Badge from '@mui/material/Badge';
import Box from '@mui/material/Box';

function ParentNotifications(props) {
    
    const [homeworks, setHomeWorks] = useState([])
    const [exams, setExams] = useState([])
    const [missingEquipments, setMissingEquipmentes] = useState([])
    const [others, setOthers] = useState([])
    const handleClick = (iconName) => {
        console.log(`${iconName} icon clicked`);
    };
    
    useEffect(()=>{
       const examsArray =  props.notifications.filter(n=> n.type === "Exam");
       setExams(examsArray);
       
       const homeworksArray = props.notifications.filter(n => n.type === "Homework");
       setHomeWorks(homeworksArray);
       
       const missingEquipmentsArray = props.notifications.filter(n=>n.type==="MissingEquipment");
       setMissingEquipmentes(missingEquipmentsArray);
       
       const othersArray = props.notifications.filter(n=>n.type === "OtherNotifications");
       setOthers(othersArray);
       
    }, [props])

    return (
        <Box
            display="flex"
            justifyContent="space-around"
            alignItems="center"
            p={2}
        >
            <Box position="relative">
                <Badge badgeContent={exams.length} color="secondary">
                    <IconButton onClick={() => handleClick('Assignment')} sx={{ fontSize: 40 }}>
                        <AssignmentIcon />
                    </IconButton>
                </Badge>
            </Box>
            <Box position="relative">
                <Badge badgeContent={missingEquipments.length} color="secondary">
                    <IconButton onClick={() => handleClick('Backpack')} sx={{ fontSize: 40 }}>
                        <BackpackIcon />
                    </IconButton>
                </Badge>
            </Box>
            <Box position="relative">
                <Badge badgeContent={homeworks.length} color="secondary">
                    <IconButton onClick={() => handleClick('HomeWork')} sx={{ fontSize: 40 }}>
                        <HomeWorkIcon />
                    </IconButton>
                </Badge>
            </Box>
            <Box position="relative">
                <Badge badgeContent={others.length} color="secondary">
                    <IconButton onClick={() => handleClick('Notifications')} sx={{ fontSize: 40 }}>
                        <InfoOutlinedIcon />
                    </IconButton>
                </Badge>
            </Box>
        </Box>
    );
}

export default ParentNotifications;
