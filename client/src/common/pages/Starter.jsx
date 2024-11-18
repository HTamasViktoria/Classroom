import React from 'react';
import { useNavigate } from 'react-router-dom';

function Starter() {
    const navigate = useNavigate();



    return (
        <>
            <button onClick={()=>navigate("/signin")}>Bejelentkezés</button>
        </>
    );
}

export default Starter;
