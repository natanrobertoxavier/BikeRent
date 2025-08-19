namespace BikeDeliveryMan.Domain.Entities;

public class DeliveryMan : BaseEntity
{
    public DeliveryMan(
        Guid id,
        DateTime registrationDate,
        string name,
        string email,
        string password,
        string cnpj,
        DateTime birthDate,
        string cnhNumber,
        string cnhCategory,
        bool photoSent) : base(id, registrationDate)
    {
        Name = name;
        Email = email;
        Password = password;
        CNPJ = cnpj;
        BirthDate = birthDate;
        CNHNumber = cnhNumber;
        CNHCategory = cnhCategory;
        PhotoSent = photoSent;
    }

    public DeliveryMan(
        string name,
        string email,
        string password,
        string cnpj,
        DateTime birthDate,
        string cnhNumber,
        string cnhType,
        bool photoSent)
    {
        Name = name;
        Email = email;
        Password = password;
        CNPJ = cnpj;
        BirthDate = birthDate;
        CNHNumber = cnhNumber;
        CNHCategory = cnhType;
        PhotoSent = photoSent;
    }

    public DeliveryMan()
    {
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CNPJ { get; set; }
    public DateTime BirthDate { get; set; }
    public string CNHNumber { get; set; }
    public string CNHCategory { get; set; }
    public bool PhotoSent { get; set; }
}