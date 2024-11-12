import { useEffect, useState } from 'react';
import './App.css';

function App()
{
    const [data, setData] = useState([]);
    const [editingCar, setEditingCar] = useState(null); 

    const fetchCarList = async () =>
    {
        const response = await fetch('api/Car');
        if (response.ok)
        {
            const jsonData = await response.json();
            setData(jsonData);
        }
    };

    useEffect(() =>
    {
        fetchCarList();
    }, []);


    const handleEdit = (car) =>
    {
        setEditingCar(car); 
    };


    const handleCarAddedOrEdited = () =>
    {
        fetchCarList(); 
        setEditingCar(null); 
    };

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
                <input type="text" placeholder="Search..." className="searchInput" />
                <div className="carList">
                    {data.map((car) => (
                        <CarItem key={car.id} car={car} onEdit={() => handleEdit(car)} />
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
        <form onSubmit={handleFormSubmit}>
            <input type="text" name="brand" placeholder="Brand" value={formData.brand} onChange={handleInputChange} />
            <input type="text" name="model" placeholder="Model" value={formData.model} onChange={handleInputChange} />
            <input type="number" name="year" placeholder="Year" value={formData.year} onChange={handleInputChange} />
            <input type="number" name="price" placeholder="Price" value={formData.price} onChange={handleInputChange} />
            <input type="file" name="photo" placeholder="Photo" />
            <input type="text" name="location" placeholder="Location" value={formData.location} onChange={handleInputChange} />
            <button type="submit">{editingCar ? 'Edit' : 'Add'}</button>
        </form>
    );
}

function CarItem({ car, onEdit })
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
                <button className="deleteButton">Delete</button>
            </div>
        </div>
    );
}

export default App;
