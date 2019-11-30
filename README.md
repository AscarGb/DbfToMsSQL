### DbfToMsSQL
App for loading large dbf files into ms sql server database via SqlBulkCopy
https://github.com/AscarGb/DbfToMsSQL/releases

### DbfToMsSQL.Loader
Only library

[![NuGet](https://img.shields.io/badge/nuget-1.2.0-blue)](https://www.nuget.org/packages/DbfToMsSQL.Loader/)

## Usage

```
using (SqlConnection connection = new SqlConnection(ConnestionString))
{
  await connection.OpenAsync();  
  using (DBFReader reader = new DBFReader("FileName.DBF", 866, "TableName"))
  {
    reader.OnLoad += BulkReader_OnLoad;
    using (SqlBulkCopy loader = new SqlBulkCopy(connection))
    {
      loader.DestinationTableName = "TableName";
      loader.BulkCopyTimeout = int.MaxValue;
      await loader.WriteToServerAsync(reader);
    }
  }
}
```
