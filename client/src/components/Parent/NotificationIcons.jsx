import Box from "@mui/material/Box";
import Badge from "@mui/material/Badge";
import AssignmentIcon from "@mui/icons-material/Assignment";
import BackpackIcon from "@mui/icons-material/Backpack";
import HomeWorkIcon from "@mui/icons-material/HomeWork";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import Typography from "@mui/material/Typography";
import React from "react";

const NotificationIconItem = ({ count, onClick, dataValue, Icon, label }) => (
    <Box display="flex" flexDirection="row" alignItems="center" sx={{ textAlign: 'center', margin: 1 }}>
        <Badge
            badgeContent={count}
            sx={{
                '& .MuiBadge-dot': {
                    backgroundColor: 'red',
                },
                '& .MuiBadge-standard': {
                    backgroundColor: 'red',
                    color: 'white',
                    borderRadius: '50%',
                    padding: '0 4px',
                },
            }}
        >
            <Box onClick={onClick} data-value={dataValue} sx={{ cursor: 'pointer', padding: 0 }}>
                <Icon sx={{ fontSize: '4em' }} />
            </Box>
        </Badge>
        <Typography variant="body1" sx={{ marginLeft: 1 }}>
            {label}
        </Typography>
    </Box>
);

function NotificationIcons({ exams, homeworks, missingEquipments, others, onClick }) {
    const clickHandler = (e) => {
        const chosen = e.currentTarget.getAttribute("data-value");
        onClick(chosen);
    };

    return (
        <Box
            display="flex"
            flexDirection="column"
            justifyContent="space-around"
            alignItems="flex-start"
            p={4}
        >
            <NotificationIconItem
                count={exams.length}
                onClick={clickHandler}
                dataValue={"exams"}
                Icon={AssignmentIcon}
                label="Vizsgák"
            />
            <NotificationIconItem
                count={missingEquipments.length}
                onClick={clickHandler}
                dataValue={"missingEquipments"}
                Icon={BackpackIcon}
                label="Hiányzó Eszközök"
            />
            <NotificationIconItem
                count={homeworks.length}
                onClick={clickHandler}
                dataValue={"homeworks"}
                Icon={HomeWorkIcon}
                label="Házi Feladatok"
            />
            <NotificationIconItem
                count={others.length}
                onClick={clickHandler}
                dataValue={"others"}
                Icon={InfoOutlinedIcon}
                label="Egyéb Értesítések"
            />
        </Box>
    );
}

export default NotificationIcons;
