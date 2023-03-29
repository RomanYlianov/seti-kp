namespace ChatClient.model.dto
{
    internal class MessageDTO
    {
        private string _MessageText;

        public string MessageText
        {
            get
            {
                return _MessageText;
            }
            set
            {
                if (value != null)
                {
                    _MessageText = value;
                }
                else
                {
                    throw new InvalidInputException("messageText field is mandatory");
                }
            }
        }

        public MessageDTO(string messageText)
        {
            MessageText = messageText;
        }
    }
}