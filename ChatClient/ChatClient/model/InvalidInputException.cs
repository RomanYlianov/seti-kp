namespace ChatClient.model
{
    internal class InvalidInputException : Exception
    {
        public InvalidInputException(String message) : base(message)
        {
        }
    }
}