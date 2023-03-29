using System.Globalization;

namespace ChatClient.model.dto
{
    internal class MessageSearchDTO : MessageDTO
    {
        private string _SenderUserName;

        private string _ReceiverUserName;

        private DateTime? _CreationTimeStart;

        private DateTime? _CreationTimeEnd;

        private DateTime? _ModificationTimeStart;

        private DateTime? _ModificationTimeEnd;

        public string SenderUserName
        {
            get
            {
                return _SenderUserName;
            }
            set
            {
                if (value != null)
                {
                    _SenderUserName = value;
                }
                else
                {
                    throw new InvalidInputException("senderUserName field can't be null");
                }
            }
        }

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
                    throw new InvalidInputException("receiverUserName field can't be null");
                }
            }
        }

        public DateTime? CreationTimeStart
        {
            get
            {
                return _CreationTimeStart;
            }
            set
            {
                if (value != null)
                {
                    _CreationTimeStart = value;
                }
                else
                {
                    throw new InvalidInputException("creationTimeStart can't be null");
                }
            }
        }

        public DateTime? CreationTimeEnd
        {
            get
            {
                return _CreationTimeEnd;
            }
            set
            {
                if (value != null)
                {
                    _CreationTimeEnd = value;
                }
                else
                {
                    throw new InvalidInputException("creationTimeEnd can't be null");
                }
            }
        }

        public DateTime? ModificationTimeStart
        {
            get
            {
                return _ModificationTimeStart;
            }
            set
            {
                if (value != null)
                {
                    _ModificationTimeStart = value;
                }
                else
                {
                    throw new InvalidInputException("modificationTimeStart can't be null");
                }
            }
        }

        public DateTime? ModificationTimeEnd
        {
            get
            {
                return _ModificationTimeEnd;
            }
            set
            {
                if (value != null)
                {
                    _CreationTimeStart = value;
                }
                else
                {
                    throw new InvalidInputException("modificationTimeEnd can't be null");
                }
            }
        }

        public MessageSearchDTO(string senderUserName, string receiverUserName, string messageText, string? creationTimeStart, string? creationTimeEnd, string? modificationTimeStrat, string? modificationTimeEnd) : base(messageText)
        {
            SenderUserName = senderUserName;
            ReceiverUserName = receiverUserName;
            try
            {
                string pattern = "yyyy-MM-dd hh:mm:ss";
                if (!string.IsNullOrEmpty(creationTimeStart))
                {
                    CreationTimeStart = DateTime.ParseExact(creationTimeStart, pattern, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(creationTimeEnd))
                {
                    CreationTimeEnd = DateTime.ParseExact(creationTimeEnd, pattern, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(modificationTimeStrat))
                {
                    ModificationTimeStart = DateTime.ParseExact(modificationTimeStrat, pattern, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(modificationTimeEnd))
                {
                    ModificationTimeEnd = DateTime.ParseExact(modificationTimeEnd, pattern, CultureInfo.InvariantCulture);
                }
            }
            catch (FormatException ex)
            {
                throw new InvalidInputException(ex.Message);
            }
        }
    }
}