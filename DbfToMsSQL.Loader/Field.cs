namespace DbfToMsSQL.Loader
{
    public class Field
    {
        public string Name { get; }
        public char Type { get; }
        public byte Size { get; }
        public byte Digits { get; }

        private readonly int _index;

        public int Index
        {
            get
            {
                if (!HasIndes)
                {
                    throw new IndexException("Index not set");
                }
                return _index;
            }
        }
        public bool HasIndes { get; }

        public Field(string Name, char Type, byte Size, byte Digits, int Index)
        {
            this.Name = Name;
            this.Type = Type;
            this.Size = Size;
            this.Digits = Digits;
            _index = Index;
            HasIndes = true;
        }

        public Field(string Name, char Type, byte Size, byte Digits)
        {
            this.Name = Name;
            this.Type = Type;
            this.Size = Size;
            this.Digits = Digits;
            HasIndes = false;
        }
    }
}
