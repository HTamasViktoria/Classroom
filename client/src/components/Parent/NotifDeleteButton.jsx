import {AButton} from "../../../StyledComponents.js";

function NotifDeleteButton({notification, onRefresh}){

    const deleteHandler = (e) => {
        const id = e.target.id;
        onRefreshing();

        fetch(`/api/notifications/${id}`, { method: 'DELETE' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to delete notification');
                }
                return response.json();
            })
            .then(data => {
                console.log(data.message);
                setRefreshNeeded(prev => !prev);
            })
            .catch(error => console.error('Error deleting notification:', error));
    };
    
    
    return(<> <AButton
        id={notification.id}
        onClick={deleteHandler}
        variant="contained"
        color="error"
        size="small">
        Értesítés törlése
    </AButton></>)
}

export default NotifDeleteButton