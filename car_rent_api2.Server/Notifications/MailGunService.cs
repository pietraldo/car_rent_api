using car_rent_api2.Server.Models;
using car_rent_api2.Server.Notifications;
using RestSharp;

namespace car_rent_api2.Server.Notifications;

public class MailGunService : INotificationService, IEmailService
{
    private readonly MailGunClient _client;
    private readonly HtmlMessageGenerator _htmlMessageGenerator;

    public MailGunService()
    {
        var apiKey = Environment.GetEnvironmentVariable("MAILGUN_API_KEY");
        _client = new MailGunClient(apiKey);
        _htmlMessageGenerator = new HtmlMessageGenerator();
    }

    public RestResponse SendEmail(string email, string subject, string message)
    {
        return _client.SendEmail(email, subject, message);
    }

    public void Notify(Rent rent)
    {
        var messageGenerator = new HtmlMessageGenerator();
        var message = messageGenerator.CreateCarReturnedMessage(rent);

        SendEmail(rent.Client.Email, "[Car Rent] Bill for your rent", message);
    }
}