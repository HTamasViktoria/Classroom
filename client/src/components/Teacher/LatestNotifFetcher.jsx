import { useEffect } from "react";

function LatestNotifFetcher({ teacherId, onData }) {
    useEffect(() => {
        fetch(`/api/notifications/teachersnewest/${teacherId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                if (response.headers.get('content-type')?.includes('application/json')) {
                    return response.json();
                } else {
                    throw new Error("Response is not in JSON format");
                }
            })
            .then(data => {
                if (data === null || (Object.keys(data).length === 0)) {
                    onData(null);
                } else {
                    onData(data);
                }
            })
            .catch(error => {
                console.error("Fetch error:", error);
            });
    }, [teacherId, onData]);

    return null;
}

export default LatestNotifFetcher;
