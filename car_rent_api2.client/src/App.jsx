import React from 'react';
import AddEditCar from './AddEditCar';
import AviableCarsView from './pages/AviableCarsView'; // Consider renaming to AvailableCarsView
import Nav from './Nav';
import Home from './Home';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Rents from './pages/Rents';

function App()
{
    return (
        <Router>
            <Nav />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/addeditcar" element={<AddEditCar />} />
                <Route path="/aviablecarview" element={<AviableCarsView />} />
                <Route path="/rents" element={<Rents />} />
            </Routes>
        </Router>
    );
}

export default App;
