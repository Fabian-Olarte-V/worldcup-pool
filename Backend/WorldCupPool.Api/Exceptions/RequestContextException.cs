namespace WorldCupPool.Api.Exceptions
{
    public sealed class RequestContextException : Exception
    {
        public RequestContextException(string message) : base(message)
        {
        }
    }
}
