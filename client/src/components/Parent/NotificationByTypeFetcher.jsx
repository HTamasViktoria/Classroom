import { useEffect } from "react";

function NotificationByTypeFetcher({ chosen, onRefreshNeeded, onData }) {
    useEffect(() => {
        fetch(`/api/notifications/${chosen}`)
            .then(response => response.json())
            .then(data => {
                const sortedData = data.sort((a, b) => {
                    if (a.read === false && b.read === true) return -1;
                    if (a.read === true && b.read === false) return 1;
                    return 0;
                });
                onData(sortedData);
            })
            .catch(error => console.error('Error fetching data:', error));
    }, [chosen, onRefreshNeeded]);

    return null;
}

export default NotificationByTypeFetcher;
