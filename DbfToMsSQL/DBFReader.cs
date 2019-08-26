using DbfToMsSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace BdfToMsSQL
{
    public class DBFReader : IDataReader
    {
        public string FileName { get; set; } = "";
        public string TableName { get; set; }
        public string[] FieldName { get; set; }
        public string[] FieldType { get; set; }
        public byte[] FieldSize { get; set; }
        public byte[] FieldDigs { get; set; }
        public int RowsCount { get; set; }
        public int FieldCount { get; }

        private delegate void FormMethod(int a);
        private delegate void FormMessage();

        private int _stepFormUpdate = 0;
        private MainForm _form;
        private Encoding _encoding;
        private FileStream _fileStream;
        private byte[] _buffer;
        private readonly int _fieldsLength;
        private readonly DateTimeFormatInfo _dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
        private readonly NumberFormatInfo _numberFormat = new CultureInfo("en-US", false).NumberFormat;
        private int _readedRow = 0;
        private Dictionary<string, object> _fieldValues = new Dictionary<string, object>();
        private readonly Dictionary<int, string> _fieldIndex = new Dictionary<int, string>();

        //implement
        public bool Read()
        {
            if (_readedRow >= RowsCount)
            {
                return false;
            }

            _fieldValues.Clear();
            _buffer = new byte[_fieldsLength];
            _fileStream.ReadByte(); // Пропускаю стартовый байт элемента данных
            _fileStream.Read(_buffer, 0, _buffer.Length);
            int Index = 0;
            for (int i = 0; i < FieldCount; i++)
            {
                string dbfValue = _encoding.GetString(_buffer, Index, FieldSize[i]).TrimEnd((char)0x00, (char)0x20);

                Index = Index + FieldSize[i];
                object value;
                if (!string.IsNullOrEmpty(dbfValue.Trim()))
                {
                    switch (FieldType[i])
                    {
                        case "L": value = dbfValue == "T" ? true : false; break;
                        case "D": value = DateTime.ParseExact(dbfValue, "yyyyMMdd", _dateTimeFormat); break;
                        case "N":
                            {
                                if (FieldDigs[i] == 0)
                                {
                                    value = int.Parse(dbfValue, _numberFormat);
                                }
                                else
                                {
                                    value = decimal.Parse(dbfValue, _numberFormat);
                                }

                                break;
                            }
                        case "F": value = double.Parse(dbfValue, _numberFormat); break;
                        default: value = dbfValue; break;
                    }
                }
                else
                {
                    value = DBNull.Value;
                }
                _fieldValues.Add(FieldName[i], value);
            }

            _readedRow++;
            _stepFormUpdate++;

            try
            {
                if (_stepFormUpdate >= 20000 || _readedRow >= RowsCount)
                {
                    _form.Invoke(new FormMethod(_form.SetStatus), new object[] { _stepFormUpdate });
                    _stepFormUpdate = 0;
                }
            }
            catch (Exception exc)
            {
                WriteException(exc);
            }

            return true;
        }
        public object GetValue(int i) { return _fieldValues[_fieldIndex[i]]; }
        public DBFReader(string fileName, Dictionary<int, string> FieldIndex, MainForm form, int encoding, string tableName)
        {
            try
            {
                _encoding = Encoding.GetEncoding(encoding);

                FileName = fileName;
                TableName = tableName;

                _form = form;

                _fileStream = new FileStream(fileName, System.IO.FileMode.Open);
                _buffer = new byte[4]; // Кол-во записей: 4 байтa, начиная с 5-го
                _fileStream.Position = 4; _fileStream.Read(_buffer, 0, _buffer.Length);
                RowsCount = _buffer[0] + (_buffer[1] * 0x100) + (_buffer[2] * 0x10000) + (_buffer[3] * 0x1000000);
                _buffer = new byte[2]; // Кол-во полей: 2 байтa, начиная с 9-го
                _fileStream.Position = 8; _fileStream.Read(_buffer, 0, _buffer.Length);
                FieldCount = ((_buffer[0] + (_buffer[1] * 0x100) - 1) / 32) - 1;
                FieldName = new string[FieldCount]; // Массив названий полей
                FieldType = new string[FieldCount]; // Массив типов полей
                FieldSize = new byte[FieldCount]; // Массив размеров полей
                FieldDigs = new byte[FieldCount]; // Массив размеров дробной части
                _buffer = new byte[32 * FieldCount]; // Описание полей: 32 байтa * кол-во, начиная с 33-го
                _fileStream.Position = 32; _fileStream.Read(_buffer, 0, _buffer.Length);
                _fieldsLength = 0;
                for (int i = 0; i < FieldCount; i++)
                {
                    // Заголовки
                    FieldName[i] = Encoding.Default.GetString(_buffer, i * 32, 10).TrimEnd(new char[] { (char)0x00 });
                    FieldType[i] = "" + (char)_buffer[i * 32 + 11];
                    FieldSize[i] = _buffer[i * 32 + 16];
                    FieldDigs[i] = _buffer[i * 32 + 17];
                    _fieldsLength = _fieldsLength + FieldSize[i];
                }
                _fileStream.ReadByte(); // Пропускаю разделитель схемы и данных

                this._fieldIndex = FieldIndex;
            }
            catch (Exception exc)
            {
                _fileStream?.Dispose();
                WriteException(exc);
            }
        }

        public void Dispose()
        {
            try
            {
                _fileStream?.Dispose();
            }
            catch (Exception exc)
            {
                WriteException(exc);
            }
        }

        private void WriteException(Exception exc)
        {
            _form.Invoke(new FormMessage(() => { _form.InfoListBox.Items.Add($"Error: {exc.Message }\r\n{ exc.InnerException?.ToString() ?? ""}"); }));
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
