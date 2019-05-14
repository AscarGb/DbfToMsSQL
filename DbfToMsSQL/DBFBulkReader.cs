using DbfToMsSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdfToMsSQL
{
    public class DBFBulkReader : IDataReader
    {
        public string FileName = "";
        int StepFormUpdate = 0;
        Form1 form;
        private delegate void FormMethod(int a);
        private delegate void FormMessage();
        System.IO.FileStream FS;
        byte[] buffer;
        int _FieldCount;
        int FieldsLength;
        System.Globalization.DateTimeFormatInfo dfi = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        public string[] FieldName;
        public string[] FieldType;
        public byte[] FieldSize;
        public byte[] FieldDigs;
        public int RowsCount;
        int ReadedRow = 0;
        Dictionary<string, object> R = new Dictionary<string, object>();
        Dictionary<int, string> FieldIndex = new Dictionary<int, string>();

        //implement
        public int FieldCount { get { return _FieldCount; } }
        public bool Read()
        {
            if (ReadedRow >= RowsCount) return false;

            R.Clear();
            buffer = new byte[FieldsLength];
            FS.ReadByte(); // Пропускаю стартовый байт элемента данных
            FS.Read(buffer, 0, buffer.Length);
            int Index = 0;
            for (int i = 0; i < FieldCount; i++)
            {
                string l = System.Text.Encoding.GetEncoding(866).GetString(buffer, Index, FieldSize[i]).TrimEnd(new char[] { (char)0x00 }).TrimEnd(new char[] { (char)0x20 });
                Index = Index + FieldSize[i];
                object Tr;
                if (l.Trim() != "")
                {
                    switch (FieldType[i])
                    {
                        case "L": Tr = l == "T" ? true : false; break;
                        case "D": Tr = DateTime.ParseExact(l, "yyyyMMdd", dfi); break;
                        case "N":
                            {
                                if (FieldDigs[i] == 0)
                                    Tr = int.Parse(l, nfi);
                                else
                                    Tr = decimal.Parse(l, nfi);
                                break;
                            }
                        case "F": Tr = double.Parse(l, nfi); break;
                        default: Tr = l; break;
                    }
                }
                else
                {
                    Tr = DBNull.Value;
                }
                R.Add(FieldName[i], Tr);
            }
            ReadedRow++;

            StepFormUpdate++;
            try
            {
                if (StepFormUpdate >= 20000 || ReadedRow >= RowsCount)
                {
                    form.Invoke(new FormMethod(form.SetStatus), new object[] { StepFormUpdate });
                    StepFormUpdate = 0;
                }
            }
            catch { }

            return true;
        }
        public object GetValue(int i) { return R[FieldIndex[i]]; }
        public DBFBulkReader(string FileName, Dictionary<int, string> FieldIndex, Form1 form)
        {
            this.FileName = FileName;
            this.form = form;

            FS = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
            buffer = new byte[4]; // Кол-во записей: 4 байтa, начиная с 5-го
            FS.Position = 4; FS.Read(buffer, 0, buffer.Length);
            RowsCount = buffer[0] + (buffer[1] * 0x100) + (buffer[2] * 0x10000) + (buffer[3] * 0x1000000);
            buffer = new byte[2]; // Кол-во полей: 2 байтa, начиная с 9-го
            FS.Position = 8; FS.Read(buffer, 0, buffer.Length);
            _FieldCount = (((buffer[0] + (buffer[1] * 0x100)) - 1) / 32) - 1;
            FieldName = new string[_FieldCount]; // Массив названий полей
            FieldType = new string[_FieldCount]; // Массив типов полей
            FieldSize = new byte[_FieldCount]; // Массив размеров полей
            FieldDigs = new byte[_FieldCount]; // Массив размеров дробной части
            buffer = new byte[32 * _FieldCount]; // Описание полей: 32 байтa * кол-во, начиная с 33-го
            FS.Position = 32; FS.Read(buffer, 0, buffer.Length);
            FieldsLength = 0;
            for (int i = 0; i < _FieldCount; i++)
            {
                // Заголовки
                FieldName[i] = System.Text.Encoding.Default.GetString(buffer, i * 32, 10).TrimEnd(new char[] { (char)0x00 });
                FieldType[i] = "" + (char)buffer[i * 32 + 11];
                FieldSize[i] = buffer[i * 32 + 16];
                FieldDigs[i] = buffer[i * 32 + 17];
                FieldsLength = FieldsLength + FieldSize[i];
            }
            FS.ReadByte(); // Пропускаю разделитель схемы и данных

            this.FieldIndex = FieldIndex;
        }        
        public void Dispose()
        {
            try { FS.Close(); }
            catch (Exception exc)
            {
                form.Invoke(new FormMessage(() => { form.listBox_validate_errors.Items.Add("Error: " + exc.Message + "\r\n" + exc.InnerException); }));
            }
        }

        //bulk not use this
        public int Depth { get { return -1; } }
        public bool IsClosed { get { return false; } }
        public Object this[int i] { get { return new object(); } }
        public Object this[string name] { get { return new object(); } }
        public int RecordsAffected { get { return -1; } }
        public void Close() { }
        public bool NextResult() { return true; }
        public bool IsDBNull(int i) { return false; }
        public string GetString(int i) { return ""; }
        public DataTable GetSchemaTable() { return null; }
        public int GetOrdinal(string name) { return -1; }
        public string GetName(int i) { return ""; }
        public long GetInt64(int i) { return -1; }
        public int GetInt32(int i) { return -1; }
        public short GetInt16(int i) { return -1; }
        public Guid GetGuid(int i) { return new Guid(); }
        public float GetFloat(int i) { return -1; }
        public Type GetFieldType(int i) { return typeof(string); }
        public double GetDouble(int i) { return -1; }
        public decimal GetDecimal(int i) { return -1; }
        public DateTime GetDateTime(int i) { return new DateTime(); }
        public string GetDataTypeName(int i) { return ""; }
        public IDataReader GetData(int i) { return this; }
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) { return -1; }
        public char GetChar(int i) { return ' '; }
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) { return -1; }
        public byte GetByte(int i) { return 0x00; }
        public bool GetBoolean(int i) { return false; }
        public int GetValues(Object[] values) { return -1; }
    }
}
