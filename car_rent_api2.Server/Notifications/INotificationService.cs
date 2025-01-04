using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.Notifications;

public interface INotificationService
{
    public void Notify(Rent rent);
}