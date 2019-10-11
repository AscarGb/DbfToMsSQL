using DbfToMsSQL.Loader;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdfToMsSQL
{
    public static class SQLHelper
    {
        public static string CreateTableSQL(DBFInfo r, string dbName)
        {
            StringBuilder s = new StringBuilder();

            s.Append($@"
{Environment.NewLine} USE {dbName + Environment.NewLine}
IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='{r.TableName}' AND XTYPE='U')
CREATE TABLE [dbo].[{r.TableName}] ({Environment.NewLine}");

            IEnumerable<string> fields = r.Fields.Select(field =>
            {

                StringBuilder fieldsBuilder = new StringBuilder();

                fieldsBuilder.Append($"\t   [{field.Name}] ");
                string sqlType = "";
                switch (field.Type)
                {
                    case 'L': sqlType = "bit"; break;
                    case 'D': sqlType = "date"; break;
                    case 'N':
                        {
                            if (field.Digits == 0)
                            {
                                sqlType = "int";
                            }
                            else
                            {
                                sqlType = $"numeric({(int)field.Size},{(int)field.Digits})";
                            }

                            break;
                        }
                    case 'F': sqlType = "double"; break;
                    default: sqlType = $"nvarchar({(int)field.Size})"; break;
                }
                fieldsBuilder.Append($"\t    { sqlType } null");

                return fieldsBuilder.ToString();
            });

            s.Append(string.Join("," + Environment.NewLine, fields));

            s.Append(")");

            return s.ToString();
        }

        public static async Task CreateTable(string text, string connestionString)
        {
            using (SqlConnection connection = new SqlConnection(connestionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(text, connection))
                {
                    await connection.OpenAsync();
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}