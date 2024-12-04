import {useState, useEffect} from "react";
import {ListItem} from "@mui/material";
import {AButton} from '../../../StyledComponents.js';
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


    return (
        <>
            {chosenClassId === "" ? (
                <>
                    <ul>
                        {classes.map((classItem, index) => (
                            <ListItem
                                onClick={(e) => setChosenClassId(e.target.id)}
                                id={classItem.id}
                                key={index}
                            >
                                {classItem.name}
                            </ListItem>
                        ))}
                    </ul>
                    <AButton onClick={() => onGoBack()}>Vissza</AButton>
                </>
            ) : (
                <GradesByClass onGoBack={() => setChosenClassId("")} classId={chosenClassId} />
            )}
        </>
    );


}

export default ViewGradesByClass