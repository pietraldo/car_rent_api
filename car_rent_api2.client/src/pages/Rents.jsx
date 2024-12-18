import React, { useState, useEffect } from "react";
import "../css/Rents.css";

function Rents()
{
    const [rents, setRents] = useState([]);
    const [active_rent, setActiveRent] = useState(null);
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() =>
    {
        const fetchRents = async () =>
        {
            const response = await fetch("/api/Rent/getrents");
            if (response.ok)
            {
                const jsonData = await response.json();
                console.log(jsonData);
                setRents(jsonData);
            }
        };
        fetchRents();
    }, []);


    return (
        <div className="rents-container">
            <h1 className="rents-title">Rents</h1>
            <div className={`rents-content ${active_rent ? "details-active" : ""}`}>
                <div className="rents-list">
                    {rents.map(rent => (
                        <Rent key={rent.id} rent={rent} onClick={() => { setActiveRent(rent); setIsEditing(false); }} />
                    ))}
                </div>

                {active_rent && (
                        <RentDetails rent={active_rent} isEditing={isEditing} setIsEditing={setIsEditing} />
                )}

            </div>
        </div>
    );
}

function RentDetails({ rent, isEditing, setIsEditing })
{



    const handleSaveNote = async () =>
    {
        //const updatedRent = {
        //    ...rent,
        //    notes: note,
        //    linkToPhotos: photo,
        //};

        //try
        //{
        //    const response = await fetch(`/api/Rent/editnote`, {
        //        method: "POST",
        //        headers: {
        //            "Content-Type": "application/json",
        //        },
        //        body: JSON.stringify(updatedRent),
        //    });

        //    if (response.ok)
        //    {
        //        alert("Note updated successfully!");
        //        setIsEditing(false);
        //    } else
        //    {
        //        alert("Failed to update note.");
        //    }
        //} catch (error)
        //{
        //    console.error("Error updating note:", error);
        //    alert("An error occurred while updating the note.");
        //}
        console.log("saving");
    };

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
            <p><strong>Start Date:</strong> {new Date(rent.startDate).toLocaleDateString()}</p>
            <p><strong>End Date:</strong> {new Date(rent.endDate).toLocaleDateString()}</p>
            <p><strong>Price:</strong> ${rent.price}</p>

            <h2>Notes</h2>
            {isEditing ? (
                <>
                    <textarea
                        defaultValue={rent.notes}
                        rows="4"
                        cols="50"
                    />
                    <br />
                    <input
                        type="file"
                        accept="image/*"
                    />
                    {rent.LinkToPhotos && <img src={rent.LinkToPhotos} alt="Uploaded" style={{ maxWidth: "100%", marginTop: "10px" }} />}
                    <br />
                    <button onClick={handleSaveNote}>Save Note</button>
                    <button onClick={() => setIsEditing(false)}>Cancel</button>
                </>
            ) : (
                <>
                    <p>{rent.notes}</p>
                    {rent.LinkToPhotos && <img src={rent.LinkToPhotos} alt="Rent" style={{ maxWidth: "100%" }} />}
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