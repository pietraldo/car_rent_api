namespace car_rent_api2.Server.Models
{
    public class Offer
    {
        public Guid Id { get; set; }
        public Car Car { get; set; }
        public string ClientId { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Offer()
        {
        }
        public Offer(Car car, string clientId, double price, DateTime startDate, DateTime endDate)
        {
            Car = car;
            ClientId = clientId;
            Price = price;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
