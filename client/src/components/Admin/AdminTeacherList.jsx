import { Box, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";
import {useNavigate} from "react-router-dom";

function AdminTeacherList({ teachers }) {
    
    const navigate = useNavigate()
    const handleTeacherChoosing = (id) => {
        navigate(`/teachers/${id}`);
    };
    
    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                Tanárok
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="teachers table">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>Családnév</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#b5a58d', fontSize: '1.1rem' }}>Keresztnév</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {teachers.map((teacher) => (
                            <TableRow   onClick={() => handleTeacherChoosing(teacher.id)} key={teacher.id}>
                                <TableCell>{teacher.id}</TableCell>
                                <TableCell>{teacher.familyName}</TableCell>
                                <TableCell>{teacher.firstName}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
}

export default AdminTeacherList;
