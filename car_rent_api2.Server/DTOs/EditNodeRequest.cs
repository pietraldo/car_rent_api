using car_rent_api2.Server.Models;

namespace car_rent_api2.Server.DTOs;

public class EditNoteRequest
{
    public int Id { get; set; }
    public string Note { get; set; }
    public string LinkToPhotos { get; set; }
}