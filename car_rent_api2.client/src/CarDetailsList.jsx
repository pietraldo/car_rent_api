import { useEffect, useState } from 'react';
import './css/CarDetails.css';

function CarDetails({ carId, carDetails, setCarDetails })
{
    const [searchResults, setSearchResults] = useState([]);
    const [searchString, setSearchString] = useState("");
    const [searchIndex, setSearchIndex] = useState(-1);

    const fetchCarDetails = async () =>
    {
        const response = await fetch(`api/car/cardetails/${carId}`);
        if (response.ok)
        {
            const jsonData = await response.json();
            setCarDetails(jsonData);
        }
    }

    useEffect(() =>
    {
        fetchCarDetails();
        console.log(carId);
    }, [carId]);

    const handleAddClick = () =>
    {
        setCarDetails([...carDetails, { id:-1, description: "", value: "" }]);
    };

    const handleRemoveClick = (index) =>
    {
        setCarDetails(carDetails.filter((carDetail, i) => i !== index));
    };


    const handleInputChange = (index, field, value) =>
    {
        const newCarDetails = [...carDetails];
        newCarDetails[index][field] = value;
        newCarDetails[index]["id"] = -1; // This is a new car detail, not one that was fetched from the database]
        setCarDetails(newCarDetails);
        setSearchIndex(index);


        if (field === "description")
        {
            setSearchString(value);
        }
        else
        {
            setSearchString("");
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

            const response = await fetch(`api/car/cardetails/search/${searchString}`);
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
        const newCarDetails = [...carDetails];
        newCarDetails[index].description = suggestion.description;
        newCarDetails[index].value = suggestion.value;
        newCarDetails[index].id = suggestion.id;
        setCarDetails(newCarDetails);
        setSearchResults([]);
        setSearchString("");
        setSearchIndex(-1);
    }

    return (
        <div>
            <h2>Car Details</h2>
            {carDetails.map((carDetail, index) => (
                <div key={index} >
                    <div className="car-detail">
                        <input
                            type="text"
                            name="description"
                            placeholder="Description"
                            value={carDetail.description}
                            onChange={(e) => handleInputChange(index, "description", e.target.value)}
                        />
                        <input
                            type="text"
                            name="carDetailValue"
                            placeholder="Value"
                            value={carDetail.value}
                            onChange={(e) => handleInputChange(index, "value", e.target.value)}
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
                                    {suggestion.description}: {suggestion.value}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            ))}

            <button onClick={handleAddClick}>Add new car detail</button>
        </div>
    );

}


export default CarDetails;