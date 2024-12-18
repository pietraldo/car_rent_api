import React, { useState, useEffect } from "react";
import "../css/Rents.css";

function Rents() {
    const [rents, setRents] = useState([]);

    useEffect(() => {
        const fetchRents = async () => {
            const response = await fetch("/api/Rent/getrents");
            if (response.ok) {
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
            <div className="rents-list">
                {rents.map(rent => (
                    <Rent key={rent.id} rent={rent} />
                ))}
            </div>
        </div>
    );
}

function Rent({ rent })
{
    return (
        <div className="rent-card">
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