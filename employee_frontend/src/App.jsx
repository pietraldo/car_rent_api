import React, { useEffect, useState } from 'react';
import axios from 'axios';

const App = () =>
{
    // State to hold the list of cars
    const [cars, setCars] = useState([]);

    // Fetch cars from the API when the component mounts
    useEffect(() =>
    {
        const fetchCars = async () =>
        {
            try
            {
                const response = await axios.get('https://carrentapi3-hrgxawavedbqg0g4.polandcentral-01.azurewebsites.net/Car');
                setCars(response.data);  // Update state with the fetched cars
            } catch (error)
            {
                console.error('Error fetching cars:', error);
            }
        };

        fetchCars();
    }, []); // Empty dependency array means this runs once on mount

    return (
        <div>
            <h1>Car List</h1>
            <ul>
                {cars.map((car) => (
                    <li key={car.id}>
                        {car.brand} {car.name} - {car.color} ({car.year})
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default App;
