import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import ParentNavbar from "../components/Parent/ParentNavbar";
import ParentMain from "../components/Parent/ParentMain";

function Starter() {
    const { id } = useParams();
    const [refreshNeeded, setRefreshNeeded] = useState(false);
    
 

    const refreshHandler = () => {
        setRefreshNeeded((prevState) => !prevState);
    };

    return (
        <>
            <ParentNavbar           
                studentId={id}
                refreshNeeded={refreshNeeded}
            />
            <ParentMain
                studentId={id}
                onRefreshNeeded={refreshHandler}
            />
        </>
    );
}

export default Starter;
