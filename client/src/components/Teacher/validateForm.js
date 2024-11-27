
const validateForm = (state, type) => {
    if (state.selectedDate === "") return "Dátum megadása kötelező!";
    if (state.description === "") return "Leírás megadása kötelező!";
    if (state.students.length === 0) return "Legalább egy diák megadása kötelező!";
    if (["Exam", "MissingEquipment", "Homework"].includes(type) && state.selectedSubjectName === "")
        return "Tantárgy megadása kötelező!";
    return "";
};

export default validateForm;
