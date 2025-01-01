import { useEffect, useState } from 'react';
import './css/CarLocation.css';

function Location({ carId, location, setLocation })
{
    const [searchResults, setSearchResults] = useState([]);
    const [searchString, setSearchString] = useState("");


    const fetchLocation = async () =>
    {
        if (carId == -1) return;
        const response = await fetch(`api/car/${carId}/location`);
        if (response.ok)
        {
            const jsonData = await response.json();
            setLocation(jsonData);
        }
    };

    useEffect(() =>
    {
        fetchLocation();
    }, [carId]);

    const handleInputChange = (field, value) =>
    {
        setLocation({ ...location, [field]: value, id: -1 });
        
        if (field === "name" || field === "address")
        {
            setSearchString(value);
        }
    };

    useEffect(() =>
    {
        const fetchSearchResults = async () =>
        {
            if (searchString === "" || !searchString)
            {
                setSearchResults([]);
                return;
            }

            const response = await fetch(`api/location/search/${searchString}`);
            if (response.ok)
            {
                const jsonData = await response.json();
                setSearchResults(jsonData);
            } else
            {
                setSearchResults([]);
            }
        };
        fetchSearchResults();
    }, [searchString]);

    const handleSuggestionClick = (suggestion) =>
    {
        setLocation({
            id: suggestion.id,
            name: suggestion.name,
            address: suggestion.address,
            latitude: suggestion.latitude,
            longitude: suggestion.longitude,
        });
        setSearchResults([]);
        setSearchString("");
    };

    return (
        <div>
            <h2>Location</h2>
            <div className="location">
                <input
                    type="text"
                    name="name"
                    placeholder="Name"
                    value={location.name || ""}
                    onChange={(e) => handleInputChange("name", e.target.value)}
                />
                <input
                    type="text"
                    name="address"
                    placeholder="Address"
                    value={location.address || ""}
                    onChange={(e) => handleInputChange("address", e.target.value)}
                />
                <input
                    type="number"
                    name="latitude"
                    placeholder="Latitude"
                    value={location.latitude || 0}
                    onChange={(e) => handleInputChange("latitude", parseFloat(e.target.value))}
                />
                <input
                    type="number"
                    name="longitude"
                    placeholder="Longitude"
                    value={location.longitude || 0}
                    onChange={(e) => handleInputChange("longitude", parseFloat(e.target.value))}
                />
            </div>

            {searchString && (
                <div className="searchDiv">
                    {searchResults.map((suggestion, i) => (
                        <div
                            key={i}
                            className="suggestion"
                            onClick={() => handleSuggestionClick(suggestion)}
                        >
                            {suggestion.name}: {suggestion.address}
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Location;
