import React from "react";
import { Link } from "react-router-dom";
import { FaHome, FaCar } from "react-icons/fa"; 
import "./css/Nav.css";
import './css/font.css';

function Nav()
{
    return (
        <nav className="modern-navbar">
            <div className="navbar-brand">CarRental</div>
            <ul className="navbar-links">
                <li>
                    <Link to="/" className="navbar-link">
                        <FaHome className="icon"/> Home
                    </Link>
                </li>
                <li>
                    <Link to="/aviablecarview" className="navbar-link">
                        <FaCar className="icon"/> Available Cars
                    </Link>
                </li>
                <li>
                    <Link to="/addeditcar" className="navbar-link">
                        <FaCar className="icon"/> Add/Edit Car
                    </Link>
                </li>
                <li>
                    <Link to="/rents" className="navbar-link">
                        <FaCar className="icon"/> Rents
                    </Link>
                </li>
                <li>
                    <Link to="/loginpage" className={"navbar-link"}>
                        <FaCar className="icon"/> Login
                    </Link>
                </li>
                <li>
                    <Link to="/registerpage" className={"navbar-link"}>
                        <FaCar className="icon"/> Register
                    </Link>
                </li>
            </ul>
        </nav>
    );
}

export default Nav;
