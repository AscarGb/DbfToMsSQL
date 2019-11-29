using BdfToMsSQL.Loader;
using DbfToMsSQL.Loader;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace BdfToMsSQL.Loader
{
    public class DBFReader : IDataReader
    {
        public int FieldCount { get; }
        public event Action<int> OnLoad;

        public int RowsCount { get; }

        private int _readedRow = 0;
        private int _step = 0;
        private byte[] _buffer;

        private readonly string _fileName;
        private readonly int _fieldsLength;
        private readonly object[] _fieldValues;
        private readonly Field[] _fields;
        private readonly Encoding _encoding;
        private readonly FileStream _fileStream;
        private readonly CultureInfo _cultureInfo;
        private readonly DateTimeFormatInfo _dateTimeFormat;
        private readonly NumberFormatInfo _numberFormat;

        private bool IsComplete()
        {
            return _readedRow >= RowsCount;
        }

        public bool Read()
        {
            if (IsComplete())
            {
                return false;
            }

            _fileStream.ReadByte(); // Пропускаю стартовый байт элемента данных
            _fileStream.Read(_buffer, 0, _buffer.Length);

            int Index = 0;
            object value;
            Field field;
            string dbfValue;

            for (int i = 0; i < _fields.Length; i++)
            {
                field = _fields[i];

                dbfValue = _encoding.GetString(_buffer, Index, field.Size)
                    .TrimEnd((char)0x00, (char)0x20);

                Index += field.Size;

                if (!string.IsNullOrEmpty(dbfValue))
                {
                    switch (field.Type)
                    {
                        case 'L':
                            value = StringComparer.Ordinal.Equals(dbfValue, "T");
                            break;

                        case 'D':
                            value = DateTime.ParseExact(dbfValue, "yyyyMMdd", _dateTimeFormat);
                            break;

                        case 'N':
                            {
                                if (field.Digits == 0)
                                {
                                    value = int.Parse(dbfValue, _numberFormat);
                                }
                                else
                                {
                                    value = decimal.Parse(dbfValue, _numberFormat);
                                }
                            }
                            break;

                        case 'F':
                            value = double.Parse(dbfValue, _numberFormat);
                            break;

                        default:
                            value = dbfValue;
                            break;
                    }
                }
                else
                {
                    value = DBNull.Value;
                }

                _fieldValues[i] = value;
            }

            _readedRow++;
            _step++;

            if (_step >= 20000 || IsComplete())
            {
                OnLoad?.Invoke(_step);
                _step = 0;
            }

            return true;
        }
        public object GetValue(int i)
        {
            return _fieldValues[i];
        }

        public DBFReader(
            string FileName,
            int encoding,
            string TableName)
        {
            _fileName = FileName;

            try
            {
                _cultureInfo = new CultureInfo("en-US", false);
                _dateTimeFormat = _cultureInfo.DateTimeFormat;
                _numberFormat = _cultureInfo.NumberFormat;
                _encoding = Encoding.GetEncoding(encoding);

                _fileStream = new FileStream(FileName, FileMode.Open);
                _buffer = new byte[4]; // Кол-во записей: 4 байтa, начиная с 5-го

                _fileStream.Position = 4;
                _fileStream.Read(_buffer, 0, _buffer.Length);

                RowsCount = _buffer[0] + (_buffer[1] * 0x100) + (_buffer[2] * 0x10000) + (_buffer[3] * 0x1000000);

                _buffer = new byte[2]; // Кол-во полей: 2 байтa, начиная с 9-го
                _fileStream.Position = 8;
                _fileStream.Read(_buffer, 0, _buffer.Length);

                FieldCount = ((_buffer[0] + (_buffer[1] * 0x100) - 1) / 32) - 1;

                _fields = new Field[FieldCount];

                _buffer = new byte[32 * FieldCount]; // Описание полей: 32 байтa * кол-во, начиная с 33-го

                _fileStream.Position = 32;
                _fileStream.Read(_buffer, 0, _buffer.Length);

                _fieldsLength = 0;

                for (int i = 0; i < FieldCount; i++)
                {
                    string name = Encoding.Default.GetString(_buffer, i * 32, 10)
                        .TrimEnd(new char[] { (char)0x00 });

                    char type = (char)_buffer[i * 32 + 11];
                    byte size = _buffer[i * 32 + 16];
                    byte digs = _buffer[i * 32 + 17];

                    _fields[i] = new Field(name, type, size, digs);

                    _fieldsLength += size;
                }
                _fileStream.ReadByte(); // Пропускаю разделитель схемы и данных              

                _fieldValues = new object[_fields.Length];

                _buffer = new byte[_fieldsLength];
            }
            catch (Exception e)
            {
                Dispose();
                throw new CreateReaderException("Dbf reader creation failed", e);
            }
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                _fileStream?.Dispose();
                disposed = true;
            }
        }

        #region not use 
        public int Depth => throw new NotImplementedException();
        public bool IsClosed => throw new NotImplementedException();
        public object this[int i] => throw new NotImplementedException();
        public object this[string name] => throw new NotImplementedException();
        public int RecordsAffected => throw new NotImplementedException();
        public void Close() { throw new NotImplementedException(); }
        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}