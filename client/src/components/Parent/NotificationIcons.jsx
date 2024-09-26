import Box from "@mui/material/Box";
import Badge from "@mui/material/Badge";
import IconButton from "@mui/material/IconButton";
import AssignmentIcon from "@mui/icons-material/Assignment.js";
import Typography from "@mui/material/Typography";
import BackpackIcon from "@mui/icons-material/Backpack.js";
import HomeWorkIcon from "@mui/icons-material/HomeWork.js";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined.js";
import React from "react";

function NotificationIcons({ exams, homeworks, missingEquipments, others, onClick }) {

    const clickHandler = (e) => {
        const chosen = e.currentTarget.getAttribute("data-value");
        onClick(chosen);
    }

    return (
        <Box
            display="flex"
            flexDirection="column"
            justifyContent="space-around"
            alignItems="flex-start"
            p={4}
        >
            <Box display="flex" flexDirection="row" alignItems="center" sx={{ textAlign: 'center', margin: 1 }}>
                <Badge badgeContent={exams.length} color="secondary">
                    <IconButton onClick={clickHandler} data-value={"exams"} sx={{ fontSize: 60 }}>
                        <AssignmentIcon fontSize="inherit" />
                    </IconButton>
                </Badge>
                <Typography variant="body1">Vizsgák</Typography>
            </Box>

            <Box display="flex" flexDirection="row" alignItems="center" sx={{ textAlign: 'center', margin: 1 }}>
                <Badge badgeContent={missingEquipments.length} color="secondary">
                    <IconButton onClick={clickHandler} data-value={"missingEquipments"} sx={{ fontSize: 60 }}>
                        <BackpackIcon fontSize="inherit" />
                    </IconButton>
                </Badge>
                <Typography variant="body1">Hiányzó Eszközök</Typography>
            </Box>

            <Box display="flex" flexDirection="row" alignItems="center" sx={{ textAlign: 'center', margin: 1 }}>
                <Badge badgeContent={homeworks.length} color="secondary">
                    <IconButton onClick={clickHandler} data-value={"homeworks"} sx={{ fontSize: 60 }}>
                        <HomeWorkIcon fontSize="inherit" />
                    </IconButton>
                </Badge>
                <Typography variant="body1">Házi Feladatok</Typography>
            </Box>

            <Box display="flex" flexDirection="row" alignItems="center" sx={{ textAlign: 'center', margin: 1 }}>
                <Badge badgeContent={others.length} color="secondary">
                    <IconButton onClick={clickHandler} data-value={"others"} sx={{ fontSize: 60 }}>
                        <InfoOutlinedIcon fontSize="inherit" />
                    </IconButton>
                </Badge>
                <Typography variant="body1">Egyéb Értesítések</Typography>
            </Box>
        </Box>
    );
}

export default NotificationIcons;
