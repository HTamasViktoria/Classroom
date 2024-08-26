import React from 'react';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';  // A helyes importálás React 18 esetén
import { BrowserRouter, Routes, Route } from 'react-router-dom';  // react-router-dom importálása
import App from './App.jsx';
import './index.css';
import ParentStarter from "./parent/Starter.jsx";
import AdminStarter from "./admin/Starter.jsx"
import TeacherStarter from "./teacher/Starter.jsx"

createRoot(document.getElementById('root')).render(
  <StrictMode>
  <BrowserRouter>
      <Routes>
          <Route path='/parent' element={<ParentStarter/>}/>
          <Route path='/admin' element={<AdminStarter/>}/>
          <Route path='/teacher' element={<TeacherStarter/>}/>
      </Routes>
  </BrowserRouter>
  </StrictMode>,
)
