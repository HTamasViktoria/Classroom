import { Box, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";

function AdminStudentList({ students }) {
    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h6" sx={{ marginBottom: 2 }}>
                Diákok
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="students table">
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Családnév</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Keresztnév</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Születési hely</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>Születési idő</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', backgroundColor: '#d9c2bd', fontSize: '1.1rem' }}>OM azonosító</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {students.map((student) => (
                            <TableRow key={student.id}>
                                <TableCell>{student.id}</TableCell>
                                <TableCell>{student.familyName}</TableCell>
                                <TableCell>{student.firstName}</TableCell>
                                <TableCell>{student.birthPlace}</TableCell>
                                <TableCell>{student.birthDate.substring(0, 10)}</TableCell>
                                <TableCell>{student.studentNo}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
}

export default AdminStudentList;
