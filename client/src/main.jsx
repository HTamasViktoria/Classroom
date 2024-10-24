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
import ParentGrades from "./parent/ParentGrades.jsx";
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import theme from "../theme.js";
import Starter from "./common/pages/Starter.jsx";
import Signin from "./common/pages/Signin.jsx";
import ParentAdding from "./admin/ParentAdding.jsx";
import { ProfileContextProvider } from "./contexts/ProfileContext.jsx";
import ProtectedRoute from "./contexts/ProtectedRoute.jsx";

const root = createRoot(document.getElementById('root'));
root.render(
    <StrictMode>
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <ProfileContextProvider>
                <BrowserRouter>
                    <Routes>
                        {/* Nyilvános útvonalak */}
                        <Route path='/' element={<Starter />} />
                        <Route path='/signin' element={<Signin />} />

                        {/* Admin útvonalak */}
                        <Route path='/admin' element={<ProtectedRoute element={<AdminStarter />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/students' element={<ProtectedRoute element={<Students />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/teachers' element={<ProtectedRoute element={<Teachers />} allowedRoles={['Admin']} />} />
                        <Route path='/add-teacher' element={<ProtectedRoute element={<TeacherAdding />} allowedRoles={['Admin']} />} />
                        <Route path='/add-parent' element={<ProtectedRoute element={<ParentAdding />} allowedRoles={['Admin']} />} />
                        <Route path='/add-student' element={<ProtectedRoute element={<StudentAdding />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/classes' element={<ProtectedRoute element={<Classes />} allowedRoles={['Admin']} />} />
                        <Route path='/add-class' element={<ProtectedRoute element={<ClassAdding />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/teachers/:id' element={<ProtectedRoute element={<TeacherMain />} allowedRoles={['Admin']} />} />

                        {/* Szülő védett útvonalak */}
                        <Route path='/parent/:id' element={<ProtectedRoute element={<ParentStarter />} allowedRoles={['Parent']} />} />
                        <Route path='/parent/grades/:id' element={<ProtectedRoute element={<ParentGrades />} allowedRoles={['Parent']} />} />
                        <Route path='/parent/notifications/:id' element={<ProtectedRoute element={<ParentNotificationsMain />} allowedRoles={['Parent']} />} />

                        {/* Tanár védett útvonalak */}
                        <Route path='/teacher/:id' element={<ProtectedRoute element={<TeacherStarter />} allowedRoles={['Teacher']} />} />
                    </Routes>
                </BrowserRouter>
            </ProfileContextProvider>
        </ThemeProvider>
    </StrictMode>,
);
