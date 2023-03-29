using System.Net;

namespace ChatClient.model
{
    public class ResponseEntity<T>
    {
        public string Version { get; set; } = "HTTP/1.1";
        public HttpStatusCode? StatusCode { get; set; }

        public string[]? Config { get; set; }

        public Cookie[]? Cookies { get; set; }

        public string ContentType { get; set; } = "application/json";

        public DateTime? Date { get; set; }

        public ConnectionType? ConnectionType { get; set; }

        public T[]? Body { get; set; }

        public ResponseEntity()
        {
            this.StatusCode = null;
            this.Config = null;
            this.Cookies = null;
            this.Date = null;
            this.ConnectionType = null;
            this.Body = null;
        }

        public void PrintResponseDetails()
        {
            Console.WriteLine("получен ответ:");
            Console.WriteLine("версия протокола: " + Version);
            Console.Write("Код ответа: ");
            if (StatusCode != null)
            {
                Console.WriteLine(StatusCode);
            }
            else
            {
                Console.WriteLine("не определен");
            }
            Console.WriteLine("служебные параметры:");
            string[]? config = Config;
            if (config != null)
            {
                foreach (string v in config)
                {
                    Console.WriteLine(v);
                }
            }
            Console.WriteLine("Куки:");
            Cookie[]? cookies = Cookies;
            if (cookies != null)
            {
                foreach (Cookie c in cookies)
                {
                    string info = $"Name: {c.Name}, value: {c.Value}, Domain: {c.Domain}, Path: {c.Path}, HttpOnly: {c.HttpOnly};";
                    Console.WriteLine(info);
                }
            }
            Console.WriteLine("Тип контента: " + ContentType);
            Console.Write("Дата: ");
            if (Date != null)
            {
                Console.WriteLine(Date);
            }
            else
            {
                Console.WriteLine("не определена");
            }
            Console.WriteLine("Тип соединения: " + ConnectionType);
            Console.WriteLine("Тело ответа:");
            if (Body != null)
            {
                foreach (T item in Body)
                {
                    if (item != null)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine("отсутствует");
            }
        }
    }
}