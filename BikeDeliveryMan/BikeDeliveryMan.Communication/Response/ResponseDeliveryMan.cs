namespace BikeDeliveryMan.Communication.Response;

public class ResponseDeliveryMan
{
    public ResponseDeliveryMan(
        Guid id,
        DateTime registrationDate,
        string name,
        string email,
        string cnpj,
        DateTime birthDate,
        string cnhNumber,
        string cnhType)
    {
        Id = id;
        RegistrationDate = registrationDate;
        Name = name;
        Email = email;
        CNPJ = cnpj;
        BirthDate = birthDate;
        CNHNumber = cnhNumber;
        CNHType = cnhType;
    }

    public ResponseDeliveryMan()
    {
    }

    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string CNPJ { get; set; }
    public DateTime BirthDate { get; set; }
    public string CNHNumber { get; set; }
    public string CNHType { get; set; }
}
