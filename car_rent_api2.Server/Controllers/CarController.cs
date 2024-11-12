using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace car_rent_api2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private static List<Car> cars = new List<Car>();

        public CarController()
        {
            if(cars.Count == 0)
            {
                cars.Add(new Car() { Id = 1, Brand = "Toyota", Model = "Corolla", Year = 2019, Photo = "https://image.api.playstation.com/vulcan/img/rnd/202011/0204/jvMomz0n9Be5mRKU8VP9Jl2A.png?w=440", IsRented = false, Price = 100, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
                cars.Add(new Car() { Id = 2, Brand = "Toyota", Model = "Yaris", Year = 2018, Photo = "https://lumiere-a.akamaihd.net/v1/images/open-uri20150422-20810-1fndzcd_41017374.jpeg?region=0,0,600,600", IsRented = false, Price = 90, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
                cars.Add(new Car() { Id = 3, Brand = "Toyota", Model = "Auris", Year = 2017, Photo = "https://m.atcdn.co.uk/ect/media/%7Bresize%7D/4b14ab0c7868451baf5912779f112f40.jpg", IsRented = false, Price = 80, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
                cars.Add(new Car() { Id = 4, Brand = "Toyota", Model = "Avensis", Year = 2016, Photo = "https://devil-cars.pl/storage/images/Tko1o17zh4AjbSQOUJhleUaysof12M7CfFMo8itK.jpg", IsRented = false, Price = 70, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
                cars.Add(new Car() { Id = 5, Brand = "Toyota", Model = "Prius", Year = 2015, Photo = "https://storage.googleapis.com/pod_public/1300/121015.jpg", IsRented = false, Price = 60, location = new Location() { Latitude = 52.2297700, Longitude = 21.0117800 } });
            }
        }

        // GET: api/<CarController>
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            
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
        public ActionResult<Car> Post([FromBody] Car car)
        {
            cars.Add(car);
            return CreatedAtAction(nameof(Post), new { id = car.Id }, car);
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
