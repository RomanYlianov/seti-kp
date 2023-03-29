namespace ChatClient.model.dto
{
    internal class MessageUpdateDTO : MessageDTO
    {
        private int _Id;

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

        public MessageUpdateDTO(string messageText, int id) : base(messageText)
        {
            this.Id = id;
        }
    }
}