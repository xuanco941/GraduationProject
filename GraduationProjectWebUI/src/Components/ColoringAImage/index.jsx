import { useTheme } from '@mui/material/styles';
import Box from '@mui/material/Box';

import Grid from "@mui/material/Unstable_Grid2";
import useMediaQuery from '@mui/material/useMediaQuery';
import Fab from '@mui/material/Fab';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import FileDownloadIcon from '@mui/icons-material/FileDownload';
import Skeleton from '@mui/material/Skeleton';
import Typography from '@mui/material/Typography';
import { useRef, useState, useEffect } from 'react';
const ColoringAImage = () => {
    //break point
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));
    const matchesSM = useMediaQuery(theme.breakpoints.up('sm'));


    const fileInputRef = useRef(null);

    const [imageOrigin, setImageOrigin] = useState(null);
    const [imageClorizedURL, setImageClorizedURL] = useState('');


    const handleButtonClick = () => {
        fileInputRef.current.click();
    };

    const handleFileChange = (event) => {
        const selectedFile = event.target.files[0];
        // Xử lý file 
        selectedFile.preview = URL.createObjectURL(selectedFile);
        setImageOrigin(selectedFile);
    };
    useEffect(() => {
        // clean up image
        return () => {
            if (imageOrigin) {
                URL.revokeObjectURL(imageOrigin.preview);
            }
        }
    }, [imageOrigin])


    return <Box width={'100%'} sx={{ paddingTop: matches === true ? 8 : 5, paddingBottom: matches === true ? 30 : 15 }}>
        <Typography variant={matches === true ? 'h3' : 'h5'} gutterBottom textAlign={"center"} fontFamily={"Arial"} sx={{ color: "#333333", marginBottom: '25px' }}>
            Tô màu cho ảnh đen trắng
        </Typography>
        <Grid container columns={2} sx={{ paddingTop: '10px', paddingBottom: '10px', borderRadius: '15px', boxShadow: '0px 0px 4px rgba(0, 0, 0, 0.7)' }} rowSpacing={{ xs: 3, sm: 3, md: 3, lg: 3, xl: 3 }} columnSpacing={{ xs: 3, sm: 3, md: 3, lg: 3, xl: 3 }} >


            <Grid xs={1} sx={{ textAlign: 'center' }}>
                <Typography variant={matches === true ? 'h6' : 'h7'} gutterBottom textAlign={"center"} fontFamily={"Arial"} sx={{ color: "#333333" }}>
                    Ảnh của bạn
                </Typography>
                <Box sx={{ width: '100%', height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                    {imageOrigin && imageOrigin.preview ? <img style={{boxShadow: '0px 0px 4px rgba(0, 0, 0, 0.7)'}} width={'100%'} src={imageOrigin.preview} alt='img preview' /> : <Skeleton variant="rounded" width={'100%'} height={matches === true ? '500px' : '250px'} />}
                </Box>

            </Grid>


            <Grid xs={1} sx={{ textAlign: 'center' }}>
                <Typography variant={matches === true ? 'h6' : 'h7'} gutterBottom textAlign={"center"} fontFamily={"Arial"} sx={{ color: "#333333" }}>
                    Ảnh sau khi tô màu
                </Typography>
                <Box sx={{ width: '100%', height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                    {imageClorizedURL ? <img style={{boxShadow: '0px 0px 4px rgba(0, 0, 0, 0.7)'}} width={'100%'} src={imageClorizedURL} alt='img preview' /> : <Skeleton variant="rounded" width={'100%'} height={matches === true ? '500px' : '250px'} />}
                </Box>
            </Grid>

            <Grid xs={1} sx={{ marginTop:'10px', display: 'flex', justifyContent: 'center', alignItems: 'center', textAlign: 'center' }}>
                <input
                    type="file"
                    accept="image/*"
                    ref={fileInputRef}
                    style={{ display: 'none' }}
                    onChange={handleFileChange}
                />
                <Fab onClick={handleButtonClick} variant="extended" color='primary' sx={{ fontSize: matchesSM === true ? '17px' : '11px' }}>
                    <FileUploadIcon sx={{ mr: matchesSM === true ? 1 : 0 }} />
                    Tải ảnh lên
                </Fab>
            </Grid>

            <Grid xs={1} sx={{ marginTop:'10px', display: 'flex', justifyContent: 'center', alignItems: 'center', textAlign: 'center' }}>
                <Fab variant="extended" color='primary' sx={{ fontSize: matchesSM === true ? '17px' : '11px' }}>
                    <FileDownloadIcon sx={{ mr: matchesSM === true ? 1 : 0 }} />
                    Tải ảnh xuống
                </Fab>
            </Grid>

        </Grid>
    </Box>
}

export default ColoringAImage;