import React, { useState, useEffect } from "react";
import "../css/Rents.css";
import Alert from "../components/Alert";

function Rents()
{

    const statusesInApi = ["Reserved", "Pending", "ReadyToReturn", "Finished", "Active", "Canceled"];

    const [rents, setRents] = useState([]);
    const [filtredRents, setFiltredRents] = useState([]);
    const [active_rent, setActiveRent] = useState(null);
    const [isEditing, setIsEditing] = useState(false);

    const [uniqueStatuses, setUniqueStatuses] = useState([]);

    const [filterStartDate, setFilterStartDate] = useState(null);
    const [filterEndDate, setFilterEndDate] = useState(null);
    const [filterStatus, setFilterStatus] = useState(statusesInApi);
    const [filterClient, setFilterClient] = useState("");
    const [filterCar, setFilterCar] = useState("");

    const [offset, setOffset] = useState(0);


    const refreashRents = async () =>
    {
        if (offset == 0) fetchRents();
        else setOffset(0);
    }

    const fetchRents = async () =>
    {
        console.log("fetching rents...");
        const response = await fetch(`/api/Rent/getrents?offset=${offset}&startDate=${filterStartDate}&endDate=${filterEndDate}&status=${filterStatus}&client=${filterClient}&car=${filterCar}`);
        if (response.ok)
        {
            const jsonData = await response.json();
            console.log(jsonData);
            setRents(jsonData);
       
        }
        setActiveRent(null);
    };

    const nextOffset = () =>
    {
        setOffset(offset + 10);
    }

    const previousOffset = () =>
    {
        var newOffset = offset - 10;
        if (newOffset < 0)
            newOffset = 0;
        setOffset(newOffset);
        
    }

    useEffect(() =>
    {
        fetchRents();
        setUniqueStatuses(statusesInApi);
    }, []);

    useEffect(() =>
    {
        fetchRents();
    }, [offset]);

    useEffect(() =>
    {
        filterRentsFunction();
    }, [rents]);


    const filterRentsFunction = () =>
    {
        var filltred = rents;
        filltred = filltred.filter(rent => rent.startDate >= filterStartDate || filterStartDate==null);
        filltred = filltred.filter(rent => rent.endDate <= filterEndDate || filterEndDate == null);
        filltred = filltred.filter(rent => filterStatus.includes(rent.status));

        filltred = filltred.filter(rent => rent.client.name.includes(filterClient) || rent.client.surname.includes(filterClient) || filterClient=="");
        filltred = filltred.filter(rent => rent.car.brand.includes(filterCar) || rent.car.model.includes(filterCar));

        console.log(filterClient=="");
        setFiltredRents(filltred);
    };

    const handleCheckboxChange = (status, checked) =>
    {
        setFilterStatus(prevFilterStatus =>
        {
            if (checked)
            {
                return [...prevFilterStatus, status];
            } else
            {
                return prevFilterStatus.filter(s => s !== status);
            }
        });
    };


    useEffect(() =>
    {
        filterRentsFunction();
    }, [filterStartDate, filterEndDate, filterStatus, filterClient, filterCar]);

    return (
        <div className="rents-container">
            <h1 className="rents-title">Rents</h1>
            <div className="rents-filters">
                <h2>Filter Rents</h2>

                <div className="filter-group">
                    <h3>Date Range</h3>
                    <div className="filter-dates">
                        <input
                            type="date"
                            placeholder="Start Date"
                            onChange={e => setFilterStartDate(e.target.value)}
                            className="filter-input"
                        />
                        <input
                            type="date"
                            placeholder="End Date"
                            onChange={e => setFilterEndDate(e.target.value)}
                            className="filter-input"
                        />
                    </div>
                </div>

                <div className="filter-group">
                    <h3>Statuses</h3>
                    <div className="filter-checkboxes">
                        {uniqueStatuses.map((status, index) => (
                            <label key={index} className="checkbox-label">
                                <input
                                    type="checkbox"
                                    value={status}
                                    checked={filterStatus.includes(status)}
                                    onChange={e => handleCheckboxChange(status, e.target.checked)}
                                    className="checkbox-input"
                                />
                                {status}
                            </label>
                        ))}
                    </div>
                </div>

                <div className="filter-group">
                    <h3>Search</h3>
                    <div className="filter-search">
                        <input
                            type="text"
                            placeholder="Client Name or Surname"
                            onChange={e => setFilterClient(e.target.value)}
                            className="filter-input"
                        />
                        <input
                            type="text"
                            placeholder="Car"
                            onChange={e => setFilterCar(e.target.value)}
                            className="filter-input"
                        />
                    </div>
                </div>
                <button onClick={refreashRents}>Refresh</button>
            </div>
            <button onClick={previousOffset}>Previous</button>
            <button onClick={nextOffset}>Next</button>
            <div className={`rents-content ${active_rent ? "details-active" : ""}`}>
                <div className="rents-list">
                    {filtredRents.map(rent => (
                        <Rent key={rent.id} rent={rent} onClick={() => { setActiveRent(rent); setIsEditing(false); }} />
                    ))}
                </div>

                {active_rent && (
                    <RentDetails rent={active_rent} isEditing={isEditing} setIsEditing={setIsEditing} refreashRents={fetchRents} />
                )}

            </div>
            
        </div>
    );
}

