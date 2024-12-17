import React from "react";

export default function LoginPage() {
    return (
        <>
            <h1>User</h1>
            <form action="/register" method="post">
                <input type="text" name="username" placeholder="Username" required />
                <input type="password" name="password" placeholder="Password" required />
                <button type="submit" name="login">Register</button>
            </form>
        </>
    );
}