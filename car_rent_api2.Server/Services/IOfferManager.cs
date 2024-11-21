using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.Services
{
    public interface IOfferManager
    {
        Guid AddOffer(Offer offer);
        Offer? GetOffer(Guid id);
    }
}
