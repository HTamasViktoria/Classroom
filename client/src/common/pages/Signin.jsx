import React, { useState } from 'react';
import { useProfile } from "../../contexts/ProfileContext.jsx";
import { useNavigate } from 'react-router-dom';
import { StyledInput, AButton, ErrorMessage, StyledForm } from "../../../StyledComponents";

function Signin() {
    const navigate = useNavigate();
    const { setProfile, login } = useProfile();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/api/auth/signin', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });

         
            if (!response.ok) {
             
                setError('Hibás bejelentkezési adatok! Kérjük, ellenőrizze a hitelesítő adatokat és próbálkozzon újra.');
                return;
            }

            const data = await response.json();
   
            setProfile(data);
            login(data);
            navigateBasedOnRole(data);

        } catch (error) {
            console.error('Error during sign-in:', error);
   
            setError('Hibás bejelentkezési adatok! Kérjük, ellenőrizze a hitelesítő adatokat és próbálkozzon újra.');
        }
    };

    const navigateBasedOnRole = (data) => {

        const role = data.user.role;
        const id = data.user.id;
        let studentId = "";
        if(role === "Parent"){
            studentId = data.user.student.id;
        }
        const roleRoutes = {
            Admin: '/admin',
            Parent: `/parent/${studentId}`,
            Student: `/student/${studentId}`,
            Teacher: `/teacher/${id}`,
        };

        const route = roleRoutes[role];

        if (route) {
          
        navigate(route)
        }
        else console.log("Nincs ilyen role");
    };

    return (
        <div>
            <h2>Bejelentkezés</h2>
            {error && <ErrorMessage>{error}</ErrorMessage>}
            <StyledForm onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                    <StyledInput
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Jelszó:</label>
                    <StyledInput
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <AButton type="submit">Bejelentkezés</AButton>
            </StyledForm>
        </div>
    );
}

export default Signin;
