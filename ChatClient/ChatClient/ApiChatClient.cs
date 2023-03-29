using ChatClient.model;
using ChatClient.model.dto;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal class ApiChatClient
    {
        private const string HOST = "localhost";

        private const int PORT = 8080;

        private const string PATH = "session.txt";

        private const int RESPONSE_SIZE = 1024;

        private class User
        {
            public string? Login;

            public string? Password;

            public void Invalidate()
            {
                Login = null;
                Password = null;
            }
        }

        private User AuthorizedUser;

        public ApiChatClient()
        {
            RestoreSessionInfo();
        }

        ~ApiChatClient()
        {
            SaveSessionInfo();
        }

        private void SaveSessionInfo()
        {
            FileInfo file = new FileInfo(PATH);
            using (StreamWriter writer = file.CreateText())
            {
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    writer.WriteLine(string.Join(":", AuthorizedUser.Login, AuthorizedUser.Password));
                }
            }
        }

        private void RestoreSessionInfo()
        {
            FileInfo file = new FileInfo(PATH);
            AuthorizedUser = new User();
            if (file.Exists)
            {
                using (StreamReader reader = file.OpenText())
                {
                    string? message = reader.ReadLine();
                    if (message != null)
                    {
                        string login = message.Split(":")[0];
                        string password = message.Split(":")[1];
                        AuthorizedUser.Login = login;
                        AuthorizedUser.Password = password;
                    }
                }
            }
        }

        #region Authorization

        public ResponseEntity<AccountViewDTO> Login(AccountLoginDTO account)
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "POST /login HTTP/1.1\r\nAuthorization: Basic #1#\r\nHost: #host#\r\nConnection: close\r\n\r\n\r\nContent-Type: application/json\r\nContent-Length: 0";
                string credentials = FormCredentials(account.Email, account.Password);
                message = message.Replace("#1#", credentials);
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    AccountViewDTO? entity = DeserializeAccount(body);

                    if (entity != null)
                    {
                        result.Body = new AccountViewDTO[] { entity };
                    }
                }

                //end parse response
                //set current user login
                AuthorizedUser = new User();
                AuthorizedUser.Login = account.Email;
                AuthorizedUser.Password = account.Password;
                //save to file
                SaveSessionInfo();
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<AccountViewDTO> Register(AccountRegisterDTO account)
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "POST /registration HTTP/1.1\r\nHost: #host#\r\nConnection: close\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n\t\"firstName\" : \"#fn#\",\r\n\t\"lastName\" : \"#ln#\",\r\n\t\"email\": \"#email#\",\r\n\t\"password\": \"#password#\"\r\n}\r\n";
                message = message.Replace("#host#", string.Join(":", HOST, PORT));
                message = message.Replace("#fn#", account.FirstName);
                message = message.Replace("#ln#", account.LastName);
                message = message.Replace("#email#", account.Email);
                message = message.Replace("#password#", account.Password);
                int contentSize = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentSize.ToString());
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    AccountViewDTO? entity = DeserializeAccount(body);

                    if (entity != null)
                    {
                        result.Body = new AccountViewDTO[] { entity };
                    }
                }

                //end parse response
                //set authorized user info
                AuthorizedUser = new User();
                AuthorizedUser.Login = account.Email;
                AuthorizedUser.Password = account.Password;
                //save to file
                SaveSessionInfo();
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<Object> Logout()
        {
            ResponseEntity<Object> result = new ResponseEntity<Object>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "POST /logout HTTP/1.1\r\nHost: #host#\r\n\r\nConnection: close\r\nContent-Type: application/json\r\nContent-Length: 0\r\n";
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                //end parse response
                //reset authorized user state
                AuthorizedUser.Invalidate();
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<AccountViewDTO> GetAuthorizedUser()
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "GET /user-info HTTP/1.1\r\n#1#Host: localhost\r\nConnection: close\r\n\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    AccountViewDTO? entity = DeserializeAccount(body);

                    if (entity != null)
                    {
                        result.Body = new AccountViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        #endregion Authorization

        #region Account

        public ResponseEntity<AccountViewDTO> SearchAccount(AccountSearchDTO dto, Paging paging)
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "GET /accounts/search?#params# HTTP/1.1\r\n#1#Host: #host#\r\n\r\nConnection: close\r\n";
                string searchParams = "firstName=#fn#&lastName=#ln#&email=#email#&roles=#roles#";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                searchParams = searchParams.Replace("#fn#", dto.FirstName);
                searchParams = searchParams.Replace("#ln#", dto.LastName);
                searchParams = searchParams.Replace("#email#", dto.Email);

                string rolesString = "";
                foreach (Role role in dto.GetRoles())
                {
                    rolesString += role + ",";
                }
                rolesString = rolesString.Remove(rolesString.Length - 1);
                searchParams = searchParams.Replace("#roles#", rolesString);
                if (paging.From != null)
                {
                    searchParams += "&from=" + paging.From;
                }
                if (paging.Size != null)
                {
                    searchParams += "&size=" + paging.Size;
                }
                searchParams = searchParams.Replace("#from#", paging.From.ToString());
                searchParams = searchParams.Replace("#size#", paging.Size.ToString());
                message = message.Replace("#params#", searchParams);

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                List<int> indexes = new List<int>();
                byte f = 0;
                AccountViewDTO[]? dTOs = null;
                if (body != null)
                {
                    indexes.Add(body.IndexOf('{'));
                    for (int i = 0; i < body.Length; i++)
                    {
                        if (body[i].Equals('{'))
                        {
                            f++;
                        }
                        if (body[i].Equals('}'))
                        {
                            f--;
                        }
                        if (f == 0)
                        {
                            if (body[i].Equals(','))
                            {
                                indexes.Add(i + 1);
                            }
                        }
                    }
                    string[] temp = new string[indexes.Count];
                    if (indexes.Count == 0)
                    {
                        body = body.Substring(1);
                        body = body.Remove(body.Length - 1);
                        temp = new string[1] { body };
                    }
                    if (indexes.Count > 0)
                    {
                        int ci = 0;
                        for (int i = 0; i < indexes.Count - 1; i++)
                        {
                            temp[ci] = body.Substring(indexes[i], indexes[i + 1] - indexes[i]);
                            ci++;
                        }
                        temp[ci] = body.Substring(indexes[ci], body.Length - indexes[ci]);
                    }
                    dTOs = new AccountViewDTO[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        AccountViewDTO? item = DeserializeAccount(temp[i]);
                        if (item != null)
                        {
                            dTOs[i] = item;
                        }
                        else
                        {
                            dTOs = null;
                            break;
                        }
                    }
                }
                result.Body = dTOs;

                //end parse response
                socket.Close();
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public ResponseEntity<AccountViewDTO> UpdateAccount(AccountUpdateDto dto)
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                //var message = "PUT /accounts/#id# HTTP/1.1\r\n#1#Host: #host#\r\nConnection: timeout=60\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n\t\"firstName\": \"#fn#\",\r\n\t\"lastName\": \"#ln#\",\r\n\t\"email\": \"#email#\",\r\n\t\"roles\": [#roles#],\r\n\t\"password\" : \"#password#\"\r\n}\r\n";
                //var message = "PUT /accounts/#id# HTTP/1.1\r\n#1#Host: #host#\r\nConnection: close\r\n\r\nContent-Type: application/json; charset=utf-8\r\nContent-Length: #cl#\r\n\r\n{\r\n\t\"firstName\": \"#fn#\",\r\n\t\"lastName\": \"#ln#\",\r\n\t\"email\": \"#email#\",\r\n\t\"roles\": [#roles#],\r\n\t\"password\" : \"#password#\r\n\"}";
                var message = "PUT /accounts/#id# HTTP/1.1\r\n#1#Host: #host#\r\nConnection: close\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n    \"firstName\": \"#fn#\",\r\n    \"lastName\": \"#ln#\",\r\n    \"email\": \"#email#\",\r\n    \"roles\": [#roles#],\r\n    \"password\" : \"#password\"\r\n}";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", dto.Id.ToString());
                message = message.Replace("#fn#", dto.FirstName);
                message = message.Replace("#ln#", dto.LastName);
                message = message.Replace("#email#", dto.Email);
                message = message.Replace("#password#", dto.Password);
                string rolesString = "";
                foreach (Role role in dto.GetRoles())
                {
                    rolesString += "\"" + role + "\",";
                }

                rolesString = rolesString.Remove(rolesString.Length - 1);
                message = message.Replace("#roles#", rolesString);
                int contentLength = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentLength.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    AccountViewDTO? entity = DeserializeAccount(body);

                    if (entity != null)
                    {
                        result.Body = new AccountViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<Object> RemoveAccountById(int id)
        {
            ResponseEntity<Object> result = new ResponseEntity<Object>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "DELETE /accounts/#id# HTTP/1.1\r\n#1#Host: #host#\r\n\r\nConnection: timeout=60\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", id.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<Role> GetRolesForUserById(int id)
        {
            ResponseEntity<Role> result = new ResponseEntity<Role>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("соединение успешно установлено");
                var message = "GET /accounts/#id#/roles HTTP/1.1\r\n#1#Host: #host#\r\n\r\nConnection: timeout=60\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", id.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                List<Role> list = new List<Role>();
                string? body = GetResponseBody(response);
                if (body != null)
                {
                    string[] rolesString = body.Split(',');
                    foreach (string item in rolesString)
                    {
                        string val = item;
                        if (val.StartsWith('"'))
                            val = val.Substring(1);
                        if (val.EndsWith('"'))
                            val = val.Remove(val.Length - 1);
                        if (Enum.TryParse(val, out Role role))
                        {
                            list.Add(role);
                        }
                        else
                        {
                            list = new List<Role>();
                            break;
                        }
                    }
                }
                if (list.Count > 0)
                {
                    result.Body = list.ToArray();
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<AccountViewDTO> AddRoleForAccount(AccountRoleDTO dto)
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);

                Console.WriteLine("соединение успешно установлено");

                var message = "POST /accounts/add-role HTTP/1.1\r\n#1#Host: #host#\r\nConnection: timeout=60\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n    \"accountId\": #id#,\r\n    \"role\": \"#roles#\"\r\n}"; ;

                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", dto.AccountId.ToString());
                message = message.Replace("#roles#", dto.GetRole().ToString());
                int contentLength = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentLength.ToString());
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    AccountViewDTO? entity = DeserializeAccount(body);

                    if (entity != null)
                    {
                        result.Body = new AccountViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<AccountViewDTO> RemoveRoleForAccount(AccountRoleDTO dto)
        {
            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);

                Console.WriteLine("соединение успешно установлено");

                var message = "POST /accounts/remove-role HTTP/1.1\r\n#1#Host: #host#\r\nConnection: timeout=60\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n    \"accountId\": #id#,\r\n    \"role\": \"#roles#\"\r\n}"; ;

                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", dto.AccountId.ToString());
                message = message.Replace("#roles#", dto.GetRole().ToString());
                int contentLength = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentLength.ToString());
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    AccountViewDTO? entity = DeserializeAccount(body);

                    if (entity != null)
                    {
                        result.Body = new AccountViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        #endregion Account

        #region Mesage

        public ResponseEntity<MessageViewDTO> SearchMessage(MessageSearchDTO dto, Paging paging)
        {
            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "GET /messages/search?#params# HTTP/1.1\r\n#1#Host: #host#\r\n\r\nConnection: close\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                string searchParams = "senderUsername=#sun#&receiverUsername=#run#&messageText=#mt#";
                searchParams = searchParams.Replace("#sun#", dto.SenderUserName);
                searchParams = searchParams.Replace("#run#", dto.ReceiverUserName);
                searchParams = searchParams.Replace("#mt#", dto.MessageText);
                if (paging.From != null)
                {
                    searchParams += "&from=" + paging.From;
                }
                if (paging.Size != null)
                {
                    searchParams += "&size=" + paging.Size;
                }
                if (dto.CreationTimeStart != null)
                {
                    searchParams += "&creationTimeStart=" + dto.CreationTimeStart;
                }
                if (dto.CreationTimeEnd != null)
                {
                    searchParams += "&creationTimeEnd=" + dto.CreationTimeEnd;
                }
                if (dto.ModificationTimeStart != null)
                {
                    searchParams += "&modificationTimeStart=" + dto.ModificationTimeStart;
                }
                if (dto.ModificationTimeEnd != null)
                {
                    searchParams += "&modificationTimeEnd=" + dto.ModificationTimeEnd;
                }
                message = message.Replace("#params#", searchParams);

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                List<int> indexes = new List<int>();
                byte f = 0;
                MessageViewDTO[]? dTOs = null;
                if (body != null)
                {
                    indexes.Add(body.IndexOf('{'));
                    for (int i = 0; i < body.Length; i++)
                    {
                        if (body[i].Equals('{'))
                        {
                            f++;
                        }
                        if (body[i].Equals('}'))
                        {
                            f--;
                        }
                        if (f == 0)
                        {
                            if (body[i].Equals(','))
                            {
                                indexes.Add(i + 1);
                            }
                        }
                    }
                    string[] temp = new string[indexes.Count];
                    if (indexes.Count == 0)
                    {
                        body = body.Substring(1);
                        body = body.Remove(body.Length - 1);
                        temp = new string[1] { body };
                    }
                    if (indexes.Count > 0)
                    {
                        int ci = 0;
                        for (int i = 0; i < indexes.Count - 1; i++)
                        {
                            temp[ci] = body.Substring(indexes[i], indexes[i + 1] - indexes[i]);
                            ci++;
                        }
                        temp[ci] = body.Substring(indexes[ci], body.Length - indexes[ci]);
                    }
                    dTOs = new MessageViewDTO[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        MessageViewDTO? item = DeserializeMessage(temp[i]);
                        if (item != null)
                        {
                            dTOs[i] = item;
                        }
                        else
                        {
                            dTOs = null;
                            break;
                        }
                    }
                }
                result.Body = dTOs;

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }

            return result;
        }

        public ResponseEntity<MessageViewDTO> GetMessageById(int id)
        {
            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "GET /messages/#id# HTTP/1.1\r\n#1#Host: #host#\r\n\r\nConnection: close\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", id.ToString());
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    MessageViewDTO? entity = DeserializeMessage(body);

                    if (entity != null)
                    {
                        result.Body = new MessageViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<MessageViewDTO> SendMessage(MessageSendDTO dto)
        {
            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "POST /messages HTTP/1.1\r\n#1#Host: #host#\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n\t\"receiverUsername\": \"#run#\",\r\n\t\"messageText\": \"#mt#\"\r\n}"; ;
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#run#", dto.ReceiverUserName);
                message = message.Replace("#mt#", dto.MessageText);
                int contentSize = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentSize.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    MessageViewDTO? entity = DeserializeMessage(body);

                    if (entity != null)
                    {
                        result.Body = new MessageViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<MessageViewDTO> UpdateMessage(MessageUpdateDTO dto)
        {
            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "PUT /messages/#id# HTTP/1.1\r\n#1#Host: #host#\r\nConnection: close\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n\t\"messageText\": \"#mt#\"\r\n}\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", dto.Id.ToString());
                message = message.Replace("#mt#", dto.MessageText);
                int contentSize = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentSize.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                string? body = GetResponseBody(response);
                if (body != null)
                {
                    MessageViewDTO? entity = DeserializeMessage(body);

                    if (entity != null)
                    {
                        result.Body = new MessageViewDTO[] { entity };
                    }
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<Object> RemoveMessageById(int id)
        {
            ResponseEntity<Object> result = new ResponseEntity<Object>();

            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "DELETE /messages/#id# HTTP/1.1\r\n#1#Host: #host#\r\n\r\nConnection: close\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#id#", id.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        public ResponseEntity<Object> RemoveMessagesByParameters(MessageSearchDTO dto)
        {
            ResponseEntity<Object> result = new ResponseEntity<Object>();

            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(HOST, PORT);
                Console.WriteLine("подключение успешно установлено");
                var message = "POST /messages/delete HTTP/1.1\r\n#1#Host: #host#\r\nConnection: close\r\nContent-Type: application/json\r\nContent-Length: #cl#\r\n\r\n{\r\n\t\"senderUsername\" : \"#sun#\",\r\n\t\"receiverUsername\": \"#run#\",\r\n\t\"messageText\": \"#mt#\"\r\n}\r\n";
                if (AuthorizedUser.Login != null && AuthorizedUser.Password != null)
                {
                    string credentials = FormCredentials(AuthorizedUser.Login, AuthorizedUser.Password);
                    string authString = "Authorization: Basic " + credentials + "\r\n";
                    message = message.Replace("#1#", authString);
                }
                else
                {
                    message = message.Replace("#1#", "");
                }
                message = message.Replace("#host#", string.Join(":", HOST, PORT));

                message = message.Replace("#sun#", dto.SenderUserName);
                message = message.Replace("#run#", dto.ReceiverUserName);
                message = message.Replace("#mt#", dto.MessageText);
                int contentSize = GetRequestBodySize(message);
                message = message.Replace("#cl#", contentSize.ToString());

                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = socket.Send(messageBytes, SocketFlags.None);
                var responseBytes = new byte[RESPONSE_SIZE];
                var bytes = socket.Receive(responseBytes, SocketFlags.None);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                //parse response

                string version = GetHttpVersion(response);
                result.Version = version;

                HttpStatusCode code = GetStatusCode(response);
                result.StatusCode = code;

                string[] array = GetConfigParams(response);
                result.Config = array;

                Cookie? cookie = GetCookie(response);
                if (cookie != null)
                {
                    result.Cookies = new Cookie[] { cookie };
                }

                string? content = GetContentType(response);
                if (content != null)
                {
                    result.ContentType = content;
                }

                DateTime? date = GetDate(response);
                if (date != null)
                {
                    result.Date = date;
                }

                ConnectionType? type = GetConnectionType(response);
                if (type != null)
                {
                    result.ConnectionType = type;
                }

                //end parse response
            }
            catch (SocketException)
            {
                Console.WriteLine("подключение не удалось");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
            }
            return result;
        }

        #endregion Mesage

        private AccountViewDTO? DeserializeAccount(string body)
        {
            AccountViewDTO? entity = null;

            int si = body.IndexOf("\"id\"");
            int ei = si;
            string? value = Parse(body, si, ei);
            try
            {
                if (value != null)
                {
                    if (int.TryParse(value, out int id))
                    {
                        entity = new AccountViewDTO();
                        entity.Id = id;
                    }
                }
            }
            catch (Exception)
            {
                entity = null;
            }
            if (entity != null)
            {
                si = body.IndexOf("\"firstName\"");
                ei = si;
                value = Parse(body, si, ei);
                if (value != null)
                {
                    entity.FirstName = value;
                }
                else
                {
                    entity = null;
                }
            }
            if (entity != null)
            {
                si = body.IndexOf("\"lastName\"");
                ei = si;
                value = Parse(body, si, ei);
                if (value != null)
                {
                    entity.LastName = value;
                }
                else
                {
                    entity = null;
                }
            }
            if (entity != null)
            {
                si = body.IndexOf("\"roles\"");
                ei = si;
                string[]? array = GetStringArray(body, si, ei);
                if (array != null)
                {
                    foreach (string val in array)
                    {
                        entity.AddRole(val);
                    }
                }
                else
                {
                    entity = null;
                }
            }

            if (entity != null)
            {
                si = body.IndexOf("\"email\"");
                ei = si;
                value = Parse(body, si, ei);
                if (value != null)
                {
                    value = value.Remove(value.Length - 2);
                    entity.Email = value;
                }
                else
                {
                    entity = null;
                }
            }
            return entity;
        }

        private MessageViewDTO? DeserializeMessage(string body)
        {
            MessageViewDTO? entity = null;
            int si = body.IndexOf("\"id\"");
            int ei = si;
            string? value = Parse(body, si, ei);
            try
            {
                if (int.TryParse(value, out int id))
                {
                    entity = new MessageViewDTO();
                    entity.Id = id;
                }
            }
            catch (Exception)
            {
                entity = null;
            }
            if (entity != null)
            {
                si = body.IndexOf("\"senderUsername\"");
                ei = si;
                value = Parse(body, si, ei);
                if (value != null)
                {
                    entity.Sender = value;
                }
                else
                {
                    entity = null;
                }
            }
            if (entity != null)
            {
                si = body.IndexOf("\"receiverUsername\"");
                ei = si;
                value = Parse(body, ei, si);
                if (value != null)
                {
                    entity.Receiver = value;
                }
                else
                {
                    entity = null;
                }
            }
            if (entity != null)
            {
                si = body.IndexOf("\"messageText\"");
                ei = si;
                value = Parse(body, si, ei);
                if (value != null)
                {
                    entity.MessageText = value;
                }
            }
            if (entity != null)
            {
                si = body.IndexOf("\"creationTime\"");
                ei = si;
                DateTime? date = ParseDateTime(body, si);
                if (date != null)
                {
                    entity.CreationTime = date;
                }
                else
                {
                    entity = null;
                }
            }
            if (entity != null)
            {
                si = body.IndexOf("\"modificationTime\"");
                ei = si;
                DateTime? date = ParseDateTime(body, si);
                if (date != null)
                {
                    entity.ModificationTime = date;
                }
            }
            return entity;
        }

        private DateTime? ParseDateTime(string body, int si)
        {
            DateTime? date = null;
            si += 1;
            string val = body.Substring(si);
            int ei = val.IndexOf(':') + 2;
            while (ei < val.Length)
            {
                if (val[ei].Equals('"'))
                {
                    break;
                }
                ei++;
            }
            val = val.Remove(ei);
            try
            {
                string dateString = val.Substring(val.IndexOf(':')).Substring(val.IndexOf(':'));
                if (!dateString.Equals("null"))
                {
                    if (DateTime.TryParse(dateString, out DateTime time))
                    {
                        date = time;
                    }
                }
            }
            catch (Exception)
            {
                date = null;
            }
            return date;
        }

        private string? Parse(string body, int si, int ei)
        {
            if (si >= 0)
            {
                while (ei < body.Length)
                {
                    if (body[ei].Equals(','))
                    {
                        break;
                    }
                    ei++;
                }
                string value = body.Substring(si, ei - si);
                value = value.Split(':')[1];
                if (value.StartsWith('"'))
                    value = value.Substring(1);
                if (value.EndsWith('"'))
                    value = value.Remove(value.Length - 1);
                return value;
            }
            else
            {
                return null;
            }
        }

        private string[]? GetStringArray(string body, int si, int ei)
        {
            if (si >= 0)
            {
                string[] arr = new string[0];
                while (si < body.Length)
                {
                    if (body[si].Equals('['))
                        break;
                    si++;
                }
                ei = si;
                while (ei < body.Length)
                {
                    if (body[ei].Equals(']'))
                        break;
                    ei++;
                }
                arr = body.Substring(si + 1, ei - si - 1).Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = arr[i].Trim();
                    if (arr[i].StartsWith('"'))
                        arr[i] = arr[i].Substring(1);
                    if (arr[i].EndsWith('"'))
                        arr[i] = arr[i].Remove(arr[i].Length - 1);
                }
                return arr;
            }
            else
            {
                return null;
            }
        }

        private string GetHttpVersion(string resp)
        {
            return resp.Split(" ")[0];
        }

        private HttpStatusCode GetStatusCode(string resp)
        {
            string val = resp.Split(" ")[1];
            return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), val);
        }

        private string[] GetConfigParams(string resp)
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

        private Cookie? GetCookie(string resp)
        {
            int si = resp.IndexOf("\r\nSet-Cookie: ");
            if (si >= 0)
            {
                si += 13;
                int ei = si;
                while (ei < resp.Length)
                {
                    if (ei + 1 < resp.Length)
                    {
                        if (resp.Substring(ei).StartsWith("\r\n"))
                            break;
                    }
                    ei++;
                }
                string resultString = resp.Substring(si, ei - si);
                string[] arr = resultString.Split(";");
                Cookie cookie = new();

                cookie.Domain = HOST;
                cookie.Name = arr[0].Split("=")[0].Trim();
                cookie.Value = arr[0].Split("=")[1];
                cookie.Path = arr[1].Split("=")[1];
                cookie.HttpOnly = IsHttpOnly();
                bool IsHttpOnly()
                {
                    bool flag = false;
                    foreach (string item in arr)
                    {
                        if (item.Trim().Equals("HttpOnly", StringComparison.OrdinalIgnoreCase))
                        {
                            flag = true;
                            break;
                        }
                    }
                    return flag;
                }
                return cookie;
            }
            else
            {
                return null;
            }
        }

        private string? GetContentType(string resp)
        {
            int si = resp.IndexOf("\r\nContent-Type: ");
            if (si >= 0)
            {
                si += 2;
                int ei = si;
                while (ei < resp.Length)
                {
                    if (ei + 1 < resp.Length)
                    {
                        if (resp.Substring(ei).StartsWith("\r\n"))
                            break;
                    }
                    ei++;
                }

                string data = resp.Substring(si, ei - si);

                return data.Split(':')[1].Trim();
            }
            else
            {
                return null;
            }
        }

        private DateTime? GetDate(string resp)
        {
            int si = resp.IndexOf("\r\nDate: ");
            if (si >= 0)
            {
                si += 8;
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
            else
            {
                return null;
            }
        }

        private ConnectionType? GetConnectionType(string resp)
        {
            int si = resp.IndexOf("\r\nConnection: ");
            if (si >= 0)
            {
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
            else
            {
                return null;
            }
        }

        private string? GetResponseBody(string resp)
        {
            int index = resp.IndexOf("\r\n\r\n");
            if (index >= 0)
            {
                string data = resp.Split("\r\n\r\n")[1];

                if (IsManyElements(data))
                {
                    index = data.IndexOf('[');
                    data = data.Substring(index);
                    if (data.EndsWith("\r\n"))
                    {
                        data = data.Remove(data.Length - 2);
                    }
                }
                else
                {
                    if (data.IndexOf('{') >= 0)
                    {
                        data = data.Substring(data.IndexOf('{'));
                    }
                    if ((data.IndexOf('"') >= 0) && (data.IndexOf('{') == -1))
                    {
                        data = data.Substring(data.IndexOf('"'));
                    }

                    if (data.EndsWith("\r\n"))
                    {
                        data = data.Remove(data.Length - 2);
                    }
                    if (data.EndsWith(']'))
                        data = data.Remove(data.Length - 1);
                }

                return data;
            }
            else
            {
                return null;
            }

            bool IsManyElements(string data)
            {
                bool f = false;
                if ((data.IndexOf('[') != -1) && (data.IndexOf('{') != -1))
                {
                    if (data.IndexOf('[') < data.IndexOf('{'))
                    {
                        int ind = data.IndexOf('[');
                        byte ch = 0;
                        bool f1 = true;
                        int ci = ind;
                        ushort f2 = 0;
                        while (ci < data.Length)
                        {
                            if ((data[ci].Equals('"') || data[ci].Equals('{')) && f1)
                            {
                                ch++;
                                ci++;
                                f1 = false;
                                f2++;
                            }
                            if ((data[ci].Equals('"') || data[ci].Equals('}')) && !f1)
                            {
                                ch--;
                                ci++;
                                f1 = true;
                                f2++;
                            }
                            ci++;
                            if (ch == 0)
                            {
                                if ((f2 >= 2) && (f2 % 2 == 0))
                                {
                                    if (data[ci].Equals(','))
                                    {
                                        f = true;
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return f;
            }
        }

        private string FormCredentials(string username, string password)
        {
            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            return encoded;
        }

        private int GetRequestBodySize(string request)
        {
            int size = request.Split("\r\n\r\n")[1].Length;
            return size;
        }
    }
}