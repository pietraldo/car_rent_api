namespace car_rent_api2.Server.Models;

public class OfferToDisplay(Offer offer)
{
    public Guid Id { get; set; } = offer.Id;
    public CarToDisplay Car { get; set; } = new(offer.Car);
    public string ClientId { get; set; } = offer.ClientId;
    public double Price { get; set; } = offer.Price;
    public DateTime StartDate { get; set; } = offer.StartDate;
    public DateTime EndDate { get; set; } = offer.EndDate;
}