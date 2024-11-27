import { useEffect } from "react";

function LatestGradeFetcher({ teacherId, onData }) {
    useEffect(() => {


        fetch(`/api/grades/teachersLast/${teacherId}`)
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
               
                onData(data);
            })
            .catch(error => {
                console.error("Error fetching grade:", error);
            });
    }, [teacherId]);

    return null;
}

export default LatestGradeFetcher;
