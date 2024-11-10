using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace car_rent_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private static readonly List<Car> cars = new List<Car>
        {
            new Car { Id = 1, Brand = "BMW", Name = "Corolla", Color = "White", Year = 2019 },
            new Car { Id = 2, Brand = "Toyota", Name = "Camry", Color = "Black", Year = 2020 },
            new Car { Id = 3, Brand = "Honda", Name = "Civic", Color = "Red", Year = 2018 },
            new Car { Id = 4, Brand = "Honda", Name = "Accord", Color = "Blue", Year = 2017 }
        };

        public CarController()
        {
        }

        // Existing GET endpoint to get all cars
        [HttpGet(Name = "GetCars")]
        public IEnumerable<Car> Get()
        {
            return cars;
        }

        // New GET endpoint to get a car by ID
        [HttpGet("{id}", Name = "GetCarById")]
        public ActionResult<Car> GetById(int id)
        {
            var car = cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }
    }
}
