namespace ChatClient.model.dto
{
    internal class MessageSendDTO : MessageDTO
    {
        private string _ReceiverUserName;

        public string ReceiverUserName
        {
            get
            {
                return _ReceiverUserName;
            }
            set
            {
                if (value != null)
                {
                    _ReceiverUserName = value;
                }
                else
                {
                    throw new InvalidInputException("receiverUserNAme field is m andatory");
                }
            }
        }

        public MessageSendDTO(string receiverUserName, string messageText) : base(messageText)
        {
            ReceiverUserName = receiverUserName;
        }
    }
}