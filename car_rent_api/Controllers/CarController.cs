using Microsoft.AspNetCore.Mvc;

namespace car_rent_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        

        public CarController()
        {
            
        }

        [HttpGet(Name = "GetCars")]
        public IEnumerable<Car> Get()
        {
            return new Car[]
            {
                new Car { Brand = "Toyota", Name = "Corolla", Color = "White", Year = 2019 },
                new Car { Brand = "Toyota", Name = "Camry", Color = "Black", Year = 2020 },
                new Car { Brand = "Honda", Name = "Civic", Color = "Red", Year = 2018 },
                new Car { Brand = "Honda", Name = "Accord", Color = "Blue", Year = 2017 }
            };
        }
    }
}
