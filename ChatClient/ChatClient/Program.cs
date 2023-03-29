using ChatClient;
using ChatClient.model;
using ChatClient.model.dto;

namespace HelloWorld
{
    internal class Hello
    {
        private static void Main(string[] args)
        {
           RunClient();
        }

        private static void RunClient()
        {
            bool f0 = true; bool f1 = true;
            string? value;
            ApiChatClient api = new ApiChatClient();
            while (f0)
            {
                f1 = true;
                Console.WriteLine("chat system v 0.1");
                Console.WriteLine("пожалуйста выберите:");
                Console.WriteLine("1 - авторизация");
                Console.WriteLine("2 - пользователи");
                Console.WriteLine("3 - сообщения");
                Console.WriteLine("0 - выход");
                value = Console.ReadLine();
                switch (Validate(value, 0, 3))
                {
                    case 0:
                        f0 = false;
                        break;

                    case 1:
                        while (f1)
                        {
                            Console.WriteLine("пожалуйста выберите:");
                            Console.WriteLine("1 - авторизация");
                            Console.WriteLine("2 - регистрация");
                            Console.WriteLine("3 - выход из системы");
                            Console.WriteLine("4 - получение информации об авторизованном пользоватенле в системе");
                            Console.WriteLine("0 - выход");
                            value = Console.ReadLine();
                            switch (Validate(value, 0, 4))
                            {
                                case 0:
                                    f1 = false;
                                    break;

                                case 1:
                                    {
                                        Console.WriteLine("авторизация в системе...");
                                        Console.WriteLine("введите адрес электронной почты:");
                                        string email = Console.ReadLine();
                                        Console.WriteLine("введите пароль:");
                                        string password = Console.ReadLine();
                                        try
                                        {
                                            AccountLoginDTO account = new AccountLoginDTO(email, password);
                                            ResponseEntity<AccountViewDTO> result = api.Login(account);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 2:
                                    {
                                        Console.WriteLine("регистрация пользователя...");
                                        Console.WriteLine("введите фамилию:");
                                        string fname = Console.ReadLine();
                                        Console.WriteLine("введите имя:");
                                        string lname = Console.ReadLine();
                                        Console.WriteLine("введите адрес электронной почты:");
                                        string email = Console.ReadLine();
                                        Console.WriteLine("введите пароль:");
                                        string password = Console.ReadLine();
                                        try
                                        {
                                            AccountRegisterDTO account = new AccountRegisterDTO(fname, lname, email, password);
                                            ResponseEntity<AccountViewDTO> result = api.Register(account);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 3:
                                    {
                                        Console.WriteLine("выход из системы...");
                                        ResponseEntity<Object> result = api.Logout();
                                        result.PrintResponseDetails();
                                        Console.WriteLine("выход успешно произведен");
                                    }
                                    break;

                                case 4:
                                    {
                                        Console.WriteLine("получение информации об авторизованном пользлователе");
                                        ResponseEntity<AccountViewDTO> result = api.GetAuthorizedUser();
                                        result.PrintResponseDetails();
                                    }
                                    break;
                            }
                        }
                        break;

                    case 2:
                        while (f1)
                        {
                            Console.WriteLine("пожалуйста выберите:");
                            Console.WriteLine("1 - поиск пользователя");
                            Console.WriteLine("2 - обновление информации о пользователе");
                            Console.WriteLine("3 - удаление пользователя");
                            Console.WriteLine("4 - получение списка ролей пользователя");
                            Console.WriteLine("5 - добавление пользоваателя в роль");
                            Console.WriteLine("6 - удаление пользователя из роли");
                            Console.WriteLine("0 - выход");
                            value = Console.ReadLine();
                            switch (Validate(value, 0, 6))
                            {
                                case 0:
                                    f1 = false;
                                    break;

                                case 1:
                                    {
                                        Console.WriteLine("поиск пользователя...");
                                        Console.WriteLine("введите фамилию");
                                        string fname = Console.ReadLine();
                                        Console.WriteLine("введите имя");
                                        string lname = Console.ReadLine();
                                        Console.WriteLine("введите адрес электронной почты");
                                        string email = Console.ReadLine();
                                        Console.WriteLine("введите список ролей через запятую");
                                        string roles = Console.ReadLine();
                                        string[] data = roles.Split(",");
                                        try
                                        {
                                            Paging paging = EnterPaging();
                                            AccountSearchDTO account = new AccountSearchDTO(fname, lname, email, data);
                                            ResponseEntity<AccountViewDTO> result = api.SearchAccount(account, paging);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 2:
                                    {
                                        Console.WriteLine("обновление информации о пользователе...");
                                        Console.WriteLine("введите идентификатор пользователя для обновления");
                                        value = Console.ReadLine();
                                        Console.WriteLine("введите фамилию");
                                        string fname = Console.ReadLine();
                                        Console.WriteLine("введите имя");
                                        string lname = Console.ReadLine();
                                        Console.WriteLine("введите адрес электронной почты");
                                        string email = Console.ReadLine();
                                        Console.WriteLine("введите пароль");
                                        string password = Console.ReadLine();
                                        Console.WriteLine("введите список ролей через запятую");
                                        string roles = Console.ReadLine();
                                        string[] data = roles.Split(",");
                                        try
                                        {
                                            int id = validate(value);
                                            AccountUpdateDto account = new AccountUpdateDto(id, fname, lname, email, password, data);
                                            ResponseEntity<AccountViewDTO> result = api.UpdateAccount(account);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 3:
                                    {
                                        Console.WriteLine("удаление пользователя...");
                                        Console.WriteLine("введите идентификатор пользователя для удаления");
                                        value = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<Object> result = api.RemoveAccountById(id);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 4:
                                    {
                                        Console.WriteLine("получение ролей пользователя...");
                                        Console.WriteLine("введите идентификатор пользователя");
                                        value = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<Role> result = api.GetRolesForUserById(id);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 5:
                                    {
                                        Console.WriteLine("добавление пользователя в роль...");
                                        Console.WriteLine("введите идентификатор пользователя");
                                        value = Console.ReadLine();
                                        Console.WriteLine("введите роль");
                                        string rolename = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<AccountViewDTO> result = api.AddRoleForAccount(new AccountRoleDTO(id, rolename));
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 6:
                                    {
                                        Console.WriteLine("удаление пользователя из роли...");
                                        Console.WriteLine("введите идентификатор пользователя");
                                        value = Console.ReadLine();
                                        Console.WriteLine("введите роль");
                                        string rolename = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<AccountViewDTO> result = api.RemoveRoleForAccount(new AccountRoleDTO(id, rolename));
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    case 3:
                        while (f1)
                        {
                            Console.WriteLine("пожалуйста выберите:");
                            Console.WriteLine("1 - поиск сообщения");
                            Console.WriteLine("2 - получение сообщения по идентификатору");
                            Console.WriteLine("3 - отправка сообшения");
                            Console.WriteLine("4 - обновление информации о сообщении");
                            Console.WriteLine("5 - удалениие сообщения");
                            Console.WriteLine("6 - удаление сообщений по параметрам");
                            value = Console.ReadLine();
                            switch (Validate(value, 0, 6))
                            {
                                case 0:
                                    f1 = false;
                                    break;

                                case 1:
                                    {
                                        Console.WriteLine("поиск сообщений...");
                                        Console.WriteLine("введите адрес отправителя");
                                        string senderUserName = Console.ReadLine();
                                        Console.WriteLine("введите адрес получателя");
                                        string receiverUserName = Console.ReadLine();
                                        Console.WriteLine("введите текст сообщения");
                                        string messageText = Console.ReadLine();
                                        Console.WriteLine("введите начальную дату создания (необязательный параметр)");
                                        string? creationTimeStart = Console.ReadLine();
                                        Console.WriteLine("введите конечную дату создания (необязательный параметр)");
                                        string? creationTimeEnd = Console.ReadLine();
                                        Console.WriteLine("введите начальную дату изменения (необязательный параметр)");
                                        string? modificationTimeStart = Console.ReadLine();
                                        Console.WriteLine("введите конечную дату изменения (необязвательный параметр)");
                                        string modificationTimeEnd = Console.ReadLine();
                                        try
                                        {
                                            Paging paging = EnterPaging();

                                            ResponseEntity<MessageViewDTO> result = api.SearchMessage(new MessageSearchDTO(senderUserName, receiverUserName, messageText, creationTimeStart, creationTimeEnd, modificationTimeStart, modificationTimeEnd), paging);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 2:
                                    {
                                        Console.WriteLine("получение сообщения по идентификатору...");
                                        Console.WriteLine("введите идентификатор сообщения");
                                        value = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<MessageViewDTO> result = api.GetMessageById(id);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 3:
                                    {
                                        Console.WriteLine("отправка сообщения...");
                                        Console.WriteLine("введите адрес электронной почты получателя");
                                        string receiverUserName = Console.ReadLine();
                                        Console.WriteLine("введите текст сообщения");
                                        value = Console.ReadLine();
                                        try
                                        {
                                            ResponseEntity<MessageViewDTO> result = api.SendMessage(new MessageSendDTO(receiverUserName, value));
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 4:
                                    {
                                        Console.WriteLine("редактирование сообщения...");
                                        Console.WriteLine("введите идентификатор сообщения");
                                        value = Console.ReadLine();
                                        Console.WriteLine("введите текст сообщения");
                                        string messageText = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<MessageViewDTO> result = api.UpdateMessage(new MessageUpdateDTO(messageText, id));
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 5:
                                    {
                                        Console.WriteLine("удаление сообщения...");
                                        Console.WriteLine("введите идентификатор сообшения");
                                        value = Console.ReadLine();
                                        try
                                        {
                                            int id = validate(value);
                                            ResponseEntity<Object> result = api.RemoveMessageById(id);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;

                                case 6:
                                    {
                                        Console.WriteLine("удаление сообщений по параметрам...");
                                        Console.WriteLine("введите адрес отправителя");
                                        string senderUserName = Console.ReadLine();
                                        Console.WriteLine("введите адрес получателя");
                                        string receiverUserName = Console.ReadLine();
                                        Console.WriteLine("введите текст сообщения");
                                        string messageText = Console.ReadLine();
                                        Console.WriteLine("введите начальную дату создания (необязательный параметр)");
                                        string creationTimeStart = Console.ReadLine();
                                        Console.WriteLine("введите конечную дату создания (необязательный параметр)");
                                        string creationTimeEnd = Console.ReadLine();
                                        Console.WriteLine("введите начальную дату изменения (необязательный параметр)");
                                        string modificationTimeStart = Console.ReadLine();
                                        Console.WriteLine("введите конечную дату изменения (необязвательный параметр)");
                                        string modificationTimeEnd = Console.ReadLine();
                                        try
                                        {
                                            MessageSearchDTO message = new MessageSearchDTO(senderUserName, receiverUserName, messageText, creationTimeStart, creationTimeEnd, modificationTimeStart, modificationTimeEnd);
                                            ResponseEntity<Object> result = api.RemoveMessagesByParameters(message);
                                            result.PrintResponseDetails();
                                        }
                                        catch (ResponseException ex)
                                        {
                                            Console.WriteLine("ошибка выполнения запроса; код ошибки {0}, сообщение {1} ", ex.StatusCode, ex.Message);
                                        }
                                        catch (InvalidInputException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }

            Paging EnterPaging()
            {
                Paging paging = new Paging();
                try
                {
                    Console.WriteLine("пагинация: введите номер страницы (по умолчанию 0)");
                    string? arg = Console.ReadLine();
                    if (!string.IsNullOrEmpty(arg))
                    {
                        int page = int.Parse(arg);
                        paging.From = page;
                    }                    
                    Console.WriteLine("пагинация: введите количество выводимых данных (по умолчанию 10)");
                    arg = Console.ReadLine();
                    if (!string.IsNullOrEmpty(arg))
                    {
                        int size = int.Parse(arg);
                        paging.Size = size;
                    }                    
                }
                catch (Exception ex)
                {
                    throw new InvalidInputException(ex.Message);
                }
                return paging;
            }

            int validate(string value)
            {
                int id = -1;
                if (int.TryParse(value, out id))
                {
                    if (id < 0)
                    {
                        throw new InvalidInputException("идентификатор дложен быть положительным");
                    }
                }
                else
                {
                    throw new InvalidInputException("идентификатор дложен быть числом");
                }
                return id;
            }

            int Validate(string? input, int minvalue, int maxvalue)
            {
                int index = -1;
                if (int.TryParse(input, out index))
                {
                    if (index < minvalue || index > maxvalue)
                    {
                        Console.WriteLine("разрешенный диапазон: [" + minvalue + ".." + maxvalue + "]");
                    }
                }
                else
                {
                    Console.WriteLine("введено не число");
                }
                return index;
            }
        }
    }
}