using System.IO;
using System.Text;

namespace DbfToMsSQL.Loader
{
    public class DBFInfo
    {
        public int FieldCount { get; }
        public int RowsCount { get; }
        public Field[] Fields { get; }
        public string FileName { get; }

        public string TableName { get; }

        private byte[] _buffer;

        public DBFInfo(string FileName)
        {
            this.FileName = FileName;

            TableName = Path.GetFileNameWithoutExtension(FileName);

            using (FileStream fileStream = new FileStream(FileName, FileMode.Open))
            {
                _buffer = new byte[4]; // Кол-во записей: 4 байтa, начиная с 5-го

                fileStream.Position = 4;
                fileStream.Read(_buffer, 0, _buffer.Length);

                RowsCount = _buffer[0] + (_buffer[1] * 0x100) + (_buffer[2] * 0x10000) + (_buffer[3] * 0x1000000);

                _buffer = new byte[2]; // Кол-во полей: 2 байтa, начиная с 9-го
                fileStream.Position = 8;
                fileStream.Read(_buffer, 0, _buffer.Length);

                FieldCount = ((_buffer[0] + (_buffer[1] * 0x100) - 1) / 32) - 1;

                Fields = new Field[FieldCount];

                _buffer = new byte[32 * FieldCount]; // Описание полей: 32 байтa * кол-во, начиная с 33-го

                fileStream.Position = 32;
                fileStream.Read(_buffer, 0, _buffer.Length);

                for (int i = 0; i < FieldCount; i++)
                {
                    string name = Encoding.Default.GetString(_buffer, i * 32, 10)
                        .TrimEnd(new char[] { (char)0x00 });

                    char type = (char)_buffer[i * 32 + 11];
                    byte size = _buffer[i * 32 + 16];
                    byte digs = _buffer[i * 32 + 17];

                    Fields[i] = new Field(name, type, size, digs);
                }
            }
        }
    }
}