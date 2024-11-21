namespace car_rent_api2.Server.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string LinkToPhotos { get; set; }
    }
}
