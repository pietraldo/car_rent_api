using System.Collections;
using System.Collections.Concurrent;
using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.Services
{
    public class OfferManager : IOfferManager
    {
        private readonly ConcurrentDictionary<Guid, (Offer offer, DateTime Expiry)> _offers = new();
        private readonly Timer _timer;

        public OfferManager()
        {
            _timer = new Timer(RemoveExpiredOffers, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        public Guid AddOffer(Offer offer)
        {
            var id = Guid.NewGuid();
            var expiry = DateTime.UtcNow.AddMinutes(10);
            _offers[id] = (offer, expiry);
            return id;
        }

        public Offer? GetOffer(Guid id)
        {
            if (_offers.TryGetValue(id, out var value) && value.Expiry > DateTime.UtcNow)
            {
                return value.offer;
            }
            return null;
        }

        private void RemoveExpiredOffers(object? state)
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _offers
                .Where(kvp => kvp.Value.Expiry <= now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _offers.TryRemove(key, out _);
            }
        }

        public IEnumerable GetAllOffers() => _offers.Select(x=>new
        {
            id = x.Key,
            offer = x.Value.offer,
            expiry = x.Value.Expiry
        });
    }
}
