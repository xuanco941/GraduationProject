import * as React from "react";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Common from "../../Common.js";
import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import ColoringAImage from "../../Components/ColoringAImage/index.jsx";
const ImageColoring = () => {

    const theme = useTheme();
    const matchesUpMd = useMediaQuery(theme.breakpoints.up('md'));


    return (
        <React.Fragment>
            <CssBaseline />
            <Box>
                <Container maxWidth="lg">
                    <Box sx={{ bgcolor: Common.colors.backgroundColorHome, marginTop: matchesUpMd === true ? '82px' : '66px' }}>
                        <ColoringAImage></ColoringAImage>
                    </Box>
                </Container>

            </Box>

        </React.Fragment>
    )
}


export default ImageColoring;