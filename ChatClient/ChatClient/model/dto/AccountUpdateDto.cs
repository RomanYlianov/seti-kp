using System.Text.RegularExpressions;

namespace ChatClient.model.dto
{
    internal class AccountUpdateDto : AccountDTO
    {
        private int _Id;

        private string _Email;

        public string _Password;

        private HashSet<Role> _Roles = new HashSet<Role>();

        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (value > 0)
                {
                    _Id = value;
                }
                else
                {
                    throw new InvalidInputException("id must be positive");
                }
            }
        }

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
                    throw new InvalidInputException("email field is mandatory");
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
                    throw new InvalidInputException("password field is mandatory");
                }
            }
        }

        public HashSet<Role> GetRoles()
        {
            return _Roles;
        }

        public void AddRole(string rolename)
        {
            if (Enum.TryParse(rolename, out Role role))
            {
                _Roles.Add(role);
            }
            else
            {
                throw new InvalidInputException("role value is not correct");
            }
        }

        public AccountUpdateDto(int id, string fname, string lname, string email, string password, string[] roles) : base(fname, lname)
        {
            Id = id;
            Email = email;
            Password = password;
            foreach (string value in roles)
            {
                AddRole(value);
            }
        }
    }
}