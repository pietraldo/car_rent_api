import { useState, useEffect } from "react";

function AviableCarsView()
{
    const [carsToDisplay, setCarsToDisplay] = useState([]);

    const fetchCarList = async () =>
    {
        const response = await fetch('api/Car');
        if (response.ok)
        {
            const jsonData = await response.json();
            setCarsToDisplay(jsonData);
        }
        filterCars();
    };

    const handleSearchResults = (e) =>
    {

    }

    useEffect(() =>
    {
        fetchCarList();
    }, []);

    return (
        <div className="container">
            <div className="section">
                <h1>Car List</h1>
                <input type="text" placeholder="Search..." className="searchInput" onChange={handleSearchResults} />
                <select className="searchSelect" onChange={handleSearchResults}>
                    <option>Brand</option>
                    <option>Model</option>
                </select>
                <div className="carList">
                    {carsToDisplay.map((car) => (
                        <CarItem
                            key={car.id}
                            car={car}
                        />
                    ))}
                </div>
            </div>
        </div>
    );
}

function CarItem({ car })
{
    return (
        <div className="carItem">
            <img src={car.photo} alt="Car" className="carImage" />
            <div className="carDetails">
                <h2 className="carBrand">{car.brand}</h2>
                <p className="carModel">{car.model}</p>
                <p className="carYear">{car.year}</p>
                <p className="carPrice">{car.price}</p>
            </div>
            <div className="carActions">
                <button className="editButton">{car.status}</button>
            </div>
        </div>
    );
}

export default AviableCarsView;