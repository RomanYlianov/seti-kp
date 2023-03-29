namespace ChatClient.model.dto
{
    internal class MessageViewDTO
    {
        private int _Id;

        private string _Sender;

        private string _Receiver;

        private string? _MessageText;

        private DateTime? _CreationTime;

        private DateTime? _ModificationTime;

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

        public string Sender
        {
            get
            {
                return _Sender;
            }
            set
            {
                if (value != null)
                {
                    _Sender = value;
                }
                else
                {
                    throw new InvalidInputException("sender field is mandatory");
                }
            }
        }

        public string Receiver
        {
            set
            {
                if (value != null)
                {
                    _Receiver = value;
                }
                else
                {
                    throw new InvalidInputException("receiver field is mandatory");
                }
            }
            get
            {
                return _Receiver;
            }
        }

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

        public DateTime? CreationTime
        {
            get
            {
                return _CreationTime;
            }
            set
            {
                if (value != null)
                    _CreationTime = value;
            }
        }

        public DateTime? ModificationTime
        {
            set
            {
                if (value != null)
                    _ModificationTime = value;
            }
            get
            {
                return _ModificationTime;
            }
        }

        public override string ToString()
        {
            string info = "id " + Id + ", sender " + Sender + ", receiver " + Receiver + ", messageText " + MessageText;
            if (CreationTime != null)
            {
                info += ", creationTime " + CreationTime;
            }
            if (ModificationTime != null)
            {
                info += ", modificationTime " + ModificationTime;
            }
            return info;
        }
    }
}