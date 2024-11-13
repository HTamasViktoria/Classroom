

import {AButton} from "../../../StyledComponents.js";

function SetToReadButton({notification, onRefresh}){


    const setToReadHandler = (e) => {
        const id = e.target.id;
        onRefresh();

        fetch(`/api/notifications/setToRead/${id}`, { method: 'POST' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to set notification to read');
                }
                return response.json();
            })
            .then(() => {
              
                onRefresh();
            })
            .catch(error => console.error('Error setting notification to read:', error));
    };
    
    return(<> 
        <AButton
        onClick={setToReadHandler}
        id={notification.id}
        variant="contained"
        color="primary"
        size="small">
        Ok, elolvastam
    </AButton>
    </>)
}

export default SetToReadButton