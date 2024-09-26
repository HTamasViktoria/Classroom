import { Button, Stack, FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import React from "react";

function AddingTeacherSubjectForm({ teacher, subjects, classes, selectedSubject, selectedClass, handleSubjectClick, handleClassClick, handleSubmit }) {
    return (
        <Stack spacing={4} sx={{ width: '100%' }}>
            <div>
                <h2>{`Tantárgy hozzáadása ${teacher.familyName} ${teacher.firstName} tanárhoz`}</h2>

                <Stack direction="row" spacing={6}>
                    <FormControl variant="outlined" sx={{ minWidth: 250 }}>
                        <InputLabel id="select-subject-label">Tantárgy:</InputLabel>
                        <Select
                            labelId="select-subject-label"
                            value={selectedSubject || ""}
                            onChange={(e) => handleSubjectClick(e.target.value)}
                            label="Select Subject"
                        >
                            {subjects.length > 0 ? (
                                subjects.map((subject) => (
                                    <MenuItem key={subject} value={subject}>
                                        {subject}
                                    </MenuItem>
                                ))
                            ) : (
                                <MenuItem disabled>No subjects available</MenuItem>
                            )}
                        </Select>
                    </FormControl>
                    <FormControl variant="outlined" sx={{ minWidth: 250 }}>
                        <InputLabel id="select-class-label">Osztály:</InputLabel>
                        <Select
                            labelId="select-class-label"
                            value={selectedClass ? selectedClass.id : ""}
                            onChange={(e) => handleClassClick(classes.find(cls => cls.id === e.target.value))}
                            label="Select Class"
                        >
                            {classes.length > 0 ? (
                                classes.map((cls) => (
                                    <MenuItem key={cls.id} value={cls.id}>
                                        {cls.name}
                                    </MenuItem>
                                ))
                            ) : (
                                <MenuItem disabled>No classes available</MenuItem>
                            )}
                        </Select>
                    </FormControl>
                </Stack>
            </div>
            <Button
                variant="contained"
                sx={{
                    width: '60%',
                    backgroundColor: '#d9c2bd',
                    color: '#fff',
                    '&:hover': {
                        backgroundColor: '#c2a6a0',
                    },
                    alignSelf: 'flex-end'
                }}
                onClick={handleSubmit}
            >
                Hozzáad
            </Button>
        </Stack>
    );
}

export default AddingTeacherSubjectForm;
