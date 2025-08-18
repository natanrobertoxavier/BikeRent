using System.Runtime.Serialization;

namespace BikeRent.Exception.ExceptionBase;

[Serializable]
public class InvalidLoginException : BikeRentException
{
    public InvalidLoginException() : base(ErrorsMessages.InvalidLogin)
    {
    }

    protected InvalidLoginException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
