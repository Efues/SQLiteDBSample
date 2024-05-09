using SQLiteDBAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddTableAndColumns
{
  internal class Updator
  {
    private string _dbFilePath;

    public Updator(string dbFilePath)
    {
      this._dbFilePath = dbFilePath;
    }

    public void Execute()
    {
      AddTableIfNotExist();
      AddColumnIfNotExist();
    }

    private void AddTableIfNotExist()
    {
      var addingTableName = "Customers";
      var columnList = new Dictionary<string, string>()
      {
        { "Name", "TEXT" },
        { "Address", "TEXT" },
      };

      var insertingRows = new List<List<object>>
      {
        new List<object> { "Yamada", "Hamamatsu" },
        new List<object> { "Suzuki", "Tokyo" }
      };

      using (var db = new SamepleDB(_dbFilePath))
      {
        var tableList = db.SelectTableList();
        if (!tableList.Contains(addingTableName))
        {
          db.CreateTable(addingTableName, columnList);
          var affected = db.InsertRows(addingTableName, columnList.Keys.ToList(), insertingRows);
        }
        else
        {
          return;
        }

        tableList = db.SelectTableList();
        if (tableList.Contains(addingTableName))
        {
          Console.WriteLine($"{addingTableName} table is added successfully.");
        }
        else
        {
          Console.WriteLine($"Failed to add {addingTableName} table.");
        }
      }
    }

    private void AddColumnIfNotExist()
    {
      var targetTableName = "Products";
      var addingColumn = new KeyValuePair<string, string>("Stock", "INTEGER");
      var defaultValue = 0;

      using (var db = new SamepleDB(_dbFilePath))
      {
        var columnList = db.SelectColumnList(targetTableName);
        if (!columnList.Contains(addingColumn.Key))
        {
          db.AddColumn(targetTableName, addingColumn);
          var affected = db.UpdateRows(targetTableName, addingColumn.Key, defaultValue);
        }
        else
        {
          return;
        }

        columnList = db.SelectColumnList(targetTableName);
        if (columnList.Contains(addingColumn.Key))
        {
          Console.WriteLine($"{addingColumn.Key} column is added successfully.");
        }
        else
        {
          Console.WriteLine($"Failed to add {addingColumn.Key} column.");
        }
      }
    }
  }
}
