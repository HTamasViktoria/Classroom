import { Box, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";

function AdminTeacherList({ teachers }) {
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
                            <TableRow key={teacher.id}>
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
