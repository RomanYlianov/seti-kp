using ChatClient.model;
using ChatClient.model.dto;
using System.Net;

namespace ChatClient
{
    internal class MockResponseConverter
    {
        private const string HOST = "localhost";

        private const int PORT = 8080;

        private const int RESPONSE_SIZE = 1024;

        #region Authorization

        public void Login()
        {
            string value = ResponseStore.AUTH.LOGIN;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                AccountViewDTO? entity = DeserializeAccount(body);

                if (entity != null)
                {
                    result.Body = new AccountViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void Register()
        {
            string value = ResponseStore.AUTH.REGISTER;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                AccountViewDTO? entity = DeserializeAccount(body);

                if (entity != null)
                {
                    result.Body = new AccountViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void Logout()
        {
            string value = ResponseStore.AUTH.LOGOUT;

            ResponseEntity<Object> result = new ResponseEntity<Object>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            result.PrintResponseDetails();
        }

        public void GetAuthorizedUser()
        {
            string value = ResponseStore.AUTH.USER_INFO;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                AccountViewDTO? entity = DeserializeAccount(body);

                if (entity != null)
                {
                    result.Body = new AccountViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        #endregion Authorization

        #region Account

        public void SearchAccount()
        {
            string value = ResponseStore.ACCOUNT.SEARCH_ACCOUNT;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            //string body = "[{\"id\":1,\"firstName\":\"Ivanov\",\"lastName\":\"Ivan\",\"roles\":[\"USER\"],\"email\":\"user@gmail.com\"},{\"id\":2,\"firstName\":\"Petrov\",\"lastName\":\"Petr\",\"roles\":[\"USER\"],\"email\":\"petya@gmail.com\"}]";
            string? body = GetResponseBody(value);
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

            result.PrintResponseDetails();
        }

        public void UpdateAccount()
        {
            string value = ResponseStore.ACCOUNT.UPDATE_ACCOUNT;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                AccountViewDTO? entity = DeserializeAccount(body);

                if (entity != null)
                {
                    result.Body = new AccountViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void RemoveAccountById()
        {
            string value = ResponseStore.ACCOUNT.REMOVE_ACCOUNT;

            ResponseEntity<Object> result = new ResponseEntity<Object>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            result.PrintResponseDetails();
        }

        public void GetRolesForUserById()
        {
            string value = ResponseStore.ACCOUNT.GET_ROLES_FOR_USER_BY_ID;

            ResponseEntity<Role> result = new ResponseEntity<Role>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            List<Role> list = new List<Role>();
            string? body = GetResponseBody(value);
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
            result.PrintResponseDetails();
        }

        public void AddRoleForAccount()
        {
            string value = ResponseStore.ACCOUNT.ADD_ROLE_FOR_ACCOUNT;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                AccountViewDTO? entity = DeserializeAccount(body);

                if (entity != null)
                {
                    result.Body = new AccountViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void RemoveRoleForAccount()
        {
            string value = ResponseStore.ACCOUNT.REMOVE_ROLE_FOR_ACCOUNT;

            ResponseEntity<AccountViewDTO> result = new ResponseEntity<AccountViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                AccountViewDTO? entity = DeserializeAccount(body);

                if (entity != null)
                {
                    result.Body = new AccountViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        #endregion Account

        #region Mesage

        public void SearchMessage()
        {
            string value = ResponseStore.MESSAGES.SEARH_MESSAGE;

            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            //string body = "[{\"id\":2,\"senderUsername\":\"admin@gmail.com\",\"receiverUsername\":\"user@gmail.com\",\"messageText\":\"hi, Ivan\",\"creationTime\":\"2021-12-31T21:00:00.000+00:00\",\"modificationTime\":null},{\"id\":6,\"senderUsername\":\"admin@gmail.com\",\"receiverUsername\":\"user@gmail.com\",\"messageText\":\"wassup my boy\",\"creationTime\":\"2023-03-07T10:19:02.813+00:00\",\"modificationTime\":null}]";
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

            result.PrintResponseDetails();
        }

        public void GetMessageById()
        {
            string value = ResponseStore.MESSAGES.GET_MESSAGE_BY_ID;

            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                MessageViewDTO? entity = DeserializeMessage(body);

                if (entity != null)
                {
                    result.Body = new MessageViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void SendMessage()
        {
            string value = ResponseStore.MESSAGES.SEND_MESSAGE;

            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                MessageViewDTO? entity = DeserializeMessage(body);

                if (entity != null)
                {
                    result.Body = new MessageViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void UpdateMessage()
        {
            string value = ResponseStore.MESSAGES.UPDATE_MESSAGE;

            ResponseEntity<MessageViewDTO> result = new ResponseEntity<MessageViewDTO>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            string? body = GetResponseBody(value);
            if (body != null)
            {
                MessageViewDTO? entity = DeserializeMessage(body);

                if (entity != null)
                {
                    result.Body = new MessageViewDTO[] { entity };
                }
            }

            result.PrintResponseDetails();
        }

        public void RemoveMessageById()
        {
            string value = ResponseStore.MESSAGES.REMOVE_MESSAGE;

            ResponseEntity<Object> result = new ResponseEntity<Object>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            result.PrintResponseDetails();
        }

        public void RemoveMessagesByParameters()
        {
            string value = ResponseStore.MESSAGES.REMOVE_SEVERAL_MESSAGES;

            ResponseEntity<Object> result = new ResponseEntity<Object>();

            string version = GetHttpVersion(value);
            result.Version = version;

            HttpStatusCode code = GetStatusCode(value);
            result.StatusCode = code;

            string[] array = GetConfigParams(value);
            result.Config = array;

            Cookie? cookie = GetCookie(value);
            if (cookie != null)
            {
                result.Cookies = new Cookie[] { cookie };
            }

            string? content = GetContentType(value);
            if (content != null)
            {
                result.ContentType = content;
            }

            DateTime? date = GetDate(value);
            if (date != null)
            {
                result.Date = date;
            }

            ConnectionType? type = GetConnectionType(value);
            if (type != null)
            {
                result.ConnectionType = type;
            }

            result.PrintResponseDetails();
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
                //["",""]
                //[{},{}]

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
    }
}