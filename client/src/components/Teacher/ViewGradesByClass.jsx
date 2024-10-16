import react, {useState, useEffect} from "react";
import {
    ListItem,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
    Typography
} from "@mui/material";
import {
    PopupContainer, StyledButton,
    StyledTableCell,
    StyledTableHead,
} from "../../../StyledComponents.js";
import GradesByClass from "./GradesByClass.jsx";
function ViewGradesByClass({teacherId, onGoBack}){
    
    const [chosenClassId, setChosenClassId] = useState("")
    
    const [classes, setClasses] = useState([])
    
    useEffect(()=>{
        fetch(`/api/classes`)
            .then(response=>response.json())
            .then(data=> {
                setClasses(data)
            })
            .catch(error=>console.error(`Error:`,error))
    },[])

    const classClickHandler=(e)=>{
        setChosenClassId(e.target.id)        
    }
    
    const noneClassIdHandler=()=>{
        setChosenClassId("")
    }

 const goBackHandler=()=>{
        setChosenClassId("")
 }
 
 const backFromListHandler=()=>{
        onGoBack()
 }

    return (
        <>
            {chosenClassId !== "" ? (<GradesByClass onGoBack={goBackHandler} classId={chosenClassId}/>) :
                (<><ul>
                    {classes.map((classItem, index) => (
                        <ListItem onClick={classClickHandler} id={classItem.id} key={index}>{classItem.name}</ListItem>
                    ))}
                </ul>
                <StyledButton onClick={backFromListHandler}>Vissza</StyledButton>
                </>)}
          
        </>
    );

}

export default ViewGradesByClass