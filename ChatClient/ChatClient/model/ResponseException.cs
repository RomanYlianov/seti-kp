using System.Net;

namespace ChatClient.model
{
    internal class ResponseException : Exception
    {
        public HttpStatusCode StatusCode;

        private string? Message;

        public ResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}