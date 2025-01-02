import { useState, useEffect } from "react";

function AviableCarsView()
{
    const [carsToDisplay, setCarsToDisplay] = useState([]);
    const [carStatus, setCarStatus] = useState([]);
    const [filterStatus, setFilterStatus] = useState([]);
    const [searchString, setSearchString] = useState("");
    const [carsFiltered, setCarsFiltered] = useState([]);


    const fetchCarStatuses = async () =>
    {
        const response = await fetch("api/CarStatus/allStatuses");
        if (response.ok)
        {
            const jsonData = await response.json();
            setCarStatus(jsonData);
        }
    };

    const fetchCarList = async () =>
    {
        const cars = [];
        for (const filter of filterStatus)
        {
            const response = await fetch(`api/CarStatus/cars/${filter}`);
            if (response.ok)
            {
                const jsonData = await response.json();
                cars.unshift(jsonData);
            }
        }
        setCarsFiltered(cars.flat());
    };


    useEffect(() =>
    {
        const filtered = carsFiltered.filter((car) =>
        {
            const matchesSearch =
                searchString === "" ||
                car.brand.toLowerCase().includes(searchString.toLowerCase()) ||
                car.model.toLowerCase().includes(searchString.toLowerCase());
            return matchesSearch;
        });
        setCarsToDisplay(filtered);
    }, [carsFiltered, searchString]);


    useEffect(() =>
    {
        fetchCarList();
    }, [filterStatus]);


    useEffect(() =>
    {
        fetchCarStatuses();
    }, []);

    const handleSearchResults = (e) =>
    {
        setSearchString(e.target.value);
    };

    const handleChangeFilter = (e) =>
    {
        if (e.target.checked)
        {
            setFilterStatus([...filterStatus, e.target.value]);
        } else
        {
            setFilterStatus(filterStatus.filter((status) => status !== e.target.value));
        }
    };

    return (
        <div className="container">
            <div className="section">
                <h1>Car List</h1>
                <input
                    type="text"
                    placeholder="Search..."
                    className="searchInput"
                    onChange={handleSearchResults}
                />
                <div className="filterSelect">
                    {carStatus.map((status) => (
                        <label key={status}>
                            <input
                                type="checkbox"
                                value={status}
                                onChange={handleChangeFilter}
                            />
                            {status}
                        </label>
                    ))}
                </div>
                <div className="carList">
                    {carsToDisplay.map((car) => (
                        <CarItem key={car.id} car={car} />
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
