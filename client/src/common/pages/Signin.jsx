import React, { useState } from 'react';
import { useProfile } from "../../contexts/ProfileContext.jsx";
import { useNavigate } from 'react-router-dom';

function Signin() {
    const navigate = useNavigate();
    const { setProfile, login } = useProfile();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/api/auth/sign-in', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            if (response.ok) {
                const data = await response.json();
                console.log('Login successful:', data);

              
                setProfile(data);
                login(data);

              
                if (data.role === "Admin") {
                    navigate('/admin');
                } else {
                  
                    if (data.role === "Parent") {
                        navigate(`/parent/${data.studentId}`);
                    } else if (data.role === "Student") {
                        navigate(`/student/${data.studentId}`);
                    } else if(data.role === "Teacher"){
                        navigate(`/teacher/${data.id}`);
                    }else{
                       console.log("Nincs ilyen role")
                    }
                }
            } else {
                throw new Error('Sign-in failed');
            }
        } catch (error) {
            console.error('Error during sign-in:', error);
            setError('Bejelentkezés nem sikerült. Kérjük, ellenőrizze a hitelesítő adatait és próbálkozzon újra.');
        }
    };

    return (
        <div>
            <h2>Bejelentkezés</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                    <input
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Jelszó:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Bejelentkezés</button>
            </form>
        </div>
    );
}

export default Signin;
