using System.Text.RegularExpressions;

namespace ChatClient.model.dto
{
    internal class AccountLoginDTO
    {
        private string? _Email;

        private string? _Password;

        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                if (value != null)
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(value);
                    if (match.Success)
                        _Email = value;
                    else
                        throw new InvalidInputException("email is not correct");
                }
                else
                {
                    throw new InvalidInputException("email is mandatory");
                }
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (value != null)
                {
                    _Password = value;
                }
                else
                {
                    throw new InvalidInputException("password is mandatory");
                }
            }
        }

        public AccountLoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}