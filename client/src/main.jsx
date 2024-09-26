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
import TeacherMain from "./admin/TeacherMain.jsx";
import ParentNotificationsMain from "./components/Parent/ParentNotificationsMain.jsx";
import ParentGrades from "./components/Parent/ParentGrades.jsx";





createRoot(document.getElementById('root')).render(
    <StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path='/parent/:id' element={<ParentStarter />} />
                <Route path='/admin' element={<AdminStarter/>}/>
                <Route path='/teacher' element={<TeacherStarter/>}/>
                <Route path='/admin/students' element={<Students/>}/>
                <Route path='/admin/teachers' element={<Teachers/>}/>
                <Route path='/add-teacher' element={<TeacherAdding/>}/>
                <Route path='/add-student' element={<StudentAdding/>}/>
                <Route path='/admin/classes' element={<Classes/>}/>
                <Route path='/add-class' element={<ClassAdding/>}/>
                <Route path='/admin/teachers/:id' element={<TeacherMain />} />
                <Route path='/parent/notifications/:id' element={<ParentNotificationsMain />} />
                <Route path='/parent/grades/:id' element={<ParentGrades />} />
        
               








            </Routes>
        </BrowserRouter>
    </StrictMode>,
)