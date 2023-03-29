namespace ChatClient.model.dto
{
    internal class AccountSearchDTO : AccountDTO
    {
        private string _Email;

        private HashSet<Role> _Roles = new HashSet<Role>();

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
                    _Email = value;
                }
                else
                {
                    throw new InvalidInputException("email field is mandatory");
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

        public void SetRoles(HashSet<Role> roles)
        {
            this._Roles = roles;
        }

        public AccountSearchDTO(string firstName, string lastName, string email, string[] roles) : base(firstName, lastName)
        {
            Email = email;
            foreach (string value in roles)
            {
                AddRole(value);
            }
        }
    }
}