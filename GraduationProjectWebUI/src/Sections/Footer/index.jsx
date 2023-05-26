import * as React from 'react';
import Grid from '@mui/material/Unstable_Grid2';
import Box from '@mui/material/Box';
import useMediaQuery from '@mui/material/useMediaQuery';
import Typography from '@mui/material/Typography';
import { useTheme } from '@mui/material/styles';

const Footer = () => {
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));


    return (
        <Box sx={{ height: '300px', textAlign: "center", backgroundColor: 'black', color:'#fff', paddingTop:'15px' }}>
            <Grid container columns={2} columnSpacing={{ xs: 1, sm: 2, md: 3, lg: 4, xl: 5 }}>
                <Grid xs={2}>
                <Typography variant={matches === true ? 'h2' : 'h4'} gutterBottom textAlign={"center"} fontFamily={"Helvetica"} sx={{ color: "#a371ff" }}>
              XWay
            </Typography>
                </Grid>
                <Grid xs={2}>
                    <Box>
                    
                    </Box>
                </Grid>
                <Grid xs={2}>
                    <Box>2</Box>
                </Grid>

            </Grid>
        </Box>
    )
}


export default Footer