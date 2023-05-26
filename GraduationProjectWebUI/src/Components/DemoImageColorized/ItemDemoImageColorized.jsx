import MobileStepper from '@mui/material/MobileStepper';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import KeyboardArrowLeft from '@mui/icons-material/KeyboardArrowLeft';
import KeyboardArrowRight from '@mui/icons-material/KeyboardArrowRight';
import SwipeableViews from 'react-swipeable-views';
import { autoPlay } from 'react-swipeable-views-utils';
import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Box from '@mui/material/Box';
import * as React from 'react';

const AutoPlaySwipeableViews = autoPlay(SwipeableViews);

export default function ItemDemoImageColorized(props) {
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));
    const [activeStep, setActiveStep] = React.useState(0);
    const maxSteps = props.itemDemo.images.length;

    const handleNext = () => {
        setActiveStep((prevActiveStep) => prevActiveStep + 1);
    };

    const handleBack = () => {
        setActiveStep((prevActiveStep) => prevActiveStep - 1);
    };

    const handleStepChange = (step) => {
        setActiveStep(step);
    };
    return (
        <Box sx={{ boxShadow: '0px 0px 5px rgba(0, 0, 0, 0.9)', borderRadius: '10px' }}>
            <Box sx={{ paddingLeft: '6px', paddingRight: '6px', paddingTop: '20px', paddingBottom: '13px' }}>
                <Typography variant={matches === true ? 'h5' : 'h6'} gutterBottom textAlign={"center"} fontFamily={"Helvetica"} sx={{ color: "#333333", fontWeight: 'bold' }}>
                    {props.itemDemo.label}
                </Typography>
                <Typography>{props.itemDemo.description}</Typography>
            </Box>
            <AutoPlaySwipeableViews
                axis={theme.direction === 'rtl' ? 'x-reverse' : 'x'}
                index={activeStep}
                onChangeIndex={handleStepChange}
                enableMouseEvents
            >
                {props.itemDemo.images.map((image, index) => (
                    <div key={index}>
                        {Math.abs(activeStep - index) <= 2 ? (
                            <Box
                                component="img"
                                sx={{
                                    width: '100%',
                                    display: 'block',
                                    maxWidth: '100%',
                                    overflow: 'hidden',
                                }}
                                src={image}
                                alt={image}
                            />
                        ) : null}
                    </div>
                ))}
            </AutoPlaySwipeableViews>
            {props.itemDemo.images.length > 1 ? <MobileStepper
                sx={{ backgroundColor: 'transparent' }}
                steps={maxSteps}
                position="static"
                activeStep={activeStep}
                nextButton={
                    <Button
                        size="small"
                        onClick={handleNext}
                        disabled={activeStep === maxSteps - 1}
                    >
                        Sau
                        {theme.direction === 'rtl' ? (
                            <KeyboardArrowLeft />
                        ) : (
                            <KeyboardArrowRight />
                        )}
                    </Button>
                }
                backButton={
                    <Button size="small" onClick={handleBack} disabled={activeStep === 0}>
                        {theme.direction === 'rtl' ? (
                            <KeyboardArrowRight />
                        ) : (
                            <KeyboardArrowLeft />
                        )}
                        Trước
                    </Button>
                }
            /> : <></>}

        </Box>)
}