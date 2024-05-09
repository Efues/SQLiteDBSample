using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteDBAccess
{
  internal class SQLGen
  {
    internal static string SelectTableList(string colName)
    {
      return $"select {colName} from sqlite_master where type='table';";
    }
    internal static string SelectTableInfo(string tableName)
    {
      return $"PRAGMA table_info({tableName});";
    }

    internal static string CreateTable(string tableName, Dictionary<string, string> columnList)
    {
      var sql = new StringBuilder();
      sql.Append($"create table \"{tableName}\" ");
      if(!columnList.Any()) return sql.ToString();

      sql.Append($"(");
      foreach ( var columnNameAndType in columnList )
      {
        sql.Append($"\"{columnNameAndType.Key}\" {columnNameAndType.Value},");
      }
      sql.Remove(sql.Length - 1,1);
      sql.Append($");");
      return sql.ToString();
    }

    internal static string AddColumn(string tableName, KeyValuePair<string, string> addingColumn)
    {
      return $"alter table {tableName} add column \"{addingColumn.Key}\" {addingColumn.Value};" ;
    }

    internal static string Insert(string tableName, List<string> cols, List<List<object>> rows)
    {
      var sql = new StringBuilder();
      sql.Append($"insert into {tableName} ");
      sql.Append("(");
      foreach (var col in cols)
      {
        sql.Append($"{col},");
      }
      sql.Remove(sql.Length - 1, 1);
      sql.Append($") values ");

      foreach (var row in rows)
      {
        sql.Append($"(");
        foreach (var val in row)
        {
          sql.Append($"'{val}',");
        }
        sql.Remove(sql.Length - 1, 1);
        sql.Append($"),");
      }
      sql.Remove(sql.Length - 1, 1);
      sql.Append($";");
      return sql.ToString();
    }

    internal static string Update(string tableName, string col, int defaultValue)
    {
      return $"update {tableName} set {col} = {defaultValue};";
    }
  }
}
