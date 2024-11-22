import React, { useState, useEffect } from "react";

function Rents() {
    const [rents, setRents] = useState([]);

    useEffect(() => {
        const fetchRents = async () => {
            const response = await fetch("/api/Offer/rents");
            if (response.ok) {
                const jsonData = await response.json();
                setRents(jsonData);
            }
        };
        fetchRents();
    }, []);

    return (
        <div className="container">
            <h1>Rents</h1>
            <table className="rents-table">
                <thead>
                <tr>
                    <th>ID</th>
                    <th>Brand</th>
                    <th>Model</th>
                    <th>Year</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Start Date</th>
                    <th>End Date</th>
                    <th>Price</th>
                    <th>Status</th>                    
                </tr>
                </thead>
                <tbody>
                {rents.map((rent) => (
                    <tr key={rent.id}>
                        <td>{rent.id}</td>
                        <td>{rent.car.brand}</td>
                        <td>{rent.car.model}</td>
                        <td>{rent.car.year}</td>
                        <td>{rent.user.name}</td>
                        <td>{rent.user.surname}</td>
                        <td>{new Date(rent.startDate).toLocaleDateString()}</td>
                        <td>{new Date(rent.endDate).toLocaleDateString()}</td>
                        <td>{rent.price}</td>
                        <td>{rent.status}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default Rents;