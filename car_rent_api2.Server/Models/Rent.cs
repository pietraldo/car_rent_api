namespace car_rent_api2.Server.Models
{
    public enum RentStatus
    {
        Reserved,
        Pending,
        Active,
        ReadyToReturn,
        Finished,
        Canceled
    }
    public class Rent
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public Client Client { get; set; }
        public Employee? Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public RentStatus Status { get; set; }
        public string Notes { get; set; }
        public string LinkToPhotos { get; set; }
    }
}
