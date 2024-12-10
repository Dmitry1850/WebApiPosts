namespace MainProgram.API.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message);
}
