import { CustomBox, AButton, BButton } from "../../../StyledComponents.js";
import { Typography, Box, Button } from "@mui/material";
import React from "react";

function OneMessageViewer({ message, onGoBack }) {
    return (
        <CustomBox>
            <Box sx={{
                padding: 3,
                maxWidth: '700px',
                margin: '0 auto',
                boxShadow: 3,
                borderRadius: 2,
                backgroundColor: '#fff',
            }}>
                {/* Üzenet információk */}
                <Box sx={{ marginBottom: 2 }}>
                    <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: 1 }}>
                        Dátum:
                    </Typography>
                    <Typography variant="body1" sx={{ marginBottom: 2 }}>
                        {message.date.substring(0, 10)}
                    </Typography>
                </Box>

                <Box sx={{ marginBottom: 2 }}>
                    <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: 1 }}>
                        Feladó:
                    </Typography>
                    <Typography variant="body1" sx={{ marginBottom: 2 }}>
                        {message.senderName}
                    </Typography>
                </Box>

                <Box sx={{ marginBottom: 2 }}>
                    <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: 1 }}>
                        Tárgy:
                    </Typography>
                    <Typography variant="body1" sx={{ marginBottom: 2 }}>
                        {message.headText}
                    </Typography>
                </Box>

                <Box sx={{ marginBottom: 2 }}>
                    <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: 1 }}>
                        Üzenet szövege:
                    </Typography>
                    <Box
                        sx={{
                            border: '1px solid #ddd',
                            padding: 2,
                            minHeight: '150px',
                            width: '100%',
                            borderRadius: 1,
                            fontSize: 16,
                            lineHeight: 1.6,
                            whiteSpace: 'pre-wrap',
                            wordWrap: 'break-word',
                        }}
                    >
                        {message.text}
                    </Box>
                </Box>
                <Box sx={{ marginTop: 3, textAlign: 'center' }}>
                    <BButton>
                        Válasz
                    </BButton>
                    <AButton>
                        Törlés
                    </AButton>
                    <AButton onClick={()=>onGoBack()}>Vissza</AButton>
                </Box>
            </Box>
        </CustomBox>
    );
}

export default OneMessageViewer;
