import * as React from 'react';
import Box from '@mui/material/Box';
import Stepper from '@mui/material/Stepper';
import Step from '@mui/material/Step';
import StepLabel from '@mui/material/StepLabel';
import { useState, useEffect } from 'react';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from '@mui/material/styles';
import Typography from '@mui/material/Typography';


// const steps = [1,2,3];
export default function StepByStep(props) {
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));

    const [stepIndex, setStepIndex] = useState(0);
    useEffect(() => {
        const interval = setInterval(() => {
            setStepIndex((prevIndex) => {
                if (prevIndex >= props.steps.length) {
                    return 0;
                }
                else {
                    return prevIndex + 1;
                }
            });
        }, 1500);

        return () => {
            clearInterval(interval);
        };
    }, [props.steps]);

    return (

        <Box sx={{ width: '100%', height: '20vh', marginTop: matches ===true ? '80px' : '40px' }}>
            <Typography variant={matches === true ? 'h4' : 'h5'} gutterBottom textAlign={"center"} fontFamily={"Helvetica"} sx={{ color: "#000000", marginBottom: matches ===true ? '50px' : '30px' }}>
                {props.title}
            </Typography>
            <Stepper sx={ matches ===true ? {
                '& .MuiStepIcon-root': {
                    fontSize: '3.5rem'
                },
                '& .css-zpcwqm-MuiStepConnector-root': {
                    top: '26px',
                    left: 'calc(-50% + 29px)',
                    right: 'calc(50% + 29px)'
                },
                '& .css-1hv8oq8-MuiStepLabel-label':{
                    fontSize: matches===true? '1.25rem' : '0.9rem'
                }
            } : {
                '& .MuiStepIcon-root': {
                    fontSize: '3rem'
                },
                '& .css-zpcwqm-MuiStepConnector-root': {
                    top: '24px',
                    left: 'calc(-50% + 27px)',
                    right: 'calc(50% + 27px)'
                }
            }
            
            } activeStep={stepIndex} alternativeLabel>
                {props.steps.map((label) => (
                    <Step key={label} >
                        <StepLabel>{label}</StepLabel>
                    </Step>
                ))}
            </Stepper>
        </Box>
    );
}