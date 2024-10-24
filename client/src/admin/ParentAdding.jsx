import React, { useState } from 'react';
import {useNavigate} from "react-router-dom";

function ParentAdding() {

    const navigate = useNavigate();
    
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [familyName, setFamilyName] = useState('');
    const [childName, setChildName] = useState('');

    const postParent = async (parent) => {
        console.log('Posting parent data:', JSON.stringify(parent, null, 2));
        return fetch('/api/auth/sign-up/parent', {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(parent)
        })
            .then((res) => {
                if (!res.ok) {
                    throw new Error(`HTTP error! status: ${res.status}`);
                }
                return res.json();
            })
            .then((data) => {
                console.log('Server response:', data);
                return data;
            })
            .catch((error) => {
                console.error('Error:', error);
                throw error;
            });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const parentRequest = {
            email,
            username,
            password,
            firstName,
            familyName,
            childName,
        };

        try {
            const response = await postParent(parentRequest);
            console.log('Registration successful:', response);
            navigate("/admin/students")
        } catch (error) {
            console.error('There was an error registering the parent!', error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <label>Email:</label>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Felhasználónév:</label>
                <input
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Jelszó:</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Keresztnév:</label>
                <input
                    type="text"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Vezetéknév:</label>
                <input
                    type="text"
                    value={familyName}
                    onChange={(e) => setFamilyName(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Gyermek neve:</label>
                <input
                    type="text"
                    value={childName}
                    onChange={(e) => setChildName(e.target.value)}
                    required
                />
            </div>
            <button type="submit">Szülő hozzáadása</button>
        </form>
    );
}

export default ParentAdding;
