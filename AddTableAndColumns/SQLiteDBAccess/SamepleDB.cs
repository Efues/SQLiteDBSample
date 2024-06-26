﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQLiteDBAccess
{
  public class SamepleDB : SQLiteDBBase
  {
    public SamepleDB(string DBFilePath) : base(DBFilePath)
    {
    }

    #region Select
    public IList<string> SelectTableList()
    {
      var colName = "name";
      var sql = SQLGen.SelectTableList(colName);
      var result = ReadToDataTable(sql);
      if (result == null) { return new List<string>(); }

      return result.AsEnumerable().Select(row => (string)row[colName]).ToList();

      /* doesn't work well....
      var metaTables = _dataContext.Mapping.GetTables();
      if (metaTables == null || !metaTables.Any()) return new List<string>();
      return metaTables.Select(item=> item.TableName).ToList();
      */
    }

    public IList<string> SelectColumnList(string targetTableName)
    {
      var colName = "name";
      var sql = SQLGen.SelectTableInfo(targetTableName);
      var result = ReadToDataTable(sql);
      if (result == null) { return new List<string>(); }

      return result.AsEnumerable().Select(row => (string)row[colName]).ToList();
    }
    #endregion

    #region NonQuery
    public int CreateTable(string tableName, Dictionary<string, string> columnList)
    {
      var sql = SQLGen.CreateTable(tableName, columnList);
      return ExecuteNonQuery(sql);
    }

    public int AddColumn(string tableName, KeyValuePair<string, string> addingColumn)
    {
      var sql = SQLGen.AddColumn(tableName, addingColumn);
      return ExecuteNonQuery(sql);
    }

    public int InsertRows(string tableName, List<string> cols, List<List<object>> rows)
    {
      if (!rows.Any()) return 0;
      var sql = SQLGen.Insert(tableName, cols, rows);
      return ExecuteNonQuery(sql);
    }

    public int UpdateRows(string tableName, string col, int defaultValue)
    {
      var sql = SQLGen.Update(tableName, col, defaultValue);
      return ExecuteNonQuery(sql);
    }
    #endregion
  }
}
