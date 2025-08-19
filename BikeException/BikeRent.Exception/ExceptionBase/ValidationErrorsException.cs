using System.Runtime.Serialization;

namespace BikeRent.Exception.ExceptionBase;

[Serializable]
public class ValidationErrorsException : BikeRentException
{
    public List<string> ErrorMessages { get; set; } = [];
    public ValidationErrorsException(List<string> errorMessages) : base(string.Empty)
    {
        ErrorMessages = errorMessages;
    }

    protected ValidationErrorsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
