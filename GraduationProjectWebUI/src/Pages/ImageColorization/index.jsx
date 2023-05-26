import * as React from "react";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";

import Common from '../../Common'

import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';

import IntroColorization from "../../Components/IntroColorization";
import Bgr from "./bgr.jpg"
import StepByStep from "../../Components/StepByStep";
import DemoImageColorized from "../../Components/DemoImageColorized";
import BeforeFooter from "../../Components/BeforeFooter";
const ImageColorization = () => {

    const theme = useTheme();
    const matchesUpMd = useMediaQuery(theme.breakpoints.up('md'));


    return (
        <React.Fragment>
            <CssBaseline />
            <Box sx={{
                backgroundImage: `url(${Bgr})`,
                backgroundSize: "cover",
                backgroundPosition: "center",
                backgroundRepeat: "repeat"
            }}>
                <Container maxWidth="xl">
                    <Box sx={{ bgcolor: Common.colors.backgroundColorHome, marginTop: matchesUpMd === true ? '82px' : '66px' }}>
                        <IntroColorization />
                        <StepByStep title={`Cách tô màu ảnh đen trắng trong ${Common.appName}`} steps={['Chọn tệp ảnh bằng cách tải lên.', 'Chờ trong giây lát và ảnh của bạn sẽ được tô màu tự động.', 'Lưu các bức ảnh được tô màu miễn phí.']} />
                    </Box>
                </Container>
                <Container maxWidth='md'>
                    <DemoImageColorized />
                </Container>

            </Box>
                <BeforeFooter title="Bạn đã sẵn sàng thêm màu vào ảnh đen trắng chưa?" link="/image-coloring" />



        </React.Fragment>
    )
}


export default ImageColorization