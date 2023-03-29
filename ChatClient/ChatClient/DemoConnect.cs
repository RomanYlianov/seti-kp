using ChatClient.model;
using ChatClient.model.dto;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal class DemoConnect
    {
        private const string HOST = "localhost";

        private const int PORT = 8080;

        public void TestConnection()
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //socket.Bind(new IPEndPoint(IPAddress.Any, PORT));
                socket.Connect(HOST, PORT);
                Console.WriteLine("соединение установлено");
                // определяем отправляемые данные
                var message = "GET /get-example HTTP/1.1\r\nHost: localhost\r\nConnection: close\r\n\r\n";
                // конвертируем данные в массив байтов
                var messageBytes = Encoding.UTF8.GetBytes(message);
                // отправляем данные
                _ = socket.Send(messageBytes, SocketFlags.None);

                // буфер для получения данных
                var responseBytes = new byte[512];
                // получаем данные
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                // преобразуем полученные данные в строку
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                // выводим данные на консоль
                Console.WriteLine("=======");
                Console.WriteLine(response);
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string FormCredentials(string username, string password)
        {
            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                          .GetBytes(username + ":" + password));
            return encoded;
        }

        public void TestAuth()
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //socket.Bind(new IPEndPoint(IPAddress.Any, PORT));
                socket.Connect(HOST, PORT);
                Console.WriteLine("соединение установлено");
                // определяем отправляемые данные
                var message = "POST /login HTTP/1.1\r\nAuthorization: Basic #1#\r\nHost: localhost\r\nConnection: close\r\n\r\n\r\nContent-Type: application/json\r\nContent-Length: 0";
                string credentials = FormCredentials("user@gmail.com", "user");
                message = message.Replace("#1#", credentials);
                // конвертируем данные в массив байтов
                var messageBytes = Encoding.UTF8.GetBytes(message);
                // отправляем данные
                _ = socket.Send(messageBytes, SocketFlags.None);

                // буфер для получения данных
                var responseBytes = new byte[512];
                // получаем данные
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                // преобразуем полученные данные в строку
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                // выводим данные на консоль
                Console.WriteLine("=======");
                Console.WriteLine(response);
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            /*
             * POST /login HTTP/1.1
                Authorization: Basic dXNlckBnbWFpbC5jb206dXNlcg==
                Host: localhost
                Content-Type: application/json
                Content-Length: 0
            */
        }

        public void MockTestRequest()
        {     //test login
              //AccountRegisterDTO dto = new AccountRegisterDTO("Valiliev", "Vasya", "vasya@gmail.com", "0000");

            ApiChatClient api = new ApiChatClient();
            MessageSearchDTO dto = new MessageSearchDTO("u", "a", "h", null, null, null, null);
            //api.Login(new AccountLoginDTO("admin@gmail.com", "admin"));
            //api.RemoveMessagesByParameters(dto);
            //api.Register(new AccountRegisterDTO("Vasiliev", "Petr", "vasya@gmail.com", "0000"));
        }

        public void SS()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://google.com");
            request.CookieContainer = new CookieContainer();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // Print the properties of each cookie.
                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Cookie:");
                    Console.WriteLine($"{cook.Name} = {cook.Value}");
                    Console.WriteLine($"Domain: {cook.Domain}");
                    Console.WriteLine($"Path: {cook.Path}");
                    Console.WriteLine($"Port: {cook.Port}");
                    Console.WriteLine($"Secure: {cook.Secure}");

                    Console.WriteLine($"When issued: {cook.TimeStamp}");
                    Console.WriteLine($"Expires: {cook.Expires} (expired? {cook.Expired})");
                    Console.WriteLine($"Don't save: {cook.Discard}");
                    Console.WriteLine($"Comment: {cook.Comment}");
                    Console.WriteLine($"Uri for comments: {cook.CommentUri}");
                    Console.WriteLine($"Version: RFC {(cook.Version == 1 ? 2109 : 2965)}");

                    // Show the string representation of the cookie.
                    Console.WriteLine($"String: {cook}");
                }
            }
        }

        public void ParseQuery()
        {
            string value = ResponseStore.ACCOUNT.UPDATE_ACCOUNT;

            string vesrion = value.Split(" ")[0];
            Console.WriteLine(vesrion);
            string statusCode = value.Split(" ")[1];
            Console.WriteLine(statusCode);

            //"HTTP/1.1 200 \r\nX-Content-Type-Options: nosniff\r\nX-XSS-Protection: 1; mode=block\r\nCache-Control: no-cache, no-store, max-age=0, must-revalidate\r\nPragma: no-cache\r\nExpires: 0\r\nX-Frame-Options: DENY\r\nSet-Cookie: SESSION=NjJkYzJmYmItNGRhYi00ZWFiLTg5MzktZmZjMGM1OGM5Mjc5; Path=/; HttpOnly; SameSite=Lax\r\nContent-Type: application/json\r\nTransfer-Encoding: chunked\r\nDate: Tue, 07 Mar 2023 10:02:35 GMT\r\nConnection: close\r\n\r\n5d\r\n{\"id\":4,\"firstName\":\"Valerya\",\"lastName\":\"Sidorov\",\"roles\":[\"USER\"],\"email\":\"demo@gmail.com\"}\r\n"

            string[] array = GetConfigParams(value);

            foreach (string val in array)
            {
                Console.WriteLine(val);
            }

            Cookie cookie = GetCookies(value);

            Console.WriteLine(cookie.ToString());

            array = GetContentType(value);

            foreach (string val in array)
            {
                Console.WriteLine(val);
            }

            DateTime date = GetDate(value);
            Console.WriteLine(date);

            ConnectionType type = GetConnectionType(value);

            Console.WriteLine(type);

            string body = GetResponseBody(value);

            string[] GetConfigParams(string resp)
            {
                int si = resp.IndexOf("\r\n");
                int ind = si;
                int temp = 0;
                while (ind < resp.Length)
                {
                    if (ind + 1 < resp.Length)
                    {
                        if (resp.Substring(ind).StartsWith("\r\n"))
                        {
                            temp++;
                            if (temp == 6)
                            {
                                break;
                            }
                        }
                    }
                    ind++;
                }
                string data = resp.Substring(si + 2, ind - si - 2);
                return data.Split("\r\n");
            }

            Cookie GetCookies(string resp)
            {
                int si = resp.IndexOf("\r\nSet-Cookie: ");
                si += 13;
                int ei = resp.IndexOf("\r\nContent-Type: ");

                string resultString = resp.Substring(si, ei - si);
                string[] arr = resultString.Split(";");
                Cookie cookie = new();
                cookie.Secure = true;
                cookie.Domain = HOST;

                cookie.Name = arr[0].Split("=")[0].Trim();
                cookie.Value = arr[0].Split("=")[1];
                cookie.Path = arr[1].Split("=")[1];
                if (arr[2].Equals(" HttpOnly"))
                {
                    cookie.HttpOnly = true;
                }

                return cookie;
            }

            string[] GetContentType(string resp)
            {
                int si = resp.IndexOf("\r\nContent-Type: ");
                int ei = resp.IndexOf("\r\nDate");
                string data = resp.Substring(si, ei - si - 2);
                string[] array = data.Split("\r\n");
                return array;
            }

            DateTime GetDate(string resp)
            {
                int si = resp.IndexOf("\r\nDate: ") + 8;
                int ind = si;
                while (ind < resp.Length)
                {
                    if (resp.Substring(ind).StartsWith("\r\n"))
                    {
                        break;
                    }
                    ind++;
                }

                string arg = resp.Substring(si, ind - si);
                return DateTime.Parse(arg);
            }

            ConnectionType GetConnectionType(string resp)
            {
                int si = resp.IndexOf("\r\nConnection: ");
                int ei = si;
                while (ei < resp.Length)
                {
                    if (ei < resp.Length - 1)
                    {
                        if (resp[ei].Equals('\r') && resp[ei + 1].Equals('\n'))
                        {
                            break;
                        }
                    }

                    ei++;
                }
                string typeValue = resp.Substring(si + 14, ei - si + 5);
                ConnectionType type = (ConnectionType)Enum.Parse(typeof(ConnectionType), typeValue.ToUpper());
                return type;
            }

            string GetResponseBody(string resp)
            {
                return resp.Split("\r\n\r\n")[1];
            }
        }

        //public string Getubstring(string text, int si, int ei)
        //{
        //    string result = string.Empty;
        //    for (int i=si; i<ei; i++)
        //    {
        //        result += result[i];
        //    }
        //    return result;
        //}

        public void AutentificationQuery()
        {
            string postData = "exec=NULL&confirm_action=NULL&referer=&email=&password=";
            byte[] bytesArray = Encoding.UTF8.GetBytes(postData);
            WebRequest request = WebRequest.Create(@"http://localhost:8080/login");

            //((HttpWebRequest)request).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0";
            request.Method = "POST";
            //((HttpWebRequest)request).Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            request.ContentLength = bytesArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(bytesArray, 0, bytesArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();

            StreamReader sr = new StreamReader(dataStream);
            string responseFromServer = sr.ReadToEnd();
        }
    }
}