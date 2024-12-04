import { useEffect } from "react";

function ClassAverageFetcher({ studentId, onData }) {

   

    useEffect(() => {
        if (studentId && studentId !== "") {
            fetch(`/api/grades/class-averages/bystudent/${studentId}`)
                .then(response => response.json())
                .then(data => {
                    onData(data);
                })
                .catch(error => console.error(`Error fetching data:`, error));
        }
    }, [studentId]);

    return null;
}

export default ClassAverageFetcher;
