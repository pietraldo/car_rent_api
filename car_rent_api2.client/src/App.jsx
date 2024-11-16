import { useEffect, useState } from 'react';
import './App.css';
import CarDetails from './CarDetailsList';
import CarService from './CarServiceList';
import Location from './CarLocation';

function App()
{
    const [data, setData] = useState([]);
    const [search, setSearch] = useState('');
    const [carsToDisplay, setCarsToDisplay] = useState([]);
    const [editingCar, setEditingCar] = useState(null);


    const fetchCarList = async () =>
    {
        const response = await fetch('api/Car');
        if (response.ok)
        {
            const jsonData = await response.json();
            setData(jsonData);
        }
        filterCars();
    };



    useEffect(() =>
    {
        fetchCarList();
    }, []);

    useEffect(() =>
    {
        filterCars();
    }, [data, search]);


    const handleEdit = (car) =>
    {
        setEditingCar(car);
    };

    const handleDelete = async (car) =>
    {
        const response = await fetch(`api/Car/${car.id}`, {
            method: 'DELETE',
        });
        handleCarAddedOrEdited();
    }


    const handleCarAddedOrEdited = () =>
    {
        fetchCarList();
        setEditingCar(null);
        filterCars();
    };

    const filterCars = () =>
    {

        if (search === '')
        {
            setCarsToDisplay(data);
        }
        else
        {
            const filteredCars = data.filter(car =>
                car.brand.toLowerCase().includes(search) ||
                car.model.toLowerCase().includes(search) ||
                car.year.toString().includes(search) ||
                car.price.toString().includes(search)
            );
            setCarsToDisplay(filteredCars);
        }
    }

    const handleSearchResults = async (e) =>
    {
        const search = e.target.value.toLowerCase();
        setSearch(search);
        filterCars();
    }

    return (
        <div className="container">
            <div className="section">
                <h1>{editingCar ? 'Edit Car' : 'Add Car'}</h1>
                <Formularz
                    onCarAddedOrEdited={handleCarAddedOrEdited}
                    editingCar={editingCar}
                    setEditingCar={setEditingCar}
                />
            </div>
            <div className="section">
                <h1>Car List</h1>
                <input type="text" placeholder="Search..." className="searchInput" onChange={handleSearchResults} />
                <div className="carList">
                    {carsToDisplay.map((car) => (
                        <CarItem
                            key={car.id}
                            car={car}
                            onEdit={() => handleEdit(car)}
                            onDelete={() => handleDelete(car)}
                        />
                    ))}
                </div>
            </div>
        </div>
    );
}

function Formularz({ onCarAddedOrEdited, editingCar, setEditingCar })
{
    const [carDetails, setCarDetails] = useState([]);
    const [carService, setCarService] = useState([]);
    const [carLocation, setCarLocation] = useState({ name: "", longitude: 0, latitude: 0, address: "", id: -1 });
    const [file, setFile] = useState(null);

    const [formData, setFormData] = useState({
        id: -1,
        brand: '',
        model: '',
        year: 0,
        price: 0,
        isrented: false,
        photo: ''
    });

    useEffect(() =>
    {
        if (editingCar)
        {
            setFormData(editingCar);
        }
    }, [editingCar]);

    const handleFileChange = (event) =>
    {
        const selectedFile = event.target.files[0];
        setFile(selectedFile);
    };

    const handleInputChange = (e) =>
    {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleFormSubmit = async (e) =>
    {
        e.preventDefault();

        const formDataFile = new FormData();
        formDataFile.append("file", file);

        const response = await fetch("/api/images/upload", {
            method: "POST",
            body: formDataFile,
        });

        if (response.ok)
        {
            const result = await response.json();
            var file_path = result.filePath;
            console.log(file_path)

            var newCar = {};
            if (editingCar)
                newCar.id = editingCar.id;
            newCar.brand = formData.brand;
            newCar.model = formData.model;
            newCar.year = formData.year;
            newCar.photo = file_path;
            newCar.isrented = false;
            newCar.price = formData.price;
            newCar.location = carLocation;
            newCar.details = carDetails;
            newCar.services = carService;
            console.log(JSON.stringify(newCar));

            if (editingCar)
            {
                // Update car (PUT request)
                const response = await fetch(`api/Car/${editingCar.id}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(newCar),
                });

                if (response.ok)
                {
                    onCarAddedOrEdited();
                    clearForm();
                }
            }
            else
            {
                const response = await fetch('api/Car', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(newCar),
                });

                if (response.ok)
                {
                    onCarAddedOrEdited();
                    clearForm();
                }
            }
        }


    };

    const clearForm = () =>
    {
        setEditingCar(null);
        setFormData({ brand: '', model: '', year: '', price: '', location: '' });
        setCarDetails([]);
        setCarService([]);
        setCarLocation({ name: "", longitude: 0, latitude: 0, address: "", id: -1 });
    }

    return (
        <div className="car-form">
            <input type="text" name="brand" placeholder="Brand" value={formData.brand} onChange={handleInputChange} />
            <input type="text" name="model" placeholder="Model" value={formData.model} onChange={handleInputChange} />
            <input type="number" name="year" placeholder="Year" value={formData.year} onChange={handleInputChange} />
            <input type="number" name="price" placeholder="Price" value={formData.price} onChange={handleInputChange} />
            <input type="file" name="image" placeholder="image" accept="image/*" onChange={handleFileChange} />
            <CarDetails carId={formData.id} carDetails={carDetails} setCarDetails={setCarDetails} />
            <CarService carId={formData.id} carService={carService} setCarService={setCarService} />
            <Location carId={formData.id} location={carLocation} setLocation={setCarLocation} />
            <button onClick={handleFormSubmit} type="submit">{editingCar ? 'Edit' : 'Add'}</button>
            <button onClick={clearForm} type="submit">Cancel</button>
        </div>
    );
}



function CarItem({ car, onEdit, onDelete })
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
                <button className="editButton" onClick={onEdit}>Edit</button>
                <button className="deleteButton" onClick={onDelete}>Delete</button>
            </div>
        </div>
    );
}

export default App;
