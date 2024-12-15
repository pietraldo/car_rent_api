import React from "react";

export default function LoginPage() {
    return (
        <>
            <h1>Login</h1>
            <form action="/login" method="post">
                <input type="text" name="email" placeholder="Email" required />
                <input type="password" name="password" placeholder="Password" required />
                <button type="submit">Login</button>
            </form>
        </>
    );
}