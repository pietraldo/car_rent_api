using RestSharp;

namespace car_rent_api2.Server.Notifications;

public interface IEmailService
{
    RestResponse SendEmail(string email, string subject, string message);
}