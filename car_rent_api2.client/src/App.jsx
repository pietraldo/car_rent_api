import React from 'react';
import AddEditCar from './AddEditCar';
import AviableCarsView from './pages/AviableCarsView'; // Consider renaming to AvailableCarsView
import Nav from './Nav';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

function App()
{
    return (
        <Router>
            <Nav />
            <Routes>
                <Route path="/" element={<AddEditCar />} />
                <Route path="/aviablecarview" element={<AviableCarsView />} />
            </Routes>
        </Router>
    );
}

export default App;
