import { useEffect, useState } from "react";
import ParentReceiverFetcher from "../../components/Teacher/ParentReceiverFetcher.jsx";
import TeacherReceiverFetcher from "./TeacherReceiverFetcher.jsx";
import ParentReceiverSelector from "../../components/Teacher/ParentReceiverSelector.jsx";
import TeacherReceiverSelector from "./TeacherReceiverSelector.jsx";

function ReceiverSelector({ id, onReceiverChange }) {
    const [role, setRole] = useState("");
    const [parents, setParents] = useState([]);
    const [teachers, setTeachers] = useState([]);
    const [selectedParents, setSelectedParents] = useState([]);
    const [selectedTeachers, setSelectedTeachers] = useState([]);

    useEffect(() => {
        const userRole = localStorage.getItem("role");
        setRole(userRole);
    }, []);

    const handleParentSelection = (parentIds) => {
        setSelectedParents(parentIds);
        onReceiverChange([...parentIds, ...selectedTeachers]);
    };

    const handleTeacherSelection = (teacherIds) => {
        if (!Array.isArray(teacherIds)) {
            teacherIds = [teacherIds];
        }
        setSelectedTeachers(teacherIds);
        onReceiverChange([...selectedParents, ...teacherIds]);
    };

    return (
        <>
            {role === "Teacher" && (
                <>
                    <ParentReceiverFetcher onData={(data) => setParents(data)} />
                    <ParentReceiverSelector
                        parents={parents}
                        onSelectionChange={handleParentSelection}
                    />
                </>
            )}
            <TeacherReceiverFetcher id={id} onData={(data) => setTeachers(data)} />
            <TeacherReceiverSelector
                teachers={teachers}
                onSelectionChange={handleTeacherSelection}
            />
        </>
    );
}

export default ReceiverSelector;
