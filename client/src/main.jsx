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
import Parents from "./admin/Parents.jsx";
import ParentsOfAStudent from "./components/Admin/ParentsOfAStudent.jsx";
import ParentProfile from "./components/Parent/ParentProfile.jsx";
import TeacherGrades from "./teacher/TeacherGrades.jsx";
import Notifications from "./components/Teacher/Notifications.jsx";
import TeacherViewingGrades from "./teacher/TeacherViewingGrades.jsx";
import GradeAddingForm from "./teacher/GradeAddingForm.jsx";
import BulkGradeAdding from "./teacher/BulkGradeAdding.jsx";
import NotificationMain from "./components/Teacher/NotificationMain.jsx";
import TeachersLastNotifications from "./components/Teacher/TeachersLastNotifications.jsx";
import TeacherMessages from "./teacher/TeacherMessages.jsx";


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
                        <Route path='/admin/parents/add-parent/:id' element={<ProtectedRoute element={<ParentAdding />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/parentsOf/:id' element={<ProtectedRoute element={<ParentsOfAStudent />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/teachers/:id' element={<ProtectedRoute element={<TeacherMain />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/students' element={<ProtectedRoute element={<Students />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/teachers' element={<ProtectedRoute element={<Teachers />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/parents' element={<ProtectedRoute element={<Parents />} allowedRoles={['Admin']} />} />
                        <Route path='/add-teacher' element={<ProtectedRoute element={<TeacherAdding />} allowedRoles={['Admin']} />} />                       
                        <Route path='/add-student' element={<ProtectedRoute element={<StudentAdding />} allowedRoles={['Admin']} />} />
                        <Route path='/admin/classes' element={<ProtectedRoute element={<Classes />} allowedRoles={['Admin']} />} />
                        <Route path='/add-class' element={<ProtectedRoute element={<ClassAdding />} allowedRoles={['Admin']} />} />                      
                        <Route path='/admin' element={<ProtectedRoute element={<AdminStarter />} allowedRoles={['Admin']} />} />
                        {/* Szülő védett útvonalak */}
                        <Route path='/parent/:id' element={<ProtectedRoute element={<ParentStarter />} allowedRoles={['Parent']} />} />
                        <Route path='/parent/grades/:id' element={<ProtectedRoute element={<ParentGrades />} allowedRoles={['Parent']} />} />
                        <Route path='/parent/notifications/:id' element={<ProtectedRoute element={<ParentNotificationsMain />} allowedRoles={['Parent']} />} />
                        <Route path='/parent/profile/:id' element={<ProtectedRoute element={<ParentProfile />} allowedRoles={['Parent']} />} />
                        {/* Tanár védett útvonalak */}
                        <Route path='/teacher/:id' element={<ProtectedRoute element={<TeacherStarter />} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/grades/:id' element={<ProtectedRoute element={<TeacherGrades />} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/messages/:id' element={<ProtectedRoute element={<TeacherMessages />} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/notifications/:id' element={<ProtectedRoute element={< Notifications/>} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/viewGrades/:id' element={<ProtectedRoute element={< TeacherViewingGrades/>} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/addingOneGrade/:id' element={<ProtectedRoute element={<GradeAddingForm />} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/bulkGradeAdding/:id' element={<ProtectedRoute element={<BulkGradeAdding />} allowedRoles={['Teacher']} />} />

                        <Route path='/teacher/notifalerts/:id' element={<ProtectedRoute element={<TeachersLastNotifications />} allowedRoles={['Teacher']} />} />
                        <Route path='/teacher/notifications/add/:id' element={<ProtectedRoute element={<NotificationMain />} allowedRoles={['Teacher']} />} />
                    </Routes>
                </BrowserRouter>
            </ProfileContextProvider>
        </ThemeProvider>
    </StrictMode>,
);
