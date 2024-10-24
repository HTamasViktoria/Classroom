import React from 'react';
import { useNavigate } from 'react-router-dom';

function Starter() {
    const navigate = useNavigate();

    const clickHandler = () => {
        navigate("/signin");
    };

    return (
        <>
            <button onClick={clickHandler}>Bejelentkezés</button>
        </>
    );
}

export default Starter;
