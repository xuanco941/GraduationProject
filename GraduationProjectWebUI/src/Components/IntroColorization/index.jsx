import * as React from "react";
import Grid from "@mui/material/Unstable_Grid2";
import Box from "@mui/material/Box";
import { Button } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import Typography from '@mui/material/Typography';
import Fab from '@mui/material/Fab';
import NavigationIcon from '@mui/icons-material/Navigation';
import IMGOriginal from './assets/original.jpg'
import IMGColorized from './assets/colorized.jpg'
import { useState, useEffect } from "react";
import { Link } from 'react-router-dom';

export default function IntroColorization() {

  //break point
  const theme = useTheme();
  const matches = useMediaQuery(theme.breakpoints.up('md'));
  const matchesUpLg = useMediaQuery(theme.breakpoints.up('lg'));

  //animation ảnh
  const [imageIndex, setImageIndex] = useState(0);
  const [isAnimating, setIsAnimating] = useState(true);
  useEffect(() => {
    const interval = setInterval(() => {
      if (isAnimating) {
        setImageIndex((prevIndex) => (prevIndex === 0 ? 1 : 0));
      }
    }, 2500);

    return () => {
      clearInterval(interval);
    };
  }, [isAnimating]);

  const handleButtonOnImage = () => {
    if (isAnimating === true) {
      setIsAnimating(false);
    }
    setImageIndex((prevIndex) => (prevIndex === 0 ? 1 : 0));
  };


  return (
    <Box sx={{ width: "100%" }}>
      <Grid container columns={2} rowSpacing={{ xs: 5, sm: 5, md: 5, lg: 0, xl: 0 }} columnSpacing={{ xs: 0, sm: 0, md: 2, lg: 3, xl: 3 }} sx={{ paddingTop: matches === true ? 15 : 5, paddingBottom: matches === true ? 20 : 5 }}>
        {/* Box 1 - Title - Button Chọn ảnh */}
        <Grid xs={matches === true ? 1 : 2}>
          <Box sx={{ height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
            {/*Title*/}
            <Typography variant={matches === true ? 'h2' : 'h4'} gutterBottom textAlign={"center"} fontFamily={"Helvetica"} sx={{ color: "#a371ff" }}>
              Tô màu cho ảnh đen trắng
            </Typography>
            {/*Mô tả*/}

            <Typography variant="subtitle1" gutterBottom fontFamily={"Courier New"} fontSize={matches === true ? 25 : 19} sx={{ textAlign: 'justify' }} >
              Hãy tô màu cho những bức ảnh đen trắng cũ của mình và tái hiện lại những khoảnh khắc quý giá từ quá khứ, bạn có thể biến những hình ảnh kỷ niệm xưa thành những cảnh vật sống động và ngập tràn màu sắc hoàn toàn miễn phí.
            </Typography>

            {/*Button*/}
            <Box paddingLeft={matchesUpLg === true ? '17%' : '4%'} paddingRight={matchesUpLg === true ? '17%' : '4%'} mt={matches === true ? 5 : 2}>
              {

                matches === true ?
                  <Fab variant="extended" color="secondary" sx={{ width: "100%", paddingTop: 4, paddingBottom: 4 }}>
                    <NavigationIcon sx={{ mr: 1, fontSize: 40 }} />
                    <Link style={{ fontSize: 22 }} to="/image-coloring">BẮT ĐẦU NGAY</Link>
                  </Fab>
                  :
                  <Fab variant="extended" color="secondary" sx={{ width: "100%", paddingTop: 3, paddingBottom: 3 }}>
                    <NavigationIcon sx={{ mr: 1, fontSize: 32 }} />
                    <Link style={{ fontSize: 20}} to="/image-coloring">BẮT ĐẦU NGAY</Link>
                  </Fab>
              }

            </Box>

          </Box>
        </Grid>

        {/* Box 2 - Ảnh sau khi tô màu */}
        <Grid xs={matches === true ? 1 : 2} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', textAlign: 'center' }} >
          <Box sx={{ width: "100%" }}>

            <Box sx={{ position: 'relative', borderRadius: '15px' }} >
              <img style={{ borderRadius: '15px', boxShadow: '0px 0px 4px rgba(0, 0, 0, 0.7)' }} onClick={handleButtonOnImage} src={imageIndex === 0 ? IMGOriginal : IMGColorized} width={'100%'} height={'100%'} alt={'colorized'} />
              <Button onClick={handleButtonOnImage} variant="contained" sx={{ position: 'absolute', top: 0, left: '50%', transform: 'translateX(-50%)', opacity: 0.8, backgroundColor: '#00000033' }}>
                {imageIndex === 0 ? 'Trước' : 'Sau'}
              </Button>
            </Box>

          </Box>
        </Grid>
      </Grid>
    </Box>
  );
}