function RentDetails({ rent, isEditing, setIsEditing, refreashRents })
{
    const handleSaveNote = async () =>
    {
        var note = document.querySelector("#note").value;
        var photo = document.querySelector("input[type=file]").files[0];

        var photo_path;
        if (photo)
        {
            const formDataFile = new FormData();
            formDataFile.append("file", photo);
            const response = await fetch("/api/images/upload", {
                method: "POST",
                body: formDataFile,
            });
            const result = await response.json();
            photo_path = result.filePath;
        }
        else
        {
            photo_path = rent.linkToPhotos ? rent.linkToPhotos : "";
        }
        console.log(photo_path);

        var jsonBody = JSON.stringify({
            id: rent.id,
            note: note,
            linkToPhotos: photo_path,
        });

        console.log(jsonBody);

        const response = await fetch('/api/Rent/editnote', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: jsonBody,
        });
        const result = await response.json();
        if (result.success)
        {
            Alert("green", "Note updated successfully!");
            setIsEditing(false);

            refreashRents();
        }
        else
        {
            Alert("red", "Failed to update note.");
        }
        console.log("saving...");
    };

    const returnAndSendBill = async () =>
    {
        console.log(JSON.stringify({ rentId: rent.id }));
        const response = await fetch('/api/Rent/acceptReturn', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ rentId: rent.id }),
        });
        const result = await response.json();
        if (result.success)
        {
            Alert("green", "Returned successfuly!");
        }
        else
        {
            Alert("red", "Failed to return.");
        }
        refreashRents();
    }

    const startRent = async () =>
    {
        console.log(JSON.stringify({ rentId: rent.id }));
        const response = await fetch('/api/Rent/pickedUpByClient', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ rentId: rent.id }),
        });
        const result = await response.json();
        if (result.success)
        {
            Alert("green", "Rent started successfully!");
        }
        else
        {
            Alert("red", "Failed to start rent.");
        }
        refreashRents();
    }

    return (

        <div className="rents-details">
            <h2>Rent Details</h2>
            <h2>Car Details</h2>
            <p><strong>Brand:</strong> {rent.car.brand}</p>
            <p><strong>Model:</strong> {rent.car.model}</p>
            <p><strong>Year:</strong> {rent.car.year}</p>
            <p><strong>Status:</strong> {rent.car.status}</p>

            <h2>Client Details</h2>
            <p><strong>Name:</strong> {rent.client.name} {rent.client.surname}</p>
            <p><strong>Client ID:</strong> {rent.client.id}</p>

            <h2>Rent Details</h2>
            <p><strong>Status:</strong> {rent.status}</p>
            {rent.status == "Reserved" && <button onClick={startRent}>Start rent</button>}
            {rent.status == "ReadyToReturn" && <button onClick={returnAndSendBill}>Returned and send bill</button>}
            <p><strong>Start Date:</strong> {new Date(rent.startDate).toLocaleDateString()}</p>
            <p><strong>End Date:</strong> {new Date(rent.endDate).toLocaleDateString()}</p>
            <p><strong>Price:</strong> ${rent.price}</p>

            <h2>Notes</h2>
            {isEditing ? (
                <>
                    <textarea id="note"
                        defaultValue={rent.notes}
                        rows="4"
                        cols="50"
                    />
                    <br />
                    <input
                        type="file"
                        accept="image/*"
                    />
                    {rent.linkToPhotos && <img src={rent.linkToPhotos} alt="Uploaded" style={{ maxWidth: "100%", marginTop: "10px" }} />}
                    <br />
                    <button onClick={handleSaveNote}>Save Note</button>
                    <button onClick={() => setIsEditing(false)}>Cancel</button>
                </>
            ) : (
                <>
                    <p>{rent.notes}</p>
                    {rent.linkToPhotos && <img src={rent.linkToPhotos} alt="Rent" style={{ maxWidth: "100%" }} />}
                    <button onClick={() => setIsEditing(true)}>Edit Note</button>
                </>
            )}
        </div>
    );
}

function Rent({ rent, onClick })
{
    return (
        <div className="rent-item" onClick={onClick}>
            <h2>{rent.car.brand} {rent.car.model}</h2>
            <p><strong>Client:</strong> {rent.client.name}</p>
            <p><strong>Status:</strong> {rent.status}</p>
            <p><strong>Start Date:</strong> {formatDate(rent.startDate)}</p>
            <p><strong>End Date:</strong> {formatDate(rent.endDate)}</p>
        </div>
    );
}

function formatDate(date_string)
{
    const date = new Date(date_string);
    return new Intl.DateTimeFormat('en-US', { year: 'numeric', month: 'long', day: 'numeric' }).format(date);
}

export default Rents;