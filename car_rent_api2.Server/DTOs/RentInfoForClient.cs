using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.DTOs;

public class RentInfoForClient
{
    public string RentId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string CarBrand { get; set; }
    public string CarModel { get; set; }
    public int CarYear { get; set; }
    public float Price { get; set; }

    public RentInfoForClient(Rent rent)
    {
        RentId = rent.Id.ToString();
        Start = rent.StartDate;
        End = rent.EndDate;
        CarBrand = rent.Car.Brand;
        CarModel = rent.Car.Model;
        CarYear = rent.Car.Year;
        Price = (float)rent.Price;
    }
}