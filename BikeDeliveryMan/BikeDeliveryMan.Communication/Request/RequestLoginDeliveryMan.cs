namespace BikeDeliveryMan.Communication.Request;

public class RequestLoginDeliveryMan(
    string email,
    string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}