import { useTheme } from '@mui/material/styles';
import Box from '@mui/material/Box';

import Grid from "@mui/material/Unstable_Grid2";
import useMediaQuery from '@mui/material/useMediaQuery';
import ItemDemoImageColorized from './ItemDemoImageColorized';
import imgDemo2 from './imgDemo2.webp';
import imgDemo1O from './demo1Original.jpg';
import imgDemo1C from './demo1Colorized.jpg';
import imgDemo3O from './demo3O.jpg';
import imgDemo3C from './demo3C.jpg';
const itemDemos = [
    {
        label: 'Tạo sức sống mới cho những bức ảnh cũ',
        description: 'Tô màu cho các bức ảnh đen trắng cũ, tái tạo và khôi phục các bức ảnh về gia đình hoặc nhân vật lịch sử của bạn, và đưa quá khứ trở lại trong mắt bạn với những màu sắc rực rỡ.',
        images: [
            imgDemo1O, imgDemo1C
        ]
    },
    {
        label: 'Thêm màu sắc tự nhiên và chân thực',
        description: 'Dễ dàng thêm màu sắc tự nhiên, chân thực vào các bức ảnh đen trắng cũ không còn là một thách thức. Không giống như các bộ lọc thông thường, đây là bộ chỉnh màu hình ảnh dựa trên các thuật toán tô màu hình ảnh AI, học sâu và khối lượng dữ liệu sẽ xử lý mọi thứ một cách dễ dàng.',
        images: [
            imgDemo2
        ]
    },
    {
        label: 'Trình chỉnh màu ảnh hữu ích và miễn phí',
        description: 'Công cụ chỉnh màu ảnh trực tuyến XWay hoàn toàn miễn phí được sử dụng không có giới hạn, bạn có thể biến ảnh đen trắng thành ảnh màu chỉ trong vài giây mà không cần bất kỳ kỹ năng nào.',
        images: [
            imgDemo3O, imgDemo3C
        ]
    }
];

function DemoImageColorized() {
    const theme = useTheme();
    const matches = useMediaQuery(theme.breakpoints.up('md'));


    return (

        <Box sx={{ width: "100%", flexGrow: 1, marginTop: "100px" }}>
            <Grid container columns={2} rowSpacing={{ xs: 10, sm: 10, md: 10, lg: 15, xl: 15 }} columnSpacing={{ xs: 0, sm: 0, md: 4, lg: 5, xl: 5 }} sx={{ paddingTop: matches === true ? 15 : 5, paddingBottom: matches === true ? 20 : 5 }}>


                {itemDemos.map((item, index) => {
                    return (
                        <Grid xs={2} key={index} >
                            <ItemDemoImageColorized itemDemo={item} />
                        </Grid>)
                })}
            </Grid>
        </Box>


    );
}

export default DemoImageColorized;
