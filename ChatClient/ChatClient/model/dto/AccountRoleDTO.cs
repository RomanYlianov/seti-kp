namespace ChatClient.model.dto
{
    internal class AccountRoleDTO
    {
        private int _AccountId;

        private Role _Role = new Role();

        public int AccountId
        {
            get
            {
                return _AccountId;
            }
            set
            {
                if (value >= 0)
                {
                    _AccountId = value;
                }
                else
                {
                    throw new InvalidInputException("accountId must be positive");
                }
            }
        }

        public Role GetRole()
        {
            return _Role;
        }

        public void SetRole(string rolename)
        {
            if (Enum.TryParse(rolename, out Role role))
            {
                _Role = role;
            }
            else
            {
                throw new InvalidInputException("role value is not correct");
            }
        }

        public AccountRoleDTO(int accountId, string role)
        {
            AccountId = accountId;
            SetRole(role);
        }
    }
}