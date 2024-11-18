import { useEffect, useState } from 'react';
import './css/CarService.css';

function CarService({ carId, carService, setCarService})
{
    const [searchResults, setSearchResults] = useState([]);
    const [searchString, setSearchString] = useState("");
    const [searchIndex, setSearchIndex] = useState(-1);

    const fetchCarService = async () =>
    {
        const response = await fetch(`api/car/carservices/${carId}`);
        if (response.ok)
        {
            const jsonData = await response.json();
            setCarService(jsonData);
        }
    }

    useEffect(() =>
    {
        fetchCarService();
    }, [carId]);

    const handleAddClick = () =>
    {
        setCarService([...carService, { id: -1, name: "", price: 0, description: "" }]);
    };

    const handleRemoveClick = (index) =>
    {
        setCarService(carService.filter((carService, i) => i !== index));
    };


    const handleInputChange = (index, field, value) =>
    {
        const newCarService = [...carService];
        newCarService[index][field] = value;
        newCarService[index]["id"] = -1;
        setCarService(newCarService);
        setSearchIndex(index);


        if (field === "name")
        {
            setSearchString(value);
        }
    }

    useEffect(() =>
    {
        const fetchSearchResults = async () =>
        {
            if (searchString === "" || !searchString)
            {
                setSearchResults([]);
                return;
            }

            const response = await fetch(`api/car/carservices/search/${searchString}`);
            if (response.ok)
            {
                const jsonData = await response.json();
                setSearchResults(jsonData);
            }
            else 
            {
                setSearchResults([]);
            }
        };
        fetchSearchResults();
    }, [searchString]);

    const handleSuggestionClick = (index, suggestion) =>
    {
        const newCarService = [...carService];
        newCarService[index].description = suggestion.description;
        newCarService[index].name = suggestion.name;
        newCarService[index].id = suggestion.id;
        newCarService[index].price = suggestion.price;
        setCarService(newCarService);
        setSearchResults([]);
        setSearchString("");
        setSearchIndex(-1);
    }

    return (
        <div>
            <h2>Car Services</h2>
            {carService.map((carService, index) => (
                <div key={index} >
                    <div className="car-service">
                        <input
                            type="text"
                            name="name"
                            placeholder="Name"
                            value={carService.name}
                            onChange={(e) => handleInputChange(index, "name", e.target.value)}
                        />
                        <input
                            type="text"
                            name="description"
                            placeholder="Description"
                            value={carService.description}
                            onChange={(e) => handleInputChange(index, "description", e.target.value)}
                        />
                        <input
                            type="number"
                            name="price"
                            placeholder="Price"
                            value={carService.price}
                            onChange={(e) => handleInputChange(index, "price", e.target.value)}
                        />
                        <button className="remove-button" onClick={() => handleRemoveClick(index)}>Remove</button>
                    </div>

                    {searchString && index === searchIndex && (
                        <div className="searchDiv">
                            {searchResults.map((suggestion, i) => (
                                <div
                                    key={i}
                                    className="suggestion"
                                    onClick={() => handleSuggestionClick(index, suggestion)}
                                >
                                    { suggestion.name}: {suggestion.description}: {suggestion.price}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            ))}

            <button onClick={handleAddClick}>Add new car service</button>
        </div>
    );

}


export default CarService;