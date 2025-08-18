using System.Runtime.Serialization;

namespace BikeRent.Exception.ExceptionBase;
public class BikeRentException : SystemException
{
    public BikeRentException(string mensagem) : base(mensagem)
    {
    }

    protected BikeRentException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}