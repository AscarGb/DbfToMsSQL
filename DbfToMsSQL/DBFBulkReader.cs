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
        public string FileName = "";
        private int StepFormUpdate = 0;
        private MainForm Form;
        public string TableName;

        private delegate void FormMethod(int a);
        private delegate void FormMessage();

        private Encoding _encoding;
        private FileStream FS;
        private byte[] buffer;
        private readonly int FieldsLength;
        private readonly DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
        private readonly NumberFormatInfo numberFormat = new CultureInfo("en-US", false).NumberFormat;
        public string[] FieldName;
        public string[] FieldType;
        public byte[] FieldSize;
        public byte[] FieldDigs;
        public int RowsCount;
        private int ReadedRow = 0;
        private Dictionary<string, object> FieldValues = new Dictionary<string, object>();
        private readonly Dictionary<int, string> FieldIndex = new Dictionary<int, string>();

        //implement
        public int FieldCount { get; }
        public bool Read()
        {
            if (ReadedRow >= RowsCount)
            {
                return false;
            }

            FieldValues.Clear();
            buffer = new byte[FieldsLength];
            FS.ReadByte(); // Пропускаю стартовый байт элемента данных
            FS.Read(buffer, 0, buffer.Length);
            int Index = 0;
            for (int i = 0; i < FieldCount; i++)
            {
                string dbfValue = _encoding.GetString(buffer, Index, FieldSize[i]).TrimEnd((char)0x00, (char)0x20);

                Index = Index + FieldSize[i];
                object value;
                if (!string.IsNullOrEmpty(dbfValue.Trim()))
                {
                    switch (FieldType[i])
                    {
                        case "L": value = dbfValue == "T" ? true : false; break;
                        case "D": value = DateTime.ParseExact(dbfValue, "yyyyMMdd", dateTimeFormat); break;
                        case "N":
                            {
                                if (FieldDigs[i] == 0)
                                {
                                    value = int.Parse(dbfValue, numberFormat);
                                }
                                else
                                {
                                    value = decimal.Parse(dbfValue, numberFormat);
                                }

                                break;
                            }
                        case "F": value = double.Parse(dbfValue, numberFormat); break;
                        default: value = dbfValue; break;
                    }
                }
                else
                {
                    value = DBNull.Value;
                }
                FieldValues.Add(FieldName[i], value);
            }

            ReadedRow++;
            StepFormUpdate++;

            try
            {
                if (StepFormUpdate >= 20000 || ReadedRow >= RowsCount)
                {
                    Form.Invoke(new FormMethod(Form.SetStatus), new object[] { StepFormUpdate });
                    StepFormUpdate = 0;
                }
            }
            catch (Exception exc)
            {
                WriteException(exc);
            }

            return true;
        }
        public object GetValue(int i) { return FieldValues[FieldIndex[i]]; }
        public DBFReader(string fileName, Dictionary<int, string> FieldIndex, MainForm form, int encoding, string tableName)
        {
            try
            {
                _encoding = Encoding.GetEncoding(encoding);

                FileName = fileName;
                TableName = tableName;

                Form = form;

                FS = new FileStream(fileName, System.IO.FileMode.Open);
                buffer = new byte[4]; // Кол-во записей: 4 байтa, начиная с 5-го
                FS.Position = 4; FS.Read(buffer, 0, buffer.Length);
                RowsCount = buffer[0] + (buffer[1] * 0x100) + (buffer[2] * 0x10000) + (buffer[3] * 0x1000000);
                buffer = new byte[2]; // Кол-во полей: 2 байтa, начиная с 9-го
                FS.Position = 8; FS.Read(buffer, 0, buffer.Length);
                FieldCount = ((buffer[0] + (buffer[1] * 0x100) - 1) / 32) - 1;
                FieldName = new string[FieldCount]; // Массив названий полей
                FieldType = new string[FieldCount]; // Массив типов полей
                FieldSize = new byte[FieldCount]; // Массив размеров полей
                FieldDigs = new byte[FieldCount]; // Массив размеров дробной части
                buffer = new byte[32 * FieldCount]; // Описание полей: 32 байтa * кол-во, начиная с 33-го
                FS.Position = 32; FS.Read(buffer, 0, buffer.Length);
                FieldsLength = 0;
                for (int i = 0; i < FieldCount; i++)
                {
                    // Заголовки
                    FieldName[i] = Encoding.Default.GetString(buffer, i * 32, 10).TrimEnd(new char[] { (char)0x00 });
                    FieldType[i] = "" + (char)buffer[i * 32 + 11];
                    FieldSize[i] = buffer[i * 32 + 16];
                    FieldDigs[i] = buffer[i * 32 + 17];
                    FieldsLength = FieldsLength + FieldSize[i];
                }
                FS.ReadByte(); // Пропускаю разделитель схемы и данных

                this.FieldIndex = FieldIndex;
            }
            catch (Exception exc)
            {
                FS?.Dispose();
                WriteException(exc);
            }
        }

        public void Dispose()
        {
            try
            {
                FS?.Dispose();
            }
            catch (Exception exc)
            {
                WriteException(exc);
            }
        }

        private void WriteException(Exception exc)
        {
            Form.Invoke(new FormMessage(() => { Form.InfoListBox.Items.Add($"Error: {exc.Message }\r\n{ exc.InnerException?.ToString() ?? ""}"); }));
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
