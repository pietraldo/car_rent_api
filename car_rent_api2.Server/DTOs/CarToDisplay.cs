namespace car_rent_api2.Server.DTOs;

public class CarToDisplay(Car car)
{
    public string Model { get; set; } = car.Model;
    public string Brand { get; set; } = car.Brand;
    public int Year { get; set; } = car.Year;
    public string Picture { get; set; } = car.Photo;
}