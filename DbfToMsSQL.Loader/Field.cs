namespace DbfToMsSQL.Loader
{
    public class Field
    {        
        public string Name { get; }        
        public char Type { get; }        
        public byte Size { get; }        
        public byte Digits { get; }        
        public Field(string Name,char Type, byte Size, byte Digits)
        {
            this.Name = Name;
            this.Type = Type;
            this.Size = Size;
            this.Digits = Digits;            
        }
    }
}
