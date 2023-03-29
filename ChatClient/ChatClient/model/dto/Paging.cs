namespace ChatClient.model.dto
{
    internal class Paging
    {
        private int? _From;

        private int? _Size;

        public int? From
        {
            get
            {
                return _From;
            }
            set
            {
                if (value != null)
                {
                    if (value >= 0)
                    {
                        _From = value;
                    }
                    else
                    {
                        throw new InvalidInputException("from parameter must be positive or zero");
                    }
                }
                else
                {
                    throw new InvalidInputException("from parameter can't be null");
                }
            }
        }

        public int? Size
        {
            get
            {
                return _Size;
            }
            set
            {
                if (value != null)
                {
                    if (value > 0)
                    {
                        _Size = value;
                    }
                    else
                    {
                        throw new InvalidInputException("size parameter must be positive");
                    }
                }
                else
                {
                    throw new InvalidInputException("size parameter can't be null");
                }
            }
        }

        public Paging()
        {
        }

        public Paging(int from, int size)
        {
            From = from;
            Size = size;
        }
    }
}