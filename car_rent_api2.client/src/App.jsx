import { useEffect, useState } from 'react';
import './App.css';
import CarDetails from './CarDetailsList';

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
            console.log(data);
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

function Formularz({ onCarAddedOrEdited, editingCar })
{
    const [formData, setFormData] = useState({
        id: 0,
        brand: '',
        model: '',
        year: 0,
        price: 0,
        location: {
            longitude: 0,
            latitude: 0
        },
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

    const handleInputChange = (e) =>
    {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleFormSubmit = async (e) =>
    {
        e.preventDefault();

        if (editingCar)
        {
            console.log(formData)
            // Update car (PUT request)
            const response = await fetch(`api/Car/${editingCar.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData),
            });

            if (response.ok)
            {
                onCarAddedOrEdited(); 
            }
        } else
        {
            console.log(JSON.stringify(formData));
            const response = await fetch('api/Car', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData),
            });

            if (response.ok)
            {
                onCarAddedOrEdited(); 
                setFormData({ brand: '', model: '', year: '', price: '', location: '' }); 
            }
        }
    };

    return (
        <div className="car-form">
            <input type="text" name="brand" placeholder="Brand" value={formData.brand} onChange={handleInputChange} />
            <input type="text" name="model" placeholder="Model" value={formData.model} onChange={handleInputChange} />
            <input type="number" name="year" placeholder="Year" value={formData.year} onChange={handleInputChange} />
            <input type="number" name="price" placeholder="Price" value={formData.price} onChange={handleInputChange} />
            <input type="file" name="photo" placeholder="Photo" />
            <input type="text" name="location" placeholder="Location" value={formData.location} onChange={handleInputChange} />
            <CarDetails carId={2} />
            <button onSubmit={handleFormSubmit} type="submit">{editingCar ? 'Edit' : 'Add'}</button>
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
