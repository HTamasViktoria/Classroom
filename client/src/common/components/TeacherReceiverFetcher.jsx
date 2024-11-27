import {useEffect} from "react";

function TeacherReceiverFetcher({id, onData}){


    useEffect(() => {
        fetch(`/api/users/getTeacherReceivers`)
            .then((response) => response.json())
            .then((data) => {
                const filteredUsers = data.filter((user) => user.id !== id);
                onData(filteredUsers);
            })
            .catch((error) =>
                console.error("Hiba a tanárok betöltésekor:", error)
            );
    }, [id]);


    return null;
}

export default TeacherReceiverFetcher