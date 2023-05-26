import * as React from 'react';
import Box from '@mui/material/Box';
import Bgr from './banner.webp'
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { Link } from 'react-router-dom';


function BeforeFooter(props) {
    const theme = useTheme();
    const matchesLg = useMediaQuery(theme.breakpoints.up('lg'));

    return (
        <Box
            sx={{
                marginTop: matchesLg === true ? '58px' : '37px',
                width: '100%',
                height: 400,
                backgroundColor: 'primary.dark',
                '&:hover': {
                    backgroundColor: 'primary.main',
                    opacity: 0.9,
                },
                backgroundImage: `url(${Bgr})`,
                backgroundSize: "cover",
                backgroundPosition: "center",
                backgroundRepeat: "repeat",
                textAlign: 'center',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                flexDirection: 'column'
            }}
        >
            <Typography variant={matchesLg === true ? 'h4' : 'h5'} gutterBottom textAlign={"center"} fontFamily={"Helvetica"} sx={{ color: "#333333", fontWeight: 'bold' }}>
                {props.title}
            </Typography>
            <Link to={props.link}>
                <Button sx={{ fontSize: matchesLg === true ? '24px' : '20px', mt: 3 }} variant="contained" color="secondary">Bắt đầu bây giờ</Button>
            </Link>


        </Box>);
}

export default BeforeFooter;