namespace ChatClient.model.dto
{
    internal class AccountDTO
    {
        private string _FirstName;

        private string _LastName;

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
                {
                    _LastName = value;
                }
                else
                {
                    throw new InvalidInputException("lastName field is mandatory");
                }
            }
        }

        public AccountDTO(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}