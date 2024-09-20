import React from 'react';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './index.css';
import Students from "./admin/Students.jsx";
import Teachers from "./admin/Teachers.jsx";
import ParentStarter from "./parent/Starter.jsx";
import AdminStarter from "./admin/Starter.jsx";
import TeacherStarter from "./teacher/Starter.jsx";
import TeacherAdding from "./admin/TeacherAdding.jsx";
import StudentAdding from "./admin/StudentAdding.jsx";
import Classes from "./admin/Classes.jsx";
import ClassAdding from "./admin/ClassAdding.jsx";
import Teacher from "./admin/Teacher.jsx";





createRoot(document.getElementById('root')).render(
    <StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path='/parent/:id' element={<ParentStarter />} />
                <Route path='/admin' element={<AdminStarter/>}/>
                <Route path='/teacher' element={<TeacherStarter/>}/>
                <Route path='/students' element={<Students/>}/>
                <Route path='/teachers' element={<Teachers/>}/>
                <Route path='/add-teacher' element={<TeacherAdding/>}/>
                <Route path='/add-student' element={<StudentAdding/>}/>
                <Route path='/classes' element={<Classes/>}/>
                <Route path='/add-class' element={<ClassAdding/>}/>
                <Route path='/teachers/:id' element={<Teacher />} />
               








            </Routes>
        </BrowserRouter>
    </StrictMode>,
)