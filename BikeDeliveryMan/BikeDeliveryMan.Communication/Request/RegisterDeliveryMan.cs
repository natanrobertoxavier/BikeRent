using BikeRent.Exception.ExceptionBase;
using System.Text.RegularExpressions;

namespace BikeDeliveryMan.Communication.Request;

public class RegisterDeliveryMan(
    string name,
    string email,
    string password,
    string cnpj,
    DateTime birthDate,
    string cnhNumber,
    string cnhCategory)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string CNPJ { get; set; } = cnpj;
    public DateTime BirthDate { get; set; } = birthDate;
    public string CNHNumber { get; set; } = cnhNumber;
    public string CNHCategory { get; set; } = cnhCategory;
}