using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private List<Car> cars = new List<Car>();

        public CarController()
        {
            cars.Add(new Car() { Id = 1, Brand = "Toyota", Model = "Corolla", Year = 2019, Photo = "https://www.cstatic-images.com/car-pictures/xl/USC90TOC021A021001.png", IsRented = false, Price = 100, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
            cars.Add(new Car() { Id = 2, Brand = "Toyota", Model = "Yaris", Year = 2018, Photo = "https://www.cstatic-images.com/car-pictures/xl/USC90TOC021A021001.png", IsRented = false, Price = 90, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
            cars.Add(new Car() { Id = 3, Brand = "Toyota", Model = "Auris", Year = 2017, Photo = "https://www.cstatic-images.com/car-pictures/xl/USC90TOC021A021001.png", IsRented = false, Price = 80, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
            cars.Add(new Car() { Id = 4, Brand = "Toyota", Model = "Avensis", Year = 2016, Photo = "https://www.cstatic-images.com/car-pictures/xl/USC90TOC021A021001.png", IsRented = false, Price = 70, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
            cars.Add(new Car() { Id = 5, Brand = "Toyota", Model = "Prius", Year = 2015, Photo = "https://www.cstatic-images.com/car-pictures/xl/USC90TOC021A021001.png", IsRented = false, Price = 60, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
        }

        // GET: api/<CarController>
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            Thread.Sleep(2000);
            return cars;
        }

        // GET api/<CarController>/5
        [HttpGet("{id}")]
        public Car Get(int id)
        {
            return cars.Where(cars => cars.Id == id).FirstOrDefault(); 
        }

        // POST api/<CarController>
        [HttpPost]
        public void Post([FromBody] Car car)
        {
            cars.Add(car);
        }

        // PUT api/<CarController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Car car)
        {
            cars.Remove(cars.Where(cars => cars.Id == id).FirstOrDefault());
            cars.Add(car);
        }

        // DELETE api/<CarController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            cars.Remove(cars.Where(cars => cars.Id == id).FirstOrDefault());
        }
    }
}
