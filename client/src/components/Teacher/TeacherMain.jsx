import React, { useState } from 'react';
import LatestNotifFetcher from "./LatestNotifFetcher.jsx";
import LatestGradeFetcher from "./LatestGradeFetcher.jsx";
import LatestNotifCard from "./LatestNotifCard.jsx";

function TeacherMain({ id }) {
    const [grade, setGrade] = useState(null);
    const [notification, setNotification] = useState(null);



    return (
        <>
            <LatestGradeFetcher teacherId={id} onData={(data) => {
                setGrade(data);
            }} />
            <LatestNotifFetcher teacherId={id} onData={(data) => {
                setNotification(data);
            }} />
         { notification  && (<LatestNotifCard notification={notification} />)}
            <div></div>
        </>
    );
}

export default TeacherMain;
