import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import ParentNavbar from "./ParentNavbar.jsx";
import NotificationIcons from "./NotificationIcons.jsx";
import HomeworkNotifications from "./HomeworkNotifications.jsx";

function ParentNotificationsMain() {
    const { id } = useParams();
    const [allNotifications, setAllNotifications] = useState([]);
    const [homeworks, setHomeWorks] = useState([]);
    const [exams, setExams] = useState([]);
    const [missingEquipments, setMissingEquipmentes] = useState([]);
    const [others, setOthers] = useState([]);
    const [chosen, setChosen] = useState("");
    const [refreshNeeded, setRefreshNeeded] = useState(false);

    useEffect(() => {
        fetchNotifications();
    }, [id, refreshNeeded]);

    const fetchNotifications = () => {
        fetch(`/api/notifications/byStudentId/${id}`)
            .then(response => response.json())
            .then(data => {
                setAllNotifications(data);
                categorizeNotifications(data);
            })
            .catch(error => console.error('Error fetching data:', error));
    };

    const categorizeNotifications = (notifications) => {
        const examsArray = notifications.filter(n => n.type === "Exam" && n.read === false);
        setExams(examsArray);

        const homeworksArray = notifications.filter(n => n.type === "Homework" && n.read === false);
        setHomeWorks(homeworksArray);

        const missingEquipmentsArray = notifications.filter(n => n.type === "MissingEquipment" && n.read === false);
        setMissingEquipmentes(missingEquipmentsArray);

        const othersArray = notifications.filter(n => n.type === "OtherNotifications" && n.read === false);
        setOthers(othersArray);
    };

    const clickHandler = (chosenName) => {
        setChosen(chosenName);
    };

    const refreshHandler = () => {
        setRefreshNeeded(prevState => !prevState);
    };

    return (
        <>
            <ParentNavbar studentId={id} notifications={allNotifications} />
            {chosen === "" && (
                <NotificationIcons
                    exams={exams}
                    homeworks={homeworks}
                    missingEquipments={missingEquipments}
                    others={others}
                    onClick={clickHandler}
                />
            )}
            {chosen === "homeworks" && (
                <HomeworkNotifications homeworks={homeworks} onRefreshing={refreshHandler} />
            )}
        </>
    );
}

export default ParentNotificationsMain;
