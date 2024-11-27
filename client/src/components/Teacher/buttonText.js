
const ButtonText = ({ type }) => {
    switch (type) {
        case "Homework":
            return "Házi feladat hozzáadása";
        case "Other":
            return "Egyéb értesítés hozzáadása";
        case "Exam":
            return "Dolgozat-értesítés hozzáadása";
        case "MissingEquipment":
            return "Felszerelés-hiány hozzáadása";
        default:
            return "Hozzáadás";
    }
};

export default ButtonText;
