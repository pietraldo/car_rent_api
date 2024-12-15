import React from "react";

export default function RegisterPage() {
    return (
        <>
            <h1>Register</h1>
            <form action="/register" method="post">
                <input type="text" name="email" placeholder="Email" required />
                <input type="password" name="password" placeholder="Password" required />
                <button type="submit">Register</button>
            </form>
        </>
    );
}