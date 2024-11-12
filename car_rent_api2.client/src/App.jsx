import { useEffect, useState } from 'react';
import './App.css';

function App()
{
    const [data, setData] = useState(null);

    useEffect(() =>
    {
        const fetchData = async () =>
        {

            const response = await fetch('api/Car');

            if (!response.ok) return;
            const jsonData = await response.json();
            setData(jsonData);

            console.log(response.body);

        };


        fetchData();
    }, []);


    return (
        <div>
            {!data ? (
                'Loading...'
            ) :
                <pre>{JSON.stringify(data, null, 2)}</pre>
            }
        </div>
    );
}

export default App;
