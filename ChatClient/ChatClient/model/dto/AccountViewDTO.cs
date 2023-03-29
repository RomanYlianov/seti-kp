namespace ChatClient.model.dto
{
    internal class AccountViewDTO
    {
        private int _Id;

        private string? _FirstName;

        private string? _LastName;

        private string? _Email;

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

        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (value != null)
                {
                    _FirstName = value;
                }
                else
                {
                    throw new InvalidInputException("firstName field is mandatory");
                }
            }
        }

        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                if (value != null)
                    _LastName = value;
                else
                    throw new InvalidInputException("lastName is mandatory");
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
                    _Email = value;
                else
                    throw new InvalidInputException("email is mandatory");
            }
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

        public HashSet<Role> GetRoles()
        {
            return _Roles;
        }

       

        public override string ToString()
        {
            string rolesString = "[";
            foreach (Role role in _Roles)
            {
                rolesString += role + ", ";
            }
            rolesString = rolesString.Remove(rolesString.Length - 2);
            rolesString += ']';
            string info = "id " + Id + ", firstName " + FirstName + ", lastName " + LastName + ", email " + Email + ", roles " + rolesString;
            return info;
        }
    }
}