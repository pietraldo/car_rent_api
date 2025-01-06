using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.DTOs;

public class OfferToDisplay(Offer offer)
{
    public Guid Id { get; set; } = offer.Id;
    public CarToDisplay Car { get; set; } = new(offer.Car);
    public string ClientId { get; set; } = offer.ClientId;
    public double Price { get; set; } = offer.Price;
    public DateTime StartDate { get; set; } = offer.StartDate;
    public DateTime EndDate { get; set; } = offer.EndDate;
    public List<CarDetail> CarDetails { get; set; } = offer.Car.Details;
    public Location Location { get; set; } = offer.Car.Location;
}