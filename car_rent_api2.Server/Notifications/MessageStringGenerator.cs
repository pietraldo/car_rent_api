using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.Notifications;

public class HtmlMessageGenerator : IMessageGenerator
{
    public string CreateCarReturnedMessage(Rent rent)
    {
        return $@"<html>
<body>
<h1>Car Rent - Car Returned</h1>
<p>Hello,</p>
<p>Your car has been returned successfully. You have been charged for using our services.</p>
<p>Rent details:</p>
<ul>
<li>Car: {rent.Car.Brand} {rent.Car.Model}</li>
<li>Year: {rent.Car.Year}</li>
<li>Rent date: {rent.StartDate:yyyy-MM-dd HH:mm:ss}</li>
<li>Return date: {rent.EndDate:yyyy-MM-dd HH:mm:ss}</li>
<li>Cost: {rent.Price}</li>
</ul>
<p>Thank you for using Car Rent.</p>
</body>
</html>";
    }
}

public interface IMessageGenerator
{
    public string CreateCarReturnedMessage(Rent rent);
}