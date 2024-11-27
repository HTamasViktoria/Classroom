import {AButton, NotifBox, 
    BButton} from "../../../StyledComponents.js";

function NotifButtonContainer({notification, onGoBack, onRefresh}) {


    const setToReadHandler = (e) => {
        const id = e.target.id;
        fetch(`/api/notifications/setToRead/${id}`, {method: 'POST'})
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to set notification to read');
                }
                return response.json();
            })
            .then(() => {
                onRefresh();
                onGoBack();
            })
            .catch(error => console.error('Error setting notification to read:', error));
    };


    const deleteHandler = (e) => {
        const id = e.target.id;
        onButtonClick();

        fetch(`/api/notifications/delete/${id}`, {method: 'DELETE'})
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to delete notification');
                }
                return response.json();
            })
            .then(data => {
                onRefresh();
            })
            .catch(error => console.error('Error deleting notification:', error));
    };


    return (<>
        <NotifBox>
            <BButton
                onClick={() => onGoBack()}
                id={notification.id}
            >
                Erre még visszatérek
            </BButton>


            {notification.read === false ? (
                <AButton
                    onClick={setToReadHandler}
                    id={notification.id}
                >
                    Ok, elolvastam
                </AButton>
            ) : (<AButton
                id={notification.id}
                onClick={deleteHandler}
            >
                Értesítés törlése
            </AButton>)}
        </NotifBox>

    </>)
}

export default NotifButtonContainer