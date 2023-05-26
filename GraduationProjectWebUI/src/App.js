import { Route, Routes } from 'react-router-dom'
import Header from './Sections/Header'
import ImageColorization from './Pages/ImageColorization'
import Footer from './Sections/Footer'
import ImageColoring from './Pages/ImageColoring'

const App = () => {
    // const navigate = useNavigate();
    return (
        <>
            <Header />

            <Routes>
                <Route path='/' element={<ImageColorization />} />
                <Route path='/image-coloring' element={<ImageColoring />} />
            </Routes>

            <Footer/>

        </>
    )
}


export default App